using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.ApplicationSettings;

namespace AppRT
{
    public abstract class GlobalSettingsCommandSource
    {
        public abstract IEnumerable<SettingsCommand> GetGlobalCommands();
    }
}
