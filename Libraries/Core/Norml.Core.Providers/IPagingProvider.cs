using System.Linq.Expressions;
using Norml.Common;

namespace Norml.Providers.Common
{
    public interface IPagingProvider
    {
        PagingModel GetPageByCriteria(Expression filterExpression = null, PagingInfo pagingInfo = null);
    }
}
