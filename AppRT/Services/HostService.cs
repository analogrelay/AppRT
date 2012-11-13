using System;
using System.Reactive.Linq;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppRT.Messaging;
using ReactiveUI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.Foundation;

namespace AppRT.Services
{
    [Shared]
    [Export]
    [Export(typeof(IMessageSink))]
    public class HostService : IMessageSink
    {
        public virtual Rect Bounds { get { return Window.Bounds; } }
        public virtual Frame Frame { get; private set; }
        public virtual Panel PopupHost { get; private set; }

        protected virtual Window Window { get { return Window.Current; } }

        public void Register(IMessageBus bus)
        {
            // Listen for the message which attaches a frame.
            bus.Listen<ApplicationSitedMessage>()
               .ObserveOn(RxApp.DeferredScheduler)
               .Subscribe(s => Frame = s.RootFrame);

            // Listen for the message which attaches a popup host.
            bus.Listen<PopupHostReadyMessage>()
               .ObserveOn(RxApp.DeferredScheduler)
               .Subscribe(m => PopupHost = m.PopupHost);
        }
    }
}
