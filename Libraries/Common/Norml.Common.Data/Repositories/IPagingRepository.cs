using System.Linq.Expressions;

namespace Norml.Common.Data.Repositories
{
    public interface IPagingRepository
    {
        PagingModel GetCriteriaByPage(Expression filterExpression = null, PagingInfo pagingInfo = null,
            bool includeParameters = true);
    }
}
