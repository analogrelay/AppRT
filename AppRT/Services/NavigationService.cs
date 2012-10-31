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
        private Frame _rootFrame;

        private ConventionManager Conventions { get; set; }

        public bool CanGoBack { get { return _rootFrame.CanGoBack; } }
        public bool CanGoForward { get { return _rootFrame.CanGoForward; } }

        [ImportingConstructor]
        public NavigationService(ConventionManager conventions)
        {
            Conventions = conventions;
        }

        public void Register(IMessageBus bus)
        {
            // Listen for the message which attaches a frame.
            bus.Listen<ApplicationSitedMessage>()
               .ObserveOn(RxApp.DeferredScheduler)
               .Subscribe(s => _rootFrame = s.RootFrame);

            // Listen for the message which has us navigate.
            bus.Listen<NavigateMessage>()
               .ObserveOn(RxApp.DeferredScheduler)
               .Subscribe(OnNavigateMessage);
        }

        public void GoBack()
        {
            _rootFrame.GoBack();
        }

        public void GoForward()
        {
            _rootFrame.GoForward();
        }

        private void OnNavigateMessage(NavigateMessage msg)
        {
            switch (msg.Mode)
            {
                case NavigationMode.Back:
                    _rootFrame.GoBack();
                    break;
                case NavigationMode.Forward:
                    _rootFrame.GoForward();
                    break;
                default:
                    _rootFrame.Navigate(Conventions.ViewModelToView.GetViewForViewModel(msg.ViewModelType), msg.Parameter);
                    break;
            }
        }
    }
}
