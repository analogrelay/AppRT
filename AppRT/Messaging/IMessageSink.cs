using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppRT.Messaging
{
    public interface IMessageSink
    {
        void Register(IMessageBus bus);
    }
}
