using System;

namespace Norml.Providers.Common
{
    public interface IReadProvider<out TInterface, TIdType>
    {
        TInterface GetById(TIdType id, Func<TIdType, bool> parameterValidationFunction = null);
    }
}
