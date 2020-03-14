using System;
using System.Collections.Generic;

namespace Norml.Common.Data.Mappings
{
    public class TypeMapping
    {
        public TypeMapping()
        {
            PropertyMappings = new List<PropertyMapping>();
        }

        public Type Type { get; set; }
        public string DataSource { get; set; }
        public string CountField { get; set; }
        public string CountAlias { get; set; }
        public IList<PropertyMapping> PropertyMappings { get; set; } 
    }
}
