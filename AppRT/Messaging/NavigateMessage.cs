using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.UI.Xaml.Navigation;

namespace AppRT.Messaging
{
    public class NavigateMessage
    {
        public NavigationMode Mode { get; private set; }
        public Type ViewModelType { get; private set; }
        public object Parameter { get; private set; }

        public NavigateMessage(NavigationMode mode)
        {
            Mode = mode;
            ViewModelType = null;
            Parameter = null;
        }

        public NavigateMessage(Type viewModelType) : this(viewModelType, null) { }
        public NavigateMessage(Type viewModelType, object parameter)
        {
            Mode = NavigationMode.New;
            ViewModelType = viewModelType;
            Parameter = parameter;
        }
    }
}
