using System;
using System.Collections.Generic;
using Norml.Common;
using Norml.Common.Extensions;

namespace Norml.Tests.Common
{
    public class ColumnInfo
    {
        public ColumnInfo() 
        {
            Values = new List<object>();
        }

        public ColumnInfo(string columnName)
        {
            ColumnName = Guard.ThrowIfNullOrEmpty("columnName", columnName);
        }

        public ColumnInfo(string columnName, object value) : this(columnName)
        {
            Values = new List<object> {value};
        }

        public ColumnInfo(string columName, IEnumerable<object> values) : this(columName)
        {
            Values = values.ToSafeList();
        }

        public string ColumnName { get; set; }
        public IEnumerable<object> Values { get; set; } 
    }
}
