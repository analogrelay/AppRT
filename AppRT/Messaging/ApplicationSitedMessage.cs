using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.UI.Xaml.Controls;

namespace AppRT.Messaging
{
    public class ApplicationSitedMessage
    {
        public Frame RootFrame { get; private set; }

        public ApplicationSitedMessage(Frame rootFrame)
        {
            RootFrame = rootFrame;
        }
    }
}
