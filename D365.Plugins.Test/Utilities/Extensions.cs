using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace D365.Plugins.Test.Utilities
{
    public static class Extensions
    {
        public static void Add(this List<KeyValuePair<string, Guid>> value, String name, Guid id)
        {
            value.Add(new KeyValuePair<string, Guid>(name, id));
        }
    }
}
