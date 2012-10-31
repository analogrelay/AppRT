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

namespace AppRT.Xaml
{
    internal static class ViewHelpers
    {
        public static void InitializeView<T>(T viewControl) where T : Control, IView
        {
            var conventions = Application.GetService<ConventionManager>();

            viewControl.SetBinding(Control.DataContextProperty, new Binding()
            {
                Path = new PropertyPath("ViewModel"),
                Source = viewControl
            });

            Application.SatisfyImports(viewControl);
            viewControl.ViewModelType = conventions.ViewToViewModel.GetViewModelForView(viewControl.GetType());
            if (viewControl.ViewModelType != null)
            {
                viewControl.ViewModel = conventions.ViewModelBuilder.ConstructViewModel(viewControl.ViewModelType);
            }

            if (!DesignMode.DesignModeEnabled)
            {
                SettingsPane.GetForCurrentView().CommandsRequested += OnCommandsRequested;
            }
        }

        static void OnCommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            var sources = Application.GetServices<GlobalSettingsCommandSource>();
            foreach (var command in sources.SelectMany(src => src.GetGlobalCommands()))
            {
                args.Request.ApplicationCommands.Add(command);
            }
        }
    }
}
