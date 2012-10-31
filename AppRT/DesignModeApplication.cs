using System;
using System.Reflection;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Hosting;
using System.Linq;
using System.Text;
using AppRT.Xaml;

namespace AppRT
{
    internal class DesignModeApplication
    {
        private CompositionHost _container;

        public DesignModeApplication()
        {
            Compose();
        }

        protected void Compose()
        {
            // Create the container
            var config = new ContainerConfiguration();
            config.WithAssembly(typeof(Application).GetTypeInfo().Assembly);
            _container = config.CreateContainer();

            // Compose the application
            _container.SatisfyImports(this);
        }

        internal void SatisfyImports(object objectWithLooseImports)
        {
            _container.SatisfyImports(objectWithLooseImports);
        }
    }
}
