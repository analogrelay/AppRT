using AppRT.ViewModels;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace AppRT.Xaml
{
    public abstract class ViewPage : Page, IView
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(object), typeof(ViewPage), new PropertyMetadata(null));

        public object ViewModel
        {
            get { return GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public Type ViewModelType { get; set; }

        public ViewPage()
        {
            DefaultStyleKey = typeof(ViewPage);
            ViewHelpers.InitializeView(this);
        }

        protected override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var navigable = ViewModel as INavigableViewModel;
            if (navigable != null)
            {
                navigable.OnNavigatedTo(e);
            }
        }
    }
}
