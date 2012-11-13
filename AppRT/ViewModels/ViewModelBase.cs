using ReactiveUI;
using ReactiveUI.Xaml;
using System;
using System.Reactive.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Navigation;

namespace AppRT.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        protected void SetProperty<T>(ref T backingField, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (propertyName == null)
            {
                throw new InvalidOperationException("If you aren't compiling with support for CallerMemberName, the 'propertyName' argument must be provided");
            }
            if (!Equals(backingField, newValue))
            {
                this.raisePropertyChanging(propertyName);
                backingField = newValue;
                this.raisePropertyChanged(propertyName);
            }
        }
    }

    public abstract class NavigableViewModelBase<TNavParam> : ViewModelBase, INavigableViewModel
    {
        public void OnNavigatedTo(NavigationEventArgs e)
        {
            OnNavigatedTo(e.NavigationMode, (TNavParam)e.Parameter);
        }

        protected abstract void OnNavigatedTo(NavigationMode mode, TNavParam param);
    }

    public abstract class NavigableViewModelBase : NavigableViewModelBase<object>
    {
        protected override void OnNavigatedTo(NavigationMode mode, object param)
        {
            OnNavigatedTo(mode);
        }

        protected abstract void OnNavigatedTo(NavigationMode mode);
    }

    public abstract class InvocableViewModelBase : ViewModelBase, IInvocableViewModel
    {
        private ICommand _invokedCommand;
        private BehaviorSubject<bool> _canInvoke = new BehaviorSubject<bool>(true);
        public ICommand InvokedCommand
        {
            get { return _invokedCommand ?? (_invokedCommand = CreateInvokedCommand()); }
        }

        protected bool CanInvoke
        {
            get { return _canInvoke.FirstAsync().Wait(); }
            set { _canInvoke.OnNext(value); }
        }

        private ICommand CreateInvokedCommand()
        {
            var cmd = new ReactiveCommand(_canInvoke);
            cmd.Subscribe(OnInvoked);
            return cmd;
        }

        protected abstract void OnInvoked(object parameter);
    }

    public abstract class DialogViewModelBase : ViewModelBase, IDialogViewModel
    {
        private bool? _result;
        public bool? Result
        {
            get { return _result; }
            set { SetProperty(ref _result, value); }
        }
    }
}
