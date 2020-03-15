using System;
using System.Collections.Generic;
using System.Data;
using NUnit.Framework;
using Norml.Common.Data.Attributes;
using Norml.Common.Data.Helpers;
using Norml.Common.Data.Mappings;
using Norml.Tests.Common.Base;

namespace Norml.Common.Data.Tests.Mappings.ReflectionBasedDataMapperTests
{
    [TestFixture]
    public class TheGetMappingForTypeMethod : MockTestBase<ReflectionBasedDataMapper>
    {
        [Test]
        public void WillThrowArgumentNullExceptionIfObjectTypeIsNull()
        {
            Asserter
                .AssertException<ArgumentNullException>(() => SystemUnderTest.GetMappingForType(null))
                .AndVerifyHasParameter("objectType");
        }

        [Test]
        public void WillReturnMappingBasedOnType()
        {
            var helper = Mocks.Get<IDataBuilderHelper>();

            helper
                .Setup(x => x.InferDatabaseType(typeof(string)))
                .Returns(SqlDbType.NVarChar);

            helper
                .Setup(x => x.GetParameterName("NotDecorated"))
                .Returns("@notDecorated");

            var expected = GetExpected();
            var actual = SystemUnderTest.GetMappingForType(typeof(TestClass));

            Asserter.AssertEquality(expected, actual, new[] {"PropertyMappings"});
            Asserter.AssertEquality(expected.PropertyMappings, actual.PropertyMappings);
        }

        private TypeMapping GetExpected()
        {
            return new TypeMapping
            {
                DataSource = "dbo.FakeTable",
                Type = typeof(TestClass),
                CountField = "Id",
                CountAlias = "NumRows",
                PropertyMappings = GetPropertyMappings()
            };
        }

        private List<PropertyMapping> GetPropertyMappings()
        {
            var propertyMappings = new List<PropertyMapping>
            {
                GetPropertyMapping("Id", SqlDbType.Int, "Id", "@id", true, 
                    false, 0, null, true),
                GetPropertyMapping("Name", SqlDbType.NVarChar, fieldName:"Name", "@name", false, false, 1, isPrimitive:false),
                GetPropertyMapping("ForeignKey", SqlDbType.Int, "ForeignKey", "@foreignKey", order: 2, joinMapping:
                    new JoinMapping
                    {
                        JoinType = JoinType.Inner,
                        LeftKey = "ForeignKey",
                        RightKey = "Id",
                        ParentProperty = "ForeignKey",
                        ChildProperty = "Id"
                    }),
                GetPropertyMapping("NotDecorated", SqlDbType.NVarChar, "NotDecorated", "@notDecorated", false, true, 3, isPrimitive:false )
            };

            return propertyMappings;
        }

        private PropertyMapping GetPropertyMapping(string propertyName, SqlDbType dbType, 
            string fieldName, string parameterName = null,
            bool isIdentity = false, bool allowDbNull = false, int order = 0, Type mappedType = null,
            bool isPrimaryKey = false, IJoinMapping joinMapping = null, string sortColumn = null, 
            string sortColumnAlias = null, bool isPrimitive = true)
        {
            return new PropertyMapping
            {
                Field = fieldName,
                SortColumn = sortColumn,
                SortColumnAlias = sortColumnAlias,
                PropertyName = propertyName,
                DatabaseType = dbType,
                ParameterName = parameterName,
                IsIdentity = isIdentity,
                AllowDbNull = allowDbNull,
                Order = order,
                MappedType = mappedType,
                IsPrimaryKey = isPrimaryKey,
                JoinMapping = joinMapping, 
                IsPrimitive = isPrimitive
            };
        }

        [Table("dbo.FakeTable")]
        public class TestClass
        {
            [CountMetadata("Id", "NumRows")]
            [FieldMetadata("Id", SqlDbType.Int, "@id", true, false, 0, null, true)]
            public int Id { get; set; }

            [FieldMetadata("Name", SqlDbType.NVarChar, "@name", false, false, 1)]
            public string Name { get; set; }

            [FieldMetadata("ForeignKey", SqlDbType.Int, "@foreignKey", false, false, 2, 
                null, false)]
            [Join(JoinType.Inner, typeof(int), "ForeignKey", "Id")]
            public int ForeignKey { get; set; }

            public string NotDecorated { get; set; }

            [Ignore]
            public string PleaseIgnoreMe { get; set; }
        }
    }
}
