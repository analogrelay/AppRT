using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRT.ViewModels
{
    public interface IDialogViewModel
    {
        /// <summary>
        /// Gets the result of the dialog, null indicates the dialog was closed, false => cancelled, true => OK
        /// </summary>
        bool? Result { get; }
    }
}
