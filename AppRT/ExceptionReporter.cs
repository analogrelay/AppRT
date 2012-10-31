using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace AppRT
{
    public static class ExceptionReporter
    {
        public static void Report(Exception ex)
        {
            ReportLocal(ex);
        }

        [Conditional("DEBUG")]
        private static async void ReportLocal(Exception ex)
        {
            // GET TO ZE CHOPPA^B^B^B UI THREAD!
            await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.High, async () =>
            {
                var dialog = new MessageDialog(ex.ToString(), "Exception!");
                await dialog.ShowAsync();
            });
        }
    }
}
