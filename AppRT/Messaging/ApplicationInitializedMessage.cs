using System;
using System.Collections.Generic;
using System.Composition.Hosting;
using System.Linq;
using System.Text;
using Windows.UI.Xaml.Controls;

namespace AppRT.Messaging
{
    public class ApplicationInitializedMessage
    {
        public Frame RootFrame { get; private set; }
        public CompositionHost Container { get; private set; }

        public ApplicationInitializedMessage(Frame rootFrame, CompositionHost container)
        {
            RootFrame = rootFrame;
            Container = container;
        }
    }
}
