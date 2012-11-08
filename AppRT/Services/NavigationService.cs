using AppRT.Conventions;
using AppRT.Messaging;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace AppRT.Services
{
    [Shared]
    [Export]
    [Export(typeof(IMessageSink))]
    public class NavigationService : IMessageSink
    {
        private HostService Host { get; set; }
        private ConventionManager Conventions { get; set; }

        public bool CanGoBack { get { return Host.Frame.CanGoBack; } }
        public bool CanGoForward { get { return Host.Frame.CanGoForward; } }

        [ImportingConstructor]
        public NavigationService(HostService host, ConventionManager conventions)
        {
            Conventions = conventions;
        }

        public void Register(IMessageBus bus)
        {
            // Listen for the message which has us navigate.
            bus.Listen<NavigateMessage>()
               .ObserveOn(RxApp.DeferredScheduler)
               .Subscribe(OnNavigateMessage);
        }

        public void GoBack()
        {
            Host.Frame.GoBack();
        }

        public void GoForward()
        {
            Host.Frame.GoForward();
        }

        private void OnNavigateMessage(NavigateMessage msg)
        {
            switch (msg.Mode)
            {
                case NavigationMode.Back:
                    Host.Frame.GoBack();
                    break;
                case NavigationMode.Forward:
                    Host.Frame.GoForward();
                    break;
                default:
                    Host.Frame.Navigate(Conventions.ViewModelToView.GetViewForViewModel(msg.ViewModelType), msg.Parameter);
                    break;
            }
        }
    }
}
