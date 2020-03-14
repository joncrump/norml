using System.Collections.Generic;
using System.Linq.Expressions;

namespace Norml.Providers.Common
{
    public interface ICriteriaProvider<out TInterface>
    {
        IEnumerable<TInterface> GetByCriteria(Expression filterExpression = null);
    }
}
