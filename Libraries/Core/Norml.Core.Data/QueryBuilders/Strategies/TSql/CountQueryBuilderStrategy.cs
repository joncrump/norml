using System;
using Microsoft.Extensions.Configuration;
using Norml.Core.Data.Mappings;
using Norml.Core.Extensions;

namespace Norml.Core.Data.QueryBuilders.Strategies.TSql
{
    public class CountQueryBuilderStrategy : QueryBuilderStrategyBase, IQueryBuilderStrategy
    {
        private readonly IObjectMapperFactory _objectMappingFactory;
        private readonly IConfiguration _configuration;

        public CountQueryBuilderStrategy(IFieldHelper fieldHelper, IObjectMapperFactory objectMappingFactory, 
            IConfiguration configuration) : base(fieldHelper)
        {
            _objectMappingFactory = objectMappingFactory.ThrowIfNull(nameof(objectMappingFactory));
            _configuration = configuration.ThrowIfNull(nameof(configuration));
        }

        public QueryInfo BuildQuery<TValue>(dynamic parameters) where TValue : class
        {
            var mappingKind = Enum.Parse<MappingKind>(_configuration[Constants.Configuration.MappingKind]);
            var mapper = _objectMappingFactory.GetMapper(mappingKind);
            var mapping = mapper.GetMappingFor<TValue>();
            var table = mapping.DataSource;

            if (table.IsNullOrEmpty())
            {
                throw new InvalidOperationException($"Could not build query. Type {typeof(TValue)} does not have a data source mapping.");
            }

            var count = mapping.CountField;
            var countAlias = mapping.CountAlias;

            if (count.IsNullOrEmpty())
            {
                throw new InvalidOperationException($"Could not build query. Type {typeof(TValue)} does not have a count mapping.");
            }

            return countAlias.IsNullOrEmpty() 
                ? new QueryInfo($"SELECT COUNT({count}) FROM {table};") 
                : new QueryInfo($"SELECT COUNT({count}) AS {countAlias} FROM {table};");
        }
    }
}
