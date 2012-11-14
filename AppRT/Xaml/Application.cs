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
using AppRT.Services;

namespace AppRT.Xaml
{
    public abstract class Application : Windows.UI.Xaml.Application
    {
        private IEnumerable<ApplicationPlugin> _plugins;
        private CompositionHost _container;

        [ImportMany]
        public IList<IMessageSink> Sinks { get; set; }

        [Import]
        public IMessageBus Bus { get; set; }

        [Import]
        public ObjectFactory Factory { get; set; }

        public static new Application Current
        {
            get { return Windows.UI.Xaml.Application.Current as AppRT.Xaml.Application; }
        }

        protected abstract Type MainViewModelType { get; }

        protected Application(params ApplicationPlugin[] plugins)
        {
            _plugins = plugins;
            Compose();
            Suspending += OnSuspending;
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

            Bus.SendMessage(new ApplicationInitializedMessage(rootFrame, _container));

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
