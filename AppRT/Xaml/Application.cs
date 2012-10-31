using System;
using System.Reflection;
using System.Collections.Generic;
using System.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using System.Composition;
using ReactiveUI;
using Windows.ApplicationModel;
using AppRT.Messaging;

namespace AppRT.Xaml
{
    public abstract class Application : Windows.UI.Xaml.Application
    {
        private static DesignModeApplication _designModeApp;
        private IEnumerable<ApplicationPlugin> _plugins;
        private CompositionHost _container;

        [ImportMany]
        public IList<IMessageSink> Sinks { get; set; }

        [Import]
        public IMessageBus Bus { get; set; }

        protected abstract Type MainViewModelType { get; }

        protected Application(params ApplicationPlugin[] plugins)
        {
            _plugins = plugins;
            Compose();
            Suspending += OnSuspending;
        }

        public static void SatisfyImports(object objectWithLooseImports)
        {
            if (DesignMode.DesignModeEnabled)
            {
                if (_designModeApp == null)
                {
                    _designModeApp = new DesignModeApplication();
                }
                _designModeApp.SatisfyImports(objectWithLooseImports);
            }
            else
            {
                var app = Windows.UI.Xaml.Application.Current as Application;
                if (app == null)
                {
                    throw new InvalidOperationException("Cannot SatisfyImports, the Application doesn't inherit from AppRT.Application!");
                }
                app._container.SatisfyImports(objectWithLooseImports);
            }
        }

        public static object GetService(Type serviceType)
        {
            if (DesignMode.DesignModeEnabled)
            {
                return Activator.CreateInstance(serviceType);
            }
            else
            {
                var app = Windows.UI.Xaml.Application.Current as Application;
                if (app == null)
                {
                    throw new InvalidOperationException("Cannot GetService, the Application doesn't inherit from AppRT.Application!");
                }
                try
                {
                    return app._container.GetExport(serviceType);
                }
                catch (CompositionFailedException cfex)
                {
                    ExceptionReporter.Report(cfex);
                    return null;
                }
            }
        }

        public static T GetService<T>()
        {
            return (T)GetService(typeof(T));
        }

        public static IEnumerable<T> GetServices<T>()
        {
            if (DesignMode.DesignModeEnabled)
            {
                return Enumerable.Empty<T>();
            }
            else
            {
                var app = Windows.UI.Xaml.Application.Current as Application;
                if (app == null)
                {
                    throw new InvalidOperationException("Cannot GetService, the Application doesn't inherit from AppRT.Application!");
                }
                return app._container.GetExports<T>();
            }
        }

        protected void Compose()
        {
            // Create the container
            var config = new ContainerConfiguration();
            ConfigureContainer(config);
            _container = config.CreateContainer();

            // Compose the application
            _container.SatisfyImports(this);
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            base.OnLaunched(args);

            InitializeMessageBus();

            // Initialize the navigation frame if necessary
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                rootFrame = new Frame();
                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    RestoreState();
                }
                Window.Current.Content = rootFrame;
            }

            Bus.SendMessage(new ApplicationSitedMessage(rootFrame));

            // Initialize the main view, if we haven't already restored a saved navigation stack
            if (rootFrame.Content == null)
            {
                Bus.SendMessage(new NavigateMessage(MainViewModelType));
            }

            Window.Current.Activate();
        }

        protected virtual void RestoreState()
        {
        }

        protected virtual void SaveState()
        {
        }

        protected virtual void ConfigureContainer(ContainerConfiguration config)
        {
            config.WithAssembly(typeof(Application).GetTypeInfo().Assembly);
            config.WithAssembly(GetType().GetTypeInfo().Assembly);
            EachPlugin(p => p.ConfigureContainer(config));
        }

        private void InitializeMessageBus()
        {
            foreach (var sink in Sinks)
            {
                sink.Register(Bus);
            }
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            try
            {
                SaveState();
            }
            finally
            {
                deferral.Complete();
            }
        }

        private void EachPlugin(Action<ApplicationPlugin> action)
        {
            foreach (var plugin in _plugins)
            {
                action(plugin);
            }
        }
    }
}
