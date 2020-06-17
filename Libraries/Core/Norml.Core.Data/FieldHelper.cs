using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Norml.Core.Data.Mappings;
using Norml.Core.Extensions;

namespace Norml.Core.Data
{
    public class FieldHelper : IFieldHelper
    {
        private readonly IObjectMapperFactory _objectMappingFactory;

        public FieldHelper(IObjectMapperFactory objectMappingFactory)
        {
            _objectMappingFactory = objectMappingFactory.ThrowIfNull(nameof(objectMappingFactory));
        }

        public TableObjectMapping BuildFields<TValue>(IEnumerable<string> desiredFields = null, 
            string tableName = null, TValue model = default(TValue), bool ignoreIdentity = false, 
            string alias = null, string instancePropertyName = null, MappingKind mappingKind = MappingKind.Attribute) 
            where TValue : class
        {
            var mapping = _objectMappingFactory.GetMapper(mappingKind);
            var typeMapping = mapping.GetMappingFor<TValue>();

            if (tableName.IsNullOrEmpty())
            {
                tableName = typeMapping.DataSource;
            }

            var fields = typeMapping.PropertyMappings
                .Where(p => !ignoreIdentity || !p.IsIdentity)
                .Where(p => !desiredFields.IsNotNullOrEmpty() || desiredFields.Contains(p.PropertyName))
                .ToList();
            
            var fieldsDictionary = fields.ToDictionary(p => p.PropertyName, p => new FieldParameterMapping(p.Field, p.ParameterName, p.DatabaseType,
                EqualityComparer<TValue>.Default.Equals(model, default)
                    ? null
                    : p.GetPropertyValue(model), p.IsIdentity, alias));

            if (fieldsDictionary.IsNullOrEmpty())
            {
                throw new InvalidOperationException("Cannot build query.  Model has no mappings.");
            }

            return new TableObjectMapping
            {
                TableName = tableName,
                FieldMappings = fieldsDictionary,
                Alias = alias,
                InstancePropertyName = instancePropertyName
            };
        }

        public IEnumerable<IDbDataParameter> ExtractParameters(TableObjectMapping tableFieldInfo, bool ignoreIdentity)
        {
            if (tableFieldInfo.IsNull() || tableFieldInfo.FieldMappings.IsNullOrEmpty())
            {
                return Enumerable.Empty<IDbDataParameter>();
            }

            return tableFieldInfo
                .FieldMappings
                .Select(f => f.Value)
                .Where(f => !ignoreIdentity || !f.IsIdentity)
                .SafeSelect(f => new SqlParameter(f.ParameterName, f.DbType)
                {
                    Value = f.Value.IsNull() ? DBNull.Value : f.Value
                })
                .ToList();
        }
    }
}
