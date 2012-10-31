using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppRT.Conventions
{
    public class ViewToViewModelConvention
    {
        public virtual Type GetViewModelForView(Type view)
        {
            return view.GetTypeInfo().Assembly.GetType(view.FullName.Replace("View", "ViewModel"));
        }
    }
}
