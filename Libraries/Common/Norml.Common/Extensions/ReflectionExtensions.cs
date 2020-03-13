using System;
using System.Reflection;

namespace Norml.Common.Extensions
{
    public static class ReflectionExtensions
    {
        public static MethodInfo LocateMethod(this object value, string name)
        {
            if (value.IsNull() || name.IsNullOrEmpty())
            {
                return null;
            }

            var method = value.GetType().GetMethod(name);

            return method;
        }

        public static MethodInfo Locate(this object value, string name, Type[] types)
        {
            if (value.IsNull() || name.IsNullOrEmpty())
            {
                return null;
            }

            var method = value.GetType().GetMethod(name, types);

            return method;
        }

        //public static TAttribute GetAttribute<TAttribute>(this PropertyInfo propertyInfo)
        //    where TAttribute : Attribute
        //{
        //    return (TAttribute)propertyInfo.GetCustomAttribute(typeof(
        //        TAttribute));
        //}
    }
}
