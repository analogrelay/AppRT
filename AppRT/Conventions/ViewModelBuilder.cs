using AppRT.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace AppRT.Conventions
{
    public class ViewModelBuilder
    {
        private Dictionary<Type, Func<object>> _cache = new Dictionary<Type, Func<object>>();

        public virtual object ConstructViewModel(Type viewModelType)
        {
            return Application.GetService(viewModelType);
        }
    }
}
