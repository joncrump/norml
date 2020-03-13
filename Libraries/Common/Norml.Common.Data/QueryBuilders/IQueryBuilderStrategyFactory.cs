using Norml.Common.Data.QueryBuilders.Strategies;

namespace Norml.Common.Data.QueryBuilders
{
    public interface IQueryBuilderStrategyFactory
    {
        IQueryBuilderStrategy GetBuilderStrategy(QueryKind queryKind);
    }
}
