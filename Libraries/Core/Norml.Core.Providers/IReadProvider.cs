using System;

namespace Norml.Core.Providers
{
    public interface IReadProvider<out TInterface, TIdType>
    {
        TInterface GetById(TIdType id, Func<TIdType, bool> parameterValidationFunction = null);
    }
}
