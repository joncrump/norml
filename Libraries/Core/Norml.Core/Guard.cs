using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Norml.Common.Exceptions;
using Norml.Common.Extensions;

namespace Norml.Common
{
    public static class Guard
    {
        public static object ThrowIfNull(string parameterName, object value, string message = null)
        {
            ValidateParameterName(parameterName);

            if (!value.IsNull())
            {
                return value;
            }

            if (message.IsNull())
            {
                throw new ArgumentNullException(parameterName);
            }

            throw new ArgumentNullException(parameterName, message);
        }

        public static T ThrowIfNull<T>(string parameterName, T value, string message = null)
        {
            ValidateParameterName(parameterName);

            if (!value.IsNull())
            {
                return value;
            }

            if (message.IsNull())
            {
                throw new ArgumentNullException(parameterName);
            }

            throw new ArgumentNullException(parameterName, message);
        }

        public static string ThrowIfNullOrEmpty(string parameterName, string value, string message = null)
        {
            ValidateParameterName(parameterName);

            if (value.IsNull())
            {
                if (message.IsNull())
                {
                    throw new ArgumentNullException(parameterName);
                }

                throw new ArgumentNullException(parameterName, message);
            }

            if (value.Length == 0)
            {
                if (message.IsNull())
                {
                    throw new ArgumentEmptyException(parameterName);
                }

                throw new ArgumentEmptyException(message, parameterName);
            }

            return value;
        }

        public static IEnumerable<T> ThrowIfNullOrEmpty<T>(string parameterName, IEnumerable<T> value, string message = null)
        {
            ValidateParameterName(parameterName);

            if (value.IsNull())
            {
                if (message.IsNull())
                {
                    throw new ArgumentNullException(parameterName);
                }

                throw new ArgumentNullException(parameterName, message);
            }

            var argumentIsNotNullOrEmpty = value as T[] ?? value.ToArray();
            if (!argumentIsNotNullOrEmpty.Any())
            {
                if (message.IsNull())
                {
                    throw new ArgumentEmptyException(parameterName);
                }

                throw new ArgumentEmptyException(message, parameterName);
            }

            return argumentIsNotNullOrEmpty;
        }

        public static object ThrowIfNull(this object value, string parameterName, string message = null)
        {
            ValidateParameterName(parameterName);

            if (!value.IsNull())
            {
                return value;
            }

            if (message.IsNull())
            {
                throw new ArgumentNullException(parameterName);
            }

            throw new ArgumentNullException(parameterName, message);
        }

        public static T ThrowIfNull<T>(this T value, string parameterName, string message = null)
        {
            ValidateParameterName(parameterName);

            if (!value.IsNull())
            {
                return value;
            }

            if (message.IsNull())
            {
                throw new ArgumentNullException(parameterName);
            }
         
            throw new ArgumentNullException(parameterName, message);
        }

        public static string EnsureStringIsNotNullOrEmpty(this string value, string parameterName, string message = null)
        {
            ValidateParameterName(parameterName);

            if (value.IsNull())
            {
                if (message.IsNull())
                {
                    throw new ArgumentNullException(parameterName);
                }

                throw new ArgumentNullException(parameterName, message);
            }

            if (value.Length == 0)
            {
                if (message.IsNull())
                {
                    throw new ArgumentEmptyException(parameterName);
                }

                throw new ArgumentEmptyException(message, parameterName);
            }

            return value;
        }

        public static IEnumerable<T> EnsureIsNotNullOrEmpty<T>(this IEnumerable<T> value, string parameterName, string message = null)
        {
            ValidateParameterName(parameterName);

            if (value.IsNull())
            {
                if (message.IsNull())
                {
                    throw new ArgumentNullException(parameterName);
                }

                throw new ArgumentNullException(parameterName, message);
            }

            var argumentIsNotNullOrEmpty = value as T[] ?? value.ToArray();
            if (!argumentIsNotNullOrEmpty.Any())
            {
                if (message.IsNull())
                {
                    throw new ArgumentEmptyException(parameterName);
                }

                throw new ArgumentEmptyException(message, parameterName);
            }

            return argumentIsNotNullOrEmpty;
        }

        public static T EnsureIsValid<T>(string parameterName, Func<T, bool> checkPredicate, T value, string valueMessage = null)
        {
            ValidateParameterName(parameterName);

            if (!checkPredicate(value))
            {
                throw new ArgumentInvalidException(valueMessage ?? "Parameter is invalid", parameterName);
            }

            return value;
        }

        private static void ValidateParameterName(string parameterName)
        {
            if (parameterName.IsNullOrEmpty())
            {
                throw new InvalidOperationException("parameterName must be supplied.");
            }
        }
    }
}
