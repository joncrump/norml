using System;
using System.Data;
using System.Linq;
using System.Reflection;

namespace TightlyCurly.Com.Repositories.Builders
{
    //public class ReflectionBasedDataReaderBuilder : IBuilder<IDataReader> 
    //{
    //    private readonly ILoaderDelegateFactory _delegateFactory;
    //    private readonly IEnumParser _enumParser;

    //    public ReflectionBasedDataReaderBuilder(ILoaderDelegateFactory delegateFactory, 
    //        IEnumParser enumParser)
    //    {
    //        _delegateFactory = Guard.EnsureIsNotNull("delegateFactory", delegateFactory);
    //        _enumParser = Guard.EnsureIsNotNull("enumParser", enumParser);
    //    }

    //    public TItem Build<TItem>(IDataReader dataSource) where TItem : class, new()
    //    {
    //        Guard.EnsureIsNotNull("dataSource", dataSource);

    //        var item = new TItem();

    //        foreach (var property in item.GetType().GetProperties())
    //        {
    //            var attributes = property.GetCustomAttributes(false);

    //            if (attributes.IsNullOrEmpty())
    //            {
    //                continue;
    //            }

    //            HandleFieldMapping(ref item, property, attributes.FirstOrDefault(t => t is FieldMetadataAttribute) as FieldMetadataAttribute,
    //                dataSource);
    //            HandleEnumMapping(ref item, property, attributes.FirstOrDefault(t => t is EnumAttribute) as EnumAttribute,
    //                dataSource);
    //            HandleLoaderDelegateKey(ref item, property, attributes.FirstOrDefault(t => t is LoaderDelegateKeyAttribute) as LoaderDelegateKeyAttribute,
    //                dataSource);
    //        }

    //        return item;
    //    }

    //    private void HandleFieldMapping<TItem>(ref TItem item, PropertyInfo property, FieldMetadataAttribute attribute,
    //        IDataReader dataSource)
    //    {
    //        if (attribute.IsNull())
    //        {
    //            return;
    //        }

    //        property.SetValue(item, dataSource.Get(property.PropertyType, attribute.FieldName));
    //    }

    //    private void HandleEnumMapping<TItem>(ref TItem item, PropertyInfo property, EnumAttribute attribute,
    //        IDataReader dataSource)
    //    {
    //        if (attribute.IsNull())
    //        {
    //            return;
    //        }

    //        var value = _enumParser.Parse(property.PropertyType, dataSource.Get(attribute.BaseType, attribute.FieldMapping).ToString());

    //        property.SetValue(item, Convert.ChangeType(value, property.PropertyType));
    //    }

    //    private void HandleLoaderDelegateKey<TItem>(ref TItem item, PropertyInfo property, LoaderDelegateKeyAttribute attribute, 
    //        IDataReader dataSource)
    //    {
    //        throw new NotImplementedException();
    //        //if (attribute.IsNull())
    //        //{
    //        //    return;
    //        //}

    //        //var parameters = GetParameters(item, dataSource, property);

    //        //var loadable = Activator.CreateInstance(typeof(Loadable<>).MakeGenericType(property.PropertyType.GenericTypeArguments[0]), 
    //        //    new object[] {parameters, _delegateFactory.GetLoaderDelegate(attribute.Key)});

    //        //property.SetValue(item, loadable);
    //    }

    //    private object[] GetParameters(object item, IDataReader dataSource, PropertyInfo property)
    //    {
    //        throw new NotImplementedException();

    //        //var fieldParameters = 
    //        //    property
    //        //        .GetCustomAttributes()
    //        //        .SafeWhere(t => t is FieldParameterAttribute)
    //        //        .Select(t => new { ((FieldParameterAttribute)t).FieldName, ((FieldParameterAttribute)t).Type })
    //        //        .ToList();

    //        //var propertyParameters =
    //        //    property
    //        //        .GetCustomAttributes()
    //        //        .SafeWhere(t => t is PropertyParameterAttribute)
    //        //        .Select(t => ((PropertyParameterAttribute)t).PropertyName)
    //        //        .ToList();

    //        //var parameters = new List<object>();

    //        //if (!fieldParameters.IsNullOrEmpty())
    //        //{
    //        //    parameters.AddRange(fieldParameters.Select(f => dataSource.Get(f.Type, f.FieldName)));
    //        //}
    //        //else if (!propertyParameters.IsNullOrEmpty())
    //        //{
    //        //    parameters.AddRange(
    //        //        propertyParameters.Select(
    //        //            propertyParameter =>
    //        //            item.GetType().GetProperties().FirstOrDefault(p => p.Name == propertyParameter))
    //        //                          .Where(propertyInfo => !propertyInfo.IsNull())
    //        //                          .Select(propertyInfo => propertyInfo.GetValue(item)));
    //        //}

    //        //return parameters.ToArray();
    //    }
    //}
}
