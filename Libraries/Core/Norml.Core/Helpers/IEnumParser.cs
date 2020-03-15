using System;

namespace Norml.Core.Helpers
{
    public interface IEnumParser
    {
        T Parse<T>(string value);
        object Parse(Type type, string value);
    }
}