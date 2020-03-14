using System;
using System.Linq.Expressions;

namespace Norml.Providers.Common
{
    public interface IWriteProvider<TInterface>
    {
        TInterface Save(TInterface model, bool isNew= true, Action<TInterface> insertAction = null,
            Action<TInterface> updateAction = null,
            Expression updateExpression = null);
        void Delete(TInterface model);
    }
}
