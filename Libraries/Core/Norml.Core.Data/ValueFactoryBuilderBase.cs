using Norml.Common.Extensions;

namespace Norml.Common.Data
{
    public abstract class ValueFactoryBuilderBase
    {
        protected readonly IValueFactory ValueFactory;

        protected ValueFactoryBuilderBase(IValueFactory valueFactory)
        {
            ValueFactory = Guard.ThrowIfNull("valueFactory", valueFactory);
        }

        protected void AddValueFactory(ValueFactoryModelBase model, 
            string loaderKey, ParameterInfo parameter = null)
        {
            if (model.IsNull())
            {
                return;
            }

            Guard.ThrowIfNullOrEmpty("loaderKey", loaderKey);

            var valueExpression = ValueFactory.GetValueFactory(
                loaderKey, parameter);

            valueExpression.Do(() =>
                model.ValueFactories.Add(loaderKey, valueExpression.Compile()));
        }
    }
}
