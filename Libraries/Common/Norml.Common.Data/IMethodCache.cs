using System;
using System.Reflection;

namespace Norml.Common.Data
{
    public interface IMethodCache
    {
        Type Type { get; set; }
        PropertyInfo PropertyInfo { get; set; }
        Func<string, object, object> GetPropertyValue { get; } 
    }
}
