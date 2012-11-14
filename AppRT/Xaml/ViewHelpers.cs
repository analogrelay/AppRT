using AppRT.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using ReactiveUI;
using AppRT.Services;

namespace AppRT.Xaml
{
    internal static class ViewHelpers
    {
        public static void InitializeView<T>(T viewControl) where T : Control, IView
        {
            var factory = Application.Current.Factory;

            viewControl.SetBinding(Control.DataContextProperty, new Binding()
            {
                Path = new PropertyPath("ViewModel"),
                Source = viewControl
            });
            factory.SatisfyImports(viewControl);
            viewControl.ViewModel = factory.CreateViewModelForView(viewControl.GetType());
            viewControl.DataContext = viewControl.ViewModel;
            viewControl.ViewModelType = viewControl.ViewModel == null ? null : viewControl.ViewModel.GetType();
            if (viewControl.ViewModelType != null)
            {
            }

            if (!DesignMode.DesignModeEnabled)
            {
                SettingsPane.GetForCurrentView().CommandsRequested += OnCommandsRequested;
            }
        }

        static void OnCommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            var sources = Application.Current.Factory.GetServices<GlobalSettingsCommandSource>();
            foreach (var command in sources.SelectMany(src => src.GetGlobalCommands()))
            {
                args.Request.ApplicationCommands.Add(command);
            }
        }

        static void OnLoaded<T>(T viewControl) where T : Control, IView
        {
        }
    }
}
