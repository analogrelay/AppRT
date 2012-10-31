using AppRT.Services;
using AppRT.Xaml;
using ReactiveUI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel;
using Windows.UI.Xaml.Navigation;

namespace AppRT.ViewModels
{
    public interface IPageViewModel
    {
        ICommand GoBackCommand { get; }
    }

    public class PageViewModelBase : NavigableViewModelBase, IPageViewModel
    {
        private ReactiveCommand _goBackCommand;

        public ICommand GoBackCommand
        {
            get { return _goBackCommand; }
        }

        protected PageViewModelBase() : this(DesignMode.DesignModeEnabled ? null : Application.GetService<NavigationService>()) { }
        protected PageViewModelBase(NavigationService nav)
        {
            if (nav != null)
            {
                _goBackCommand = ReactiveCommand.Create(
                    _ => nav.CanGoBack,
                    _ => nav.GoBack());
            }
        }

        protected override void OnNavigatedTo(NavigationMode mode)
        {
        }
    }

    public class PageViewModelBase<T> : NavigableViewModelBase<T>, IPageViewModel
    {
        private ReactiveCommand _goBackCommand;

        public ICommand GoBackCommand
        {
            get { return _goBackCommand; }
        }

        protected PageViewModelBase() : this(DesignMode.DesignModeEnabled ? null : Application.GetService<NavigationService>()) { }
        protected PageViewModelBase(NavigationService nav)
        {
            if (nav != null)
            {
                _goBackCommand = ReactiveCommand.Create(
                    _ => nav.CanGoBack,
                    _ => nav.GoBack());
            }
        }

        protected override void OnNavigatedTo(NavigationMode mode, T param)
        {
        }
    }
}
