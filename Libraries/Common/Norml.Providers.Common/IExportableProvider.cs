using System;
using System.Linq.Expressions;
using Norml.Common;
using Norml.Common.Data;

namespace Norml.Providers.Common
{
    public interface IExportableProvider<out TInterface>
    {
        DatatableObjectMapping ToDataTable(Expression filterExpression = null, PagingInfo pagingInfo = null);
        void TransformData(Action<TInterface> transformAction, Expression filterExpression = null,
            PagingInfo pagingInfo = null, bool includeParameters = true);
    }
}
