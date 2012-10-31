using AppRT.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WinRtBehaviors;

namespace AppRT.Xaml
{
    public class ItemClickCommandBehavior : CommandBehavior<ListViewBase>
    {
        private bool _oldItemClickEnabled;

        protected override void OnAttached()
        {
            base.OnAttached();
            
            _oldItemClickEnabled = AssociatedObject.IsItemClickEnabled;
            AssociatedObject.IsItemClickEnabled = true;
            AssociatedObject.ItemClick += OnItemClick;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.IsItemClickEnabled = _oldItemClickEnabled;
            AssociatedObject.ItemClick -= OnItemClick;

            base.OnDetaching();
        }

        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            // Check if the item is invocable
            ICommand command = null;
            IInvocableViewModel invocable = e.ClickedItem as IInvocableViewModel;
            if (invocable != null)
            {
                command = invocable.InvokedCommand;
            }
            command = command ?? Command;
            if (command != null && command.CanExecute(e.ClickedItem))
            {
                command.Execute(e.ClickedItem);
            }
        }
    }
}
