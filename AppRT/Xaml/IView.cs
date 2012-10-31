using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRT.Xaml
{
    public interface IView
    {
        Type ViewModelType { get; set; }
        object ViewModel { get; set; }
    }
}
