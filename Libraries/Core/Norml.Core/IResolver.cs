using System;

namespace Norml.Core
{
    public interface IResolver
    {
        TValue Resolve<TValue>();
        object Resolve(Type type);
        TValue Resolve<TValue>(string key);
    }
}
