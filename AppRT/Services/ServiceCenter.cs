using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRT.Services
{
    [Shared]
    public class ServiceCenter
    {
        [Export]
        public IMessageBus Bus { get; private set; }

        public ServiceCenter()
        {
            Bus = new MessageBus();
        }
    }
}
