using System;
using System.Linq.Expressions;

namespace Norml.Core.Providers
{
    public interface IExportableProvider<out TInterface>
    {
        DatatableObjectMapping ToDataTable(Expression filterExpression = null, PagingInfo pagingInfo = null);
        void TransformData(Action<TInterface> transformAction, Expression filterExpression = null,
            PagingInfo pagingInfo = null, bool includeParameters = true);
    }
}
