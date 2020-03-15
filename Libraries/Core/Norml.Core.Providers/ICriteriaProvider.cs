using System.Collections.Generic;
using System.Linq.Expressions;

namespace Norml.Core.Providers
{
    public interface ICriteriaProvider<out TInterface>
    {
        IEnumerable<TInterface> GetByCriteria(Expression filterExpression = null);
    }
}
