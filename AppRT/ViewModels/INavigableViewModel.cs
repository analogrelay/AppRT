using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace AppRT.ViewModels
{
    public interface INavigableViewModel
    {
        void OnNavigatedTo(NavigationEventArgs e);
    }
}
