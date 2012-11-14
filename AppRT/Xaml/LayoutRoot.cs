using System;
using System.Collections.Generic;
using System.Linq;
using AppRT.Messaging;
using ReactiveUI;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace AppRT.Xaml
{
    [ContentProperty(Name = "Content")]
    [TemplatePart(Name = LayoutRoot.Const_PopupHostPartName, Type = typeof(Panel))]
    public sealed class LayoutRoot : ContentControl
    {
        internal const string Const_PopupHostPartName = "PART_PopupHost";
        public static readonly string PopupHostPartName = Const_PopupHostPartName; // Public "constants" should never be 'const' since they get inlined.

        public static readonly DependencyProperty PageTitleProperty = DependencyProperty.Register(
            "PageTitle", typeof(string), typeof(LayoutRoot), new PropertyMetadata(String.Empty));
        public static readonly DependencyProperty ShowProgressAnimationProperty = DependencyProperty.Register(
            "ShowProgressAnimation", typeof(bool), typeof(LayoutRoot), new PropertyMetadata(false));
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            "Header", typeof(object), typeof(LayoutRoot), new PropertyMetadata(null));

        public string PageTitle
        {
            get { return (string)GetValue(PageTitleProperty); }
            set { SetValue(PageTitleProperty, value); }
        }

        public bool ShowProgressAnimation
        {
            get { return (bool)GetValue(ShowProgressAnimationProperty); }
            set { SetValue(ShowProgressAnimationProperty, value); }
        }

        public object Header
        {
            get { return GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public LayoutRoot()
        {
            Header = new Grid();
            this.DefaultStyleKey = typeof(LayoutRoot);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Find popup host part
            Panel p = GetTemplateChild(PopupHostPartName) as Panel;
            if (p != null && !DesignMode.DesignModeEnabled)
            {
                Application.Current
                           .Factory
                           .GetService<IMessageBus>()
                           .SendMessage(new PopupHostReadyMessage(p));
            }
        }
    }
}
