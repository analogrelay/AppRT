using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AppRT.Conventions;
using Windows.UI.Xaml;

namespace AppRT.Services
{
    [Shared]
    [Export]
    public class ObjectFactory
    {
        private Dictionary<Type, Func<object>> _factories = new Dictionary<Type, Func<object>>();

        public ConventionManager Conventions { get; private set; }

        [ImportingConstructor]
        public ObjectFactory(ConventionManager conventions)
        {
            Conventions = conventions;
        }

        public UIElement CreateViewForViewModel(Type viewModel)
        {
            Type viewType = Conventions.ViewModelToView.GetViewForViewModel(viewModel);
            return Construct<UIElement>(viewType);
        }

        public object Construct(Type type)
        {
            Func<object> factory;
            if (!_factories.TryGetValue(type, out factory))
            {
                factory = Expression.Lambda<Func<object>>(
                    Expression.New(type)).Compile();
                _factories[type] = factory;
            }
            return factory();
        }

        public T Construct<T>(Type type)
        {
            return (T)Construct(type);
        }
    }
}
