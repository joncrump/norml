using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Norml.Common.Data.Constants;
using Norml.Common.Extensions;

namespace Norml.Common.Data
{
    public class ValueFactory : IValueFactory
    {
        protected Dictionary<string, LambdaExpression> Delegates;

        public ValueFactory()
        {
            Delegates = new Dictionary<string, LambdaExpression>();
        }

        public void AddValueFactory(string key, Expression<Action> valueFactory)
        {
            Guard.ThrowIfNullOrEmpty("key", key);
            Guard.ThrowIfNull("valueFactory", valueFactory);

            if (Delegates.ContainsKey(key))
            {
                throw new InvalidOperationException(ErrorMessages.CannotAddExpression);
            }

            Delegates.Add(key, valueFactory);
        }

        public void AddValueFactory(string key, Expression<Func<object, object>> valueFactory)
        {
            Guard.ThrowIfNullOrEmpty("key", key);
            Guard.ThrowIfNull("valueFactory", valueFactory);

            if (Delegates.ContainsKey(key))
            {
                throw new InvalidOperationException(ErrorMessages.CannotAddExpression);
            }

            Delegates.Add(key, valueFactory);
        }

        public void DeleteValueFactory(string key)
        {
            Guard.ThrowIfNullOrEmpty("key", key);

            if (!Delegates.ContainsKey(key))
            {
                throw new InvalidOperationException(ErrorMessages.CannotDeleteExpression);
            }

            Delegates.Remove(key);
        }

        public Expression<Func<object>> GetValueFactory(string key, ParameterInfo parameter = null)
        {
            Guard.ThrowIfNullOrEmpty("key", key);

            if (!Delegates.ContainsKey(key))
            {
                return null;
            }

            var expression = Delegates[key];
            var parameterExpression = expression.Parameters
                .SafeSelect(p => p)
                .FirstOrDefault();

            if (parameter.IsNull() || parameterExpression.IsNull())
            {
                return Expression.Lambda<Func<object>>(expression.Body, null);
            }

            var blockExpression = Expression.Block(
                new[] { parameterExpression },
// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable once PossibleNullReferenceException
                Expression.Assign(parameterExpression, Expression.Constant(parameter.Value)),
                //Expression.Constant(parameter.Value, parameter.Type)),
// ReSharper restore AssignNullToNotNullAttribute
                expression.Body);

            return Expression.Lambda<Func<object>>(blockExpression, null);
        }

        public Expression GetValueFactory(string key)
        {
            Guard.ThrowIfNullOrEmpty("key", key);

            if (!Delegates.ContainsKey(key))
            {
                return null;
            }

            var expression = Delegates[key];

            if (expression.IsNull())
            {
                return null;
            }

            return Expression.Lambda<Action>(expression, null);
        }
    }
}
