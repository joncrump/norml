using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Moq;
using Norml.Core.Data.Mappings;
using Norml.Core.Extensions;
using Norml.Core.Tests.Common.Base;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Norml.Core.Data.Tests.FieldHelperTests
{
    [TestFixture]
    public class TheBuildFieldsMethod : MockTestBase<FieldHelper>
    {
        [Test]
        public void WillThrowInvalidOperationExceptionIfTableNameIsNullAndObjectHasNoTableAttribute()
        {
            // ReSharper disable once RedundantArgumentDefaultValue
            Asserter
                .AssertException<InvalidOperationException>(
                    () => SystemUnderTest.BuildFields<object>(tableName: null))
                .AndVerifyMessageContains(
                    "Cannot build query.  Type {0} has no table attributes".FormatString(typeof (object)));
        }

        [Test]
        public void WillThrowInvalidOperationExceptionIfTableNameIsEmptyAndObjectHasNoTableAttribute()
        {
            Asserter
                .AssertException<InvalidOperationException>(
                    () => SystemUnderTest.BuildFields<object>(tableName: String.Empty))
                .AndVerifyMessageContains(
                    "Cannot build query.  Type {0} has no table attributes".FormatString(typeof (object)));
        }

        [Test]
        public void WillReturnFieldsWithoutInstance()
        {
            var expected = new TableObjectMapping
            {
                TableName = "dbo.TestTable",
                FieldMappings = new Dictionary<string, FieldParameterMapping>
                {
                    {"Id", new FieldParameterMapping("TestFixtureId", "@id", SqlDbType.Int, null, true)},
                    {"Foo", new FieldParameterMapping("SomeFoo", "@fooParameter", SqlDbType.NVarChar)},
                    {"Bar", new FieldParameterMapping("PioneerSquareBar", "@itsFridayLetsGoToTheBar", SqlDbType.NVarChar)}
                }
            };

            var actual = SystemUnderTest.BuildFields<TestClass>();
            Expression<Action<KeyValuePair<string, FieldParameterMapping>, KeyValuePair<string, FieldParameterMapping>>> expression =
                (e, a) => CompareFieldParameterInfos(e, a, f => Assert.IsNull(f, null));

            Asserter.AssertEquality(expected, actual, new[] { "FieldMappings", "Joins", "JoinType" });

            Asserter.AssertEquality(expected.FieldMappings, actual.FieldMappings, additionalParameters:
                new Dictionary<string, object>
                {
                    {Core.Tests.Common.Constants.ParameterNames.ComparisonDelegate, expression}
                });
        }

        [Test]
        public void WillReturnFieldsWithInstance()
        {
            var model = ObjectCreator.CreateNew<TestClass>();
            var mockDataMapper = new Mock<IDataMapper>();

            Mocks.Get<IObjectMapperFactory>()
                .Setup(x => x.GetMapper(It.IsAny<MappingKind>()))
                .Returns(mockDataMapper.Object);

            mockDataMapper
                .Setup(x => x.GetMappingFor<TestClass>())
                .Returns(GetMappingForTestClass(model));

            var expected = new TableObjectMapping
            {
                TableName = "dbo.TestTable",
                FieldMappings = new Dictionary<string, FieldParameterMapping>
                {
                    {"Id", new FieldParameterMapping("TestFixtureId", "@id", SqlDbType.Int, model.Id, true)},
                    {"Foo", new FieldParameterMapping("SomeFoo", "@fooParameter", SqlDbType.NVarChar, model.Foo)},
                    {"Bar", new FieldParameterMapping("PioneerSquareBar", "@itsFridayLetsGoToTheBar", SqlDbType.NVarChar, model.Bar)}
                }
            };

            var actual = SystemUnderTest.BuildFields(model: model);

            Asserter.AssertEquality(expected, actual, new[] { "FieldMappings", "Joins", "JoinType" });
            Expression<Action<KeyValuePair<string, FieldParameterMapping>, KeyValuePair<string, FieldParameterMapping>>> expression =
                (e, a) => CompareFieldParameterInfos(e, a, f => Assert.IsNotNull(f, null));

            Asserter.AssertEquality(expected.FieldMappings, actual.FieldMappings, additionalParameters:
                new Dictionary<string, object>
                {
                    {Core.Tests.Common.Constants.ParameterNames.ComparisonDelegate, expression}
                });
        }

        [Test]
        public void WillReturnSelectedFieldsWithInstance()
        {
            TableObjectMapping expected = null;
            TestClass model = null;

            model = ObjectCreator.CreateNew<TestClass>();

            expected = new TableObjectMapping
            {
                TableName = "dbo.TestTable",
                FieldMappings = new Dictionary<string, FieldParameterMapping>
                    {
                        {"Id", new FieldParameterMapping("TestFixtureId", "@id", SqlDbType.Int, model.Id, true)},
                        {"Bar", new FieldParameterMapping("PioneerSquareBar", "@itsFridayLetsGoToTheBar", SqlDbType.NVarChar, model.Bar)}
                    }
            };

            var actual = SystemUnderTest.BuildFields(new[] { "Id", "Bar" }, model: model);
            Expression<Action<KeyValuePair<string, FieldParameterMapping>, KeyValuePair<string, FieldParameterMapping>>> expression =
                (e, a) => CompareFieldParameterInfos(e, a, f => Assert.IsNotNull(f, null));

            Asserter.AssertEquality(expected, actual, new[] { "FieldMappings", "Joins", "JoinType" });

            Asserter.AssertEquality(expected.FieldMappings, actual.FieldMappings, additionalParameters:
                new Dictionary<string, object>
                {
                    {Core.Tests.Common.Constants.ParameterNames.ComparisonDelegate, expression}
                });
        }

        [Test]
        public void WillSetTableName()
        {
            string tableName = null;

            tableName = DataGenerator.GenerateString();

            var actual = SystemUnderTest.BuildFields<TestClass>(tableName: tableName);

            Asserter.AssertEquality(tableName, actual.TableName);
        }

        [Test]
        public void WillSetAlias()
        {
            string alias = null;

            alias = DataGenerator.GenerateString();

            var actual = SystemUnderTest.BuildFields<TestClass>(alias: alias);

            Asserter.AssertEquality(alias, actual.Alias);
        }
        
        private void CompareFieldParameterInfos(KeyValuePair<string, FieldParameterMapping> expected, 
            KeyValuePair<string, FieldParameterMapping> actual, Action<object> valueAsserter)
        {
            Asserter.AssertEquality(expected.Key, actual.Key);
            Asserter.AssertEquality(expected.Value, actual.Value, new[] {"DbType", "Value"});
            Asserter.AssertEquality(expected.Value.DbType, actual.Value.DbType);

            valueAsserter(actual.Value.Value);
        }

        private TypeMapping GetMappingForTestClass(TestClass model)
        {
            return new TypeMapping
            {
                Type = typeof(TestClass),
                DataSource = "dbo.TestTable",
                PropertyMappings = new List<PropertyMapping>
                {
                    new PropertyMapping
                    {
                        Field = "TestFixtureId",
                        DatabaseType = SqlDbType.Int,
                        AllowDbNull = true,
                        ParameterName = "@id",
                        PropertyName = "Id"
                    },
                    new PropertyMapping
                    {
                        Field = "SomeFoo",
                        DatabaseType = SqlDbType.NVarChar,
                        ParameterName = "@fooParameter",
                        PropertyName = "Foo"
                    },
                    new PropertyMapping
                    {
                        Field = "Bar",
                        DatabaseType = SqlDbType.NVarChar,
                        ParameterName =  "@itsFridayLetsGoToTheBar",
                        PropertyName = "Bar"
                    }
                }
            };
        }
    }
}