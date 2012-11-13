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
using AppRT.Conventions;
using AppRT.Xaml;
using Callisto.Controls;
using System.Reactive;
using Windows.UI.Xaml.Controls.Primitives;
using AppRT.ViewModels;

namespace AppRT.Services
{
    [Shared]
    [Export]
    public class UIService
    {
        public HostService Host { get; private set; }
        public ObjectFactory Factory { get; private set; }

        [ImportingConstructor]
        public UIService(ObjectFactory factory, HostService host)
        {
            Factory = factory;
            Host = host;
        }

        public virtual Task<T> ShowPopupAsync<T>(PlacementMode placement) where T : IDialogViewModel
        {
            // Construct the view
            TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();
            UIElement viewControl = Factory.CreateViewForViewModel(typeof(T));
            IView view = viewControl as IView;
            if (view == null)
            {
                throw new InvalidOperationException("Popup view must implement IView");
            }
            
            // Construct the flyout
            Flyout fly = new Flyout()
            {
                Content = viewControl,
                Placement = placement,
                PlacementTarget = Host.Frame
            };

            // Set up for closing
            if (Host.PopupHost != null)
            {
                fly.Closed += (_, __) =>
                {
                    Host.PopupHost.Children.Remove(fly.HostPopup);
                    tcs.TrySetResult((T)view.ViewModel);
                };
                Host.PopupHost.Children.Add(fly.HostPopup);
            } else {
                fly.Closed += (_, __) => tcs.TrySetResult((T)view.ViewModel);
            }

            fly.IsOpen = true;
            return tcs.Task;
        }

        public virtual Task ShowMessageAsync(string message)
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
