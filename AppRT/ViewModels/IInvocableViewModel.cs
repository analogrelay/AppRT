using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppRT.ViewModels
{
    public interface IInvocableViewModel
    {
        ICommand InvokedCommand { get; }
    }
}
