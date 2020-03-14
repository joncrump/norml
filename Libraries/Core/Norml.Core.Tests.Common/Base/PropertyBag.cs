using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Norml.Common;

namespace Norml.Tests.Common.Base
{
    public class PropertyBag
    {
        private readonly IDictionary<string, Tuple<Type, object>> _values;

        public PropertyBag()
        {
            _values = new Dictionary<string, Tuple<Type, object>>();
        }

        public void Add<TValue>(string key, Mock<TValue> mock) where TValue : class
        {
            Guard.ThrowIfNullOrEmpty("key", key);

            _values.Add(key, new Tuple<Type, object>(mock.GetType().GenericTypeArguments.First(), mock));
        }

        public Mock<TValue> Get<TValue>() where TValue : class
        {
            var value = _values.Values
                .FirstOrDefault(v => v.Item1 == typeof(TValue));

            return value?.Item2 as Mock<TValue>;
        }

        public TValue Get<TValue>(string key)
        {
            Guard.ThrowIfNullOrEmpty("key", key);

            var value = _values[key];

            return (TValue)value.Item2;
        }
        
        public bool HasValue(string key)
        {
            Guard.ThrowIfNullOrEmpty("key", key);

            return _values.ContainsKey(key);
        }

        public IEnumerable<object> Values()
        {
            return _values.Values;
        }
    }
}
