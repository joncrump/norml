using Norml.Core.Data.QueryBuilders.Strategies;

namespace Norml.Core.Data.QueryBuilders
{
    public class QueryBuilderStrategyFactory : IQueryBuilderStrategyFactory
    {
        private readonly IResolver _resolver;

        public QueryBuilderStrategyFactory(IResolver resolver)
        {
            _resolver = Guard.ThrowIfNull("resolver", resolver);
        }

        public IQueryBuilderStrategy GetBuilderStrategy(QueryKind queryKind)
        {
            return _resolver.Resolve<IQueryBuilderStrategy>(queryKind.ToString());
        }
    }
}
