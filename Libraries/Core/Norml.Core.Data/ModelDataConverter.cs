using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Norml.Core.Data.Attributes;
using Norml.Core.Data.Mappings;
using Norml.Core.Extensions;

namespace Norml.Core.Data
{
    public class ModelDataConverter : IModelDataConverter
    {
        private DataTable _dataTable;
        private IDictionary<string, string> _columnMappings;
        private IObjectMapperFactory _objectMappingFactory;
        private readonly IConfiguration _configuration;

        public ModelDataConverter(IObjectMapperFactory objectMappingFactory, IConfiguration configuration)
        {
            _objectMappingFactory = objectMappingFactory.ThrowIfNull(nameof(objectMappingFactory));
            _configuration = configuration.ThrowIfNull(nameof(configuration));
        }

        public IDatatableObjectMapping ConvertToDataTable<TModel>(IEnumerable<TModel> models)
        {
            Guard.ThrowIfNullOrEmpty("models", models);

            BuildDataTable(models);

            return new DatatableObjectMapping(_dataTable, _columnMappings);
        }

        private void BuildDataTable<TModel>(IEnumerable<TModel> models)
        {
            var mappingKind = (MappingKind)Enum.Parse(typeof(MappingKind), _configuration["Norml:MappingKind"]);
            var objectMapping = _objectMappingFactory.GetMapper(mappingKind);
            var typeMapping = objectMapping?.GetMappingFor<TModel>();


            _dataTable = typeMapping != null && typeMapping.DataSource.IsNotNullOrEmpty()
                ? BuildTableBasedOnMetadata(models, typeMapping)
                : BuildTableBasedOnModels(models);
        }

        private DataTable BuildTableBasedOnModels<TModel>(IEnumerable<TModel> models)
        {
            var table = new DataTable();

            BuildColumnsBasedOnPropertiesInternal<TModel>(table);

            foreach (var model in models)
            {
                var values = model
                    .ToDictionary()
                    .Values
                    .ToArray();

                table.Rows.Add(values);
            }

            return table;
        }

        private DataTable BuildTableBasedOnMetadata<TModel>(IEnumerable<TModel> models, TypeMapping mapping)
        {
            var table = new DataTable
            {
                TableName = mapping.DataSource
            };

            BuildColumnsBasedOnTypeMappingInternal(table, mapping);

            foreach (var model in models)
            {
                var values = model
                    .ToDictionary(new[] { typeof(FieldMetadataAttribute) })
                    .Values
                    .ToArray();

                table.Rows.Add(values);
            }

            return table;
        }

        private void BuildColumnsBasedOnPropertiesInternal<TModel>(DataTable table)
        {
            _columnMappings = new Dictionary<string, string>();

            foreach (var property in typeof(TModel).GetProperties())
            {
                Type type;

                if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    type = property.PropertyType.GetGenericArguments()[0];
                }
                else
                {
                    type = property.PropertyType;
                }

                table.Columns.Add(new DataColumn(property.Name, type));

                _columnMappings.Add(property.Name, property.Name);
            }
        }

        private void BuildColumnsBasedOnTypeMappingInternal(DataTable table, TypeMapping typeMapping)
        {
            _columnMappings = new Dictionary<string, string>();
            var columns = typeMapping.PropertyMappings
                .Select(propertyMapping => new Tuple<string, int, bool, Type>(propertyMapping.Field, propertyMapping.Order, propertyMapping.AllowDbNull, propertyMapping.MappedType))
                .ToList();

            foreach (var column in columns.OrderBy(c => c.Item2))
            {
                table.Columns.Add(new DataColumn(column.Item1, column.Item4)
                {
                    AllowDBNull = column.Item3
                });

                _columnMappings.Add(column.Item1, column.Item1);
            }
        }
    }
}
