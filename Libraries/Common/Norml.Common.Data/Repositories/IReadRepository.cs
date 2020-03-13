using System.Collections.Generic;
using System.Linq.Expressions;

namespace Norml.Common.Data.Repositories
{
    public interface IReadRepository<out TInterface>
    {
        IEnumerable<TInterface> Get(Expression filterExpression = null, ILoadOptions loadOptions = null);
    }
}
