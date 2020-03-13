using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Norml.Common.Data.Constants;
using Norml.Common.Data.Mappings;
using Norml.Common.Extensions;

namespace Norml.Common.Data
{
    public class FieldHelper : IFieldHelper
    {
        private readonly IObjectMapperFactory _objectMappingFactory;

        public FieldHelper(IObjectMapperFactory objectMappingFactory)
        {
            _objectMappingFactory = Guard.ThrowIfNull("objectMappingFactory", objectMappingFactory);
        }

        public TableObjectMapping BuildFields<TValue>(IEnumerable<string> desiredFields = null, 
            string tableName = null, TValue model = default(TValue), bool ignoreIdentity = false, 
            string alias = null, string instancePropertyName = null) 
            where TValue : class
        {
            throw new NotImplementedException();
            //var mapping = _objectMappingFactory.GetMappingFor<TValue>();

            //if (tableName.IsNullOrEmpty())
            //{
            //    tableName = mapping.DataSource;
            //}

            //var fields = mapping.PropertyMappings
            //    .Where(p => !ignoreIdentity || !p.IsIdentity)
            //    .Where(p => (!desiredFields.IsNotNullOrEmpty() || desiredFields.Contains(p.PropertyName)))
            //    .ToDictionary(p => p.PropertyName, p => new FieldParameterMapping(p.Field, p.ParameterName, DatabaseTypes.FieldMappings[p.FieldType.ToLower()], 
            //        EqualityComparer<TValue>.Default.Equals(model, default(TValue)) 
            //            ? null 
            //            : p.MethodCache.GetPropertyValue(p.PropertyName, model), p.IsIdentity, alias));

            //if (fields.IsNullOrEmpty())
            //{
            //    throw new InvalidOperationException("Cannot build query.  Model has no data attributes.");
            //}

            //return new TableObjectMapping
            //{
            //    TableName = tableName,
            //    FieldMappings = fields,
            //    Alias = alias,
            //    InstancePropertyName = instancePropertyName
            //};
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
