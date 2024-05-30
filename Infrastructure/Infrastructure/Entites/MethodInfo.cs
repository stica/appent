using System;
using System.Collections.Generic;
using System.Text;

namespace Start.Infrastructure.Entites
{
    public class MethodInfo
    {
        public string Name { get; set; }
        public object[] Arguments { get; set; }

        public static MethodInfo Create(string name, params object[] arguments)
        {
            return new MethodInfo
            {
                Name = name,
                Arguments = arguments
            };
        }
    }
}
