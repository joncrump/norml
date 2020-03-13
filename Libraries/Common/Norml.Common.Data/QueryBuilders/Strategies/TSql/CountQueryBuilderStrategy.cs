using Norml.Common.Data.Helpers;
using Norml.Common.Data.Mappings;

namespace Norml.Common.Data.QueryBuilders.Strategies.TSql
{
    public class CountQueryBuilderStrategy : QueryBuilderStrategyBase, IQueryBuilderStrategy
    {
        private readonly IObjectMapperFactory _objectMappingFactory;
        private readonly IDatabaseConfiguration _databaseConfiguration;

        public CountQueryBuilderStrategy(IFieldHelper fieldHelper, IObjectMapperFactory objectMappingFactory, 
            IDatabaseConfiguration databaseConfiguration) : base(fieldHelper)
        {
            _objectMappingFactory = objectMappingFactory.ThrowIfNull(nameof(objectMappingFactory));
            _databaseConfiguration = databaseConfiguration.ThrowIfNull(nameof(databaseConfiguration));
        }

        public QueryInfo BuildQuery<TValue>(dynamic parameters) where TValue : class
        {
            var mapper = _objectMappingFactory.GetMapper(_databaseConfiguration.MappingKind);
            var mapping = mapper.GetMappingFor<TValue>();
            var table = mapping.DataSource;
            var count = mapping.CountField;

            return new QueryInfo($"SELECT COUNT({count}) AS {count} FROM {table};");
        }
    }
}
