using Norml.Core.Data.QueryBuilders.Strategies;

namespace Norml.Core.Data.QueryBuilders
{
    public interface IQueryBuilderStrategyFactory
    {
        IQueryBuilderStrategy GetBuilderStrategy(QueryKind queryKind);
    }
}
