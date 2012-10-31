using System;
using System.Collections.Generic;
using System.Composition.Hosting;
using System.Linq;
using System.Text;

namespace AppRT
{
    public abstract class ApplicationPlugin
    {
        public virtual void ConfigureContainer(ContainerConfiguration config)
        {
        }
    }
}
