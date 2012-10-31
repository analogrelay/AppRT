using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AppRT.Xaml
{
    public static class StateManager
    {
        public static readonly DependencyProperty VisualStateProperty = DependencyProperty.RegisterAttached(
            "VisualState", typeof(string), typeof(StateManager), new PropertyMetadata(null, OnVisualStateChanged));

        public static string GetVisualState(Control self)
        {
            return (string)self.GetValue(VisualStateProperty);
        }

        public static void SetVisualState(Control self, string state)
        {
            self.SetValue(VisualStateProperty, state);
        }

        private static void OnVisualStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            string newState = (string)e.NewValue;
            Control c = d as Control;
            if (!String.IsNullOrEmpty(newState) && c != null)
            {
                VisualStateManager.GoToState(c, newState, useTransitions: true);
            }
        }
    }
}
