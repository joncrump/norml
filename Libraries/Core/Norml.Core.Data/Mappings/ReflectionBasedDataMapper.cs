using System;
using System.Reflection;
using Norml.Common.Data.Attributes;
using Norml.Common.Data.Helpers;

namespace Norml.Common.Data.Mappings
{
    public class ReflectionBasedDataMapper : IDataMapper
    {
        private readonly IDataBuilderHelper _dataBuilderHelper;

        public ReflectionBasedDataMapper(IDataBuilderHelper dataBuilderHelper)
        {
            _dataBuilderHelper = dataBuilderHelper.ThrowIfNull(nameof(dataBuilderHelper));
        }

        public TypeMapping GetMappingFor<TValue>()
        {
            return GetMappingForType(typeof(TValue));
        }

        public TypeMapping GetMappingForType(Type objectType)
        {
            objectType.ThrowIfNull(nameof(objectType));

            return BuildMapping(objectType);
        }

        private TypeMapping BuildMapping(Type objectType)
        {
            var typeMapping = new TypeMapping();

            BuildTableMapping(objectType, typeMapping);
            BuildPropertyMappings(objectType, typeMapping);

            return typeMapping;
        }

        private void BuildPropertyMappings(Type objectType, TypeMapping typeMapping)
        {
            var propertyInfos = objectType.GetProperties();
            var order = 0;

            foreach (var propertyInfo in propertyInfos)
            {
                BuildPropertyMapping(propertyInfo, typeMapping, order);
                order++;
            }
        }

        private void BuildPropertyMapping(PropertyInfo propertyInfo, TypeMapping typeMapping, int order)
        {
            var fieldAttribute = propertyInfo.GetCustomAttribute<FieldMetadataAttribute>();
            var joinAttribute = propertyInfo.GetCustomAttribute<JoinAttribute>();
            var countAttribute = propertyInfo.GetCustomAttribute<CountMetadataAttribute>();
            PropertyMapping propertyMapping;

            if (fieldAttribute == null)
            {
                propertyMapping = BuildMappingFromProperty(propertyInfo, order);
            }
            else
            {
                propertyMapping = new PropertyMapping();

                BuildFromFieldAttribute(propertyInfo, propertyMapping, fieldAttribute);
                BuildFromCountAttribute(typeMapping, countAttribute);
                BuildFromJoinAttribute(propertyMapping, joinAttribute);
            }

            AddPropertyMappingToTypeMapping(typeMapping, propertyMapping);
        }

        private PropertyMapping BuildMappingFromProperty(PropertyInfo propertyInfo, int order)
        {
            if (propertyInfo.GetCustomAttribute<IgnoreAttribute>() != null)
            {
                return null;
            }

            var propertyMapping = new PropertyMapping
            {
                PropertyName = propertyInfo.Name,
                DatabaseType = _dataBuilderHelper.InferDatabaseType(propertyInfo.PropertyType),
                ParameterName = _dataBuilderHelper.GetParameterName(propertyInfo.Name),
                IsIdentity = false,
                AllowDbNull = true,
                IsPrimaryKey = false,
                Order = order,
                IsPrimitive = propertyInfo.PropertyType.IsPrimitive,
                Field = propertyInfo.Name
            };

            return propertyMapping;
        }

        private void AddPropertyMappingToTypeMapping(TypeMapping typeMapping, PropertyMapping propertyMapping)
        {
            if (propertyMapping != null)
            {
                typeMapping.PropertyMappings.Add(propertyMapping);
            }
        }

        private void BuildFromJoinAttribute(PropertyMapping propertyMapping, JoinAttribute joinAttribute)
        {
            if (joinAttribute == null)
            {
                return;
            }

            var joinMapping = new JoinMapping
            {
                JoinType = joinAttribute.JoinType,
                LeftKey = joinAttribute.LeftKey,
                RightKey = joinAttribute.RightKey,
                JoinTable = joinAttribute.JoinTable,
                JoinTableLeftKey = joinAttribute.JoinTableLeftKey,
                JoinTableRightKey = joinAttribute.JoinTableRightKey,
                JoinTableJoinType = joinAttribute.JoinTableJoinType,
                ParentProperty = joinAttribute.ParentProperty,
                ChildProperty = joinAttribute.ChildProperty
            };

            propertyMapping.JoinMapping = joinMapping;
        }

        private void BuildFromCountAttribute(TypeMapping typeMapping, CountMetadataAttribute countAttribute)
        {
            if (countAttribute == null)
            {
                return;
            }

            typeMapping.CountField = countAttribute.FieldName;
            typeMapping.CountAlias = countAttribute.FieldAlias;
        }

        private void BuildFromFieldAttribute(PropertyInfo propertyInfo, PropertyMapping propertyMapping,
            FieldMetadataAttribute fieldAttribute)
        {
            propertyMapping.PropertyName = propertyInfo.Name;
            propertyMapping.DatabaseType = fieldAttribute.DbType;
            propertyMapping.ParameterName = fieldAttribute.ParameterName;
            propertyMapping.IsIdentity = fieldAttribute.IsIdentity;
            propertyMapping.AllowDbNull = fieldAttribute.AllowDbNull;
            propertyMapping.IsPrimaryKey = fieldAttribute.IsPrimaryKey;
            propertyMapping.Order = fieldAttribute.Order ?? 0;
            propertyMapping.IsPrimitive = propertyInfo.PropertyType.IsPrimitive;
            propertyMapping.Field = fieldAttribute.FieldName;
        }

        private void BuildTableMapping(Type objectType, TypeMapping mapping)
        {
            var tableAttribute = (TableAttribute)objectType.GetCustomAttribute(typeof(TableAttribute));

            mapping.DataSource = tableAttribute.Name;
            mapping.Type = objectType;
        }
    }
}