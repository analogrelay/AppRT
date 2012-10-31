using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AppRT.Xaml
{
    public class ViewUserControl : UserControl, IView
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(object), typeof(ViewPage), new PropertyMetadata(null));

        public object ViewModel
        {
            get { return GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public Type ViewModelType { get; set; }

        public ViewUserControl()
        {
            ViewHelpers.InitializeView(this);
        }
    }
}
