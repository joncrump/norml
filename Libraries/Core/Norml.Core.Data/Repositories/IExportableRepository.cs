using System;
using System.Linq.Expressions;

namespace Norml.Core.Data.Repositories
{
    public interface IExportableRepository<out TInterface>
    {
        DatatableObjectMapping ToDataTable(Expression filterExpression = null, PagingInfo pagingInfo = null);

        void TransformData(Action<TInterface> transformAction, Expression filterExpression = null,
            PagingInfo pagingInfo = null, bool includeParameters = true);
    }
}
