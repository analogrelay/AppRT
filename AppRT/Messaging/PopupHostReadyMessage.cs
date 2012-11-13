using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.UI.Xaml.Controls;

namespace AppRT.Messaging
{
    public class PopupHostReadyMessage
    {
        public Panel PopupHost { get; private set; }

        public PopupHostReadyMessage(Panel popupHost)
        {
            PopupHost = popupHost;
        }
    }
}
