using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace AppRT.Xaml
{
    public class SwapView : Panel
    {
        public static readonly DependencyProperty ViewNameProperty = DependencyProperty.RegisterAttached(
            "ViewName", typeof(string), typeof(SwapView), new PropertyMetadata(String.Empty, new PropertyChangedCallback(OnViewNameChanged)));
        public static readonly DependencyProperty ActiveViewNameProperty = DependencyProperty.Register(
            "ActiveViewName", typeof(string), typeof(SwapView), new PropertyMetadata(String.Empty, new PropertyChangedCallback(OnActiveViewNameChanged)));

        public static string GetViewName(UIElement self)
        {
            return (string)self.GetValue(ViewNameProperty);
        }

        public static void SetViewName(UIElement self, string name)
        {
            self.SetValue(ViewNameProperty, name);
        }

        public string ActiveViewName
        {
            get { return (string)GetValue(ActiveViewNameProperty); }
            set { SetValue(ActiveViewNameProperty, value); }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            return availableSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            UIElement active = Children.FirstOrDefault(child =>
                String.Equals(GetViewName(child),
                              ActiveViewName,
                              StringComparison.OrdinalIgnoreCase));
            if (active != null)
            {
                active.Arrange(new Rect(new Point(0.0, 0.0), finalSize));
            }
            return finalSize;
        }

        private static void OnViewNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DependencyObject current = d;
            SwapView view = null;
            while (current != null && (view = current as SwapView) == null)
            {
                current = VisualTreeHelper.GetParent(current);
            }
            if (view != null)
            {
                view.InvalidateArrange();
            }
        }

        private static void OnActiveViewNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SwapView view = d as SwapView;
            if (view != null)
            {
                view.InvalidateArrange();
            }
        }
    }
}
