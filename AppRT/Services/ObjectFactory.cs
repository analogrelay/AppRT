using System;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Hosting;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AppRT.Conventions;
using AppRT.Messaging;
using ReactiveUI;
using Windows.UI.Xaml;

namespace AppRT.Services
{
    [Shared]
    [Export]
    [Export(typeof(IMessageSink))]
    public class ObjectFactory : IMessageSink
    {
        private Dictionary<Type, Func<object>> _factories = new Dictionary<Type, Func<object>>();

        public ConventionManager Conventions { get; private set; }
        public CompositionHost Container { get; private set; }

        [ImportingConstructor]
        public ObjectFactory(ConventionManager conventions)
        {
            Conventions = conventions;
        }

        public UIElement CreateViewForViewModel(Type viewModel)
        {
            Type viewType = Conventions.ViewModelToView.GetViewForViewModel(viewModel);
            return GetService<UIElement>(viewType);
        }

        public object CreateViewModelForView(Type view)
        {
            Type viewModelType = Conventions.ViewToViewModel.GetViewModelForView(view);
            return GetService(viewModelType);
        }

        public object GetService(Type type)
        {
            // Try to resolve it as a service
            object service;
            if (Container.TryGetExport(type, out service))
            {
                return service;
            }

            Func<object> factory;
            if (!_factories.TryGetValue(type, out factory))
            {
                factory = Expression.Lambda<Func<object>>(
                    Expression.New(type)).Compile();
                _factories[type] = factory;
            }
            return factory();
        }

        public T GetService<T>()
        {
            return (T)GetService(typeof(T));
        }

        public T GetService<T>(Type type)
        {
            return (T)GetService(type);
        }

        public void Register(IMessageBus bus)
        {
            bus.Listen<ApplicationInitializedMessage>()
               .Subscribe(m => Container = m.Container);
        }

        public void SatisfyImports(object objectWithLooseImports)
        {
            if (Container != null)
            {
                Container.SatisfyImports(objectWithLooseImports);
            }
        }

        public IEnumerable<T> GetServices<T>()
        {
            return Container == null ?
                Enumerable.Empty<T>() :
                Container.GetExports<T>();
        }
    }
}
