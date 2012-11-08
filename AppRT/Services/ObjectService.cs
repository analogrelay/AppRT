using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRT.Services
{
    [Shared]
    [Export]
    public class ObjectService
    {
        public TBase Construct<TBase>(Type actual) where TBase: class
        {
            return Construct(actual) as TBase;
        }

        public object Construct(Type type)
        {
            // TODO: Cached Compiled Expressions FTW!
            return Activator.CreateInstance(type);
        }
    }
}
