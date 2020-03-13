using System;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Norml.Common.Data.Mappings
{
    public class PropertyMapping
    {
        public PropertyMapping()
        {

        }

        [JsonConstructor()]
        public PropertyMapping(JoinMapping joinMapping)
        {
            JoinMapping = joinMapping;
        }

        public string PropertyName { get; set; }
        
        [JsonConverter(typeof(StringEnumConverter))]
        public SqlDbType DatabaseType { get; set; }

        public string ParameterName { get; set; }
        public bool IsIdentity { get; set; }
        public bool AllowDbNull { get; set; }
        public int Order { get; set; }
        public bool IsPrimaryKey { get; set; }
        public IJoinMapping JoinMapping { get; set; }
        public string SortColumn { get; set; }
        public string SortColumnAlias { get; set; }
        public string Field { get; set; }
        public bool IsPrimitive { get; set; }
        public string LazyLoader { get; set; }
        public Type MappedType { get; set; }
        public IMethodCache MethodCache { get; set; }
    }
}