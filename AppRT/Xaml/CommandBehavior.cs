using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using WinRtBehaviors;

namespace AppRT.Xaml
{
    public abstract class CommandBehavior<T> : Behavior<T> where T : Windows.UI.Xaml.FrameworkElement
    {
        private static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command", typeof(ICommand), typeof(CommandBehavior<T>), new PropertyMetadata(null, OnCommandChanged));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CommandBehavior<T> self = d as CommandBehavior<T>;
            if (self == null) return;

            if (e.NewValue != null)
            {
                ICommand cmd = (ICommand)e.NewValue;
                cmd.CanExecuteChanged += self.OnCanExecuteChanged;
            }
        }

        protected virtual void OnCanExecuteChanged(object sender, EventArgs e)
        {
        }
    }
}
