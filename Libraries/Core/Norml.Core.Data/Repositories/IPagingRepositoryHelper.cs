using System.Linq.Expressions;

namespace Norml.Core.Data.Repositories
{
    public interface IPagingRepositoryHelper
    {
        PagingModel GetCriteriaByPage(Expression filterExpression = null, PagingInfo pagingInfo = null,
            bool includeParameters = true);
    }
}
