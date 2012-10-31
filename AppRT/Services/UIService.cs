using AppRT.Messaging;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace AppRT.Services
{
    [Shared]
    [Export]
    public class UIService
    {
        public Task ShowMessageAsync(string message)
        {
            return ShowMessageAsyncCore(message, secondLevel: false);
        }

        private async Task ShowMessageAsyncCore(string message, bool secondLevel)
        {
            if (!Window.Current.Dispatcher.HasThreadAccess)
            {
                if (secondLevel)
                {
                    throw new InvalidOperationException("Tried to move to UI thread, but failed. Call UIService.ShowMessageAsync on the UI Thread");
                }

                // Rerun on the UI Thread
                await Window.Current.Dispatcher.RunAsync(
                    CoreDispatcherPriority.Normal,
                    async () => await ShowMessageAsyncCore(message, secondLevel: true));
            }
            else
            {
                var dialog = new MessageDialog(message);
                dialog.Commands.Add(new UICommand("OK"));
                await dialog.ShowAsync();
            }
        }
    }
}
