using System.Linq.Expressions;

namespace Norml.Core.Providers
{
    public interface IPagingProvider
    {
        PagingModel GetPageByCriteria(Expression filterExpression = null, PagingInfo pagingInfo = null);
    }
}
