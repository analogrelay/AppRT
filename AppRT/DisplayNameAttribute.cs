using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRT
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event)]
    public class DisplayNameAttribute : Attribute
    {
        public string Name { get; private set; }

        public DisplayNameAttribute(string name)
        {
            Name = name;
        }
    }
}
