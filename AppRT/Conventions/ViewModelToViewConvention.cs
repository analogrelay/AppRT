using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppRT.Conventions
{
    public class ViewModelToViewConvention
    {
        public virtual Type GetViewForViewModel(Type viewModel)
        {
            return viewModel.GetTypeInfo().Assembly.GetType(viewModel.FullName.Replace("ViewModel", "View"));
        }
    }
}
