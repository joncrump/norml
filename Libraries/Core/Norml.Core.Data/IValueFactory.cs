using System;
using System.Linq.Expressions;

namespace Norml.Core.Data
{
    public interface IValueFactory
    {
        void AddValueFactory(string key, Expression<Func<object, object>> valueFactory);
        void DeleteValueFactory(string key);
        Expression<Func<object>> GetValueFactory(string key, ParameterInfo parameter = null);
        void AddValueFactory(string key, Expression<Action> valueFactory);
    }
}
