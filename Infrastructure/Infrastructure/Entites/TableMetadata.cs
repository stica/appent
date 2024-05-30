using System;
using System.Reflection;

namespace Start.Infrastructure
{
    public class TableMetadata
    {
        public string TableName { get; set; }

        public PropertyInfo Key { get; set; }
    }
}
