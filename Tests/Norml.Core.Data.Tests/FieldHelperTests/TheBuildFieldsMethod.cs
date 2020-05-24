﻿using System;
using System.Collections.Generic;
using Norml.Core.Extensions;
using Norml.Core.Tests.Common.Base;
using NUnit.Framework;

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
            TableObjectMapping expected = null;

//            expected = new TableObjectMapping
//            {
//                TableName = "dbo.TestTable", 
//                FieldMappings = new Dictionary<string, FieldParameterMapping>
//                    {
//                        {"Id", new FieldParameterMapping("TestFixtureId", "@id", SqlDbType.Int, null, true)},
//                        {"Foo", new FieldParameterMapping("SomeFoo", "@fooParameter", SqlDbType.NVarChar)},
//                        {"Bar", new FieldParameterMapping("PioneerSquareBar", "@itsFridayLetsGoToTheBar", SqlDbType.NVarChar)}
//                    }
//            };
    
            throw new NotImplementedException();
//            var actual = SystemUnderTest.BuildFields<TestFixture>();
//            Expression<Action<KeyValuePair<string, FieldParameterMapping>, KeyValuePair<string, FieldParameterMapping>>> expression = 
//                (e, a) => CompareFieldParameterInfos(e, a, f => Assert.IsNull(f, null));
//
//            Asserter.AssertEquality(expected, actual, new[] {"FieldMappings","Joins", "JoinType"});
//
//            Asserter.AssertEquality(expected.FieldMappings, actual.FieldMappings, additionalParameters:
//                new Dictionary<string, object>
//                {
//                    {Norml.Tests.Common.Constants.ParameterNames.ComparisonDelegate, expression}
//                });
        }

        [Test]
        public void WillReturnFieldsWithInstance()
        {
            TableObjectMapping expected = null;
            TestFixture model = null;

            throw new NotImplementedException();
//            model = ObjectCreator.CreateNew<TestFixture>();
//
//            expected = new TableObjectMapping
//            {
//                TableName = "dbo.TestTable", 
//                FieldMappings = new Dictionary<string, FieldParameterMapping>
//                    {
//                        {"Id", new FieldParameterMapping("TestFixtureId", "@id", SqlDbType.Int, model.Id, true)},
//                        {"Foo", new FieldParameterMapping("SomeFoo", "@fooParameter", SqlDbType.NVarChar, model.Foo)},
//                        {"Bar", new FieldParameterMapping("PioneerSquareBar", "@itsFridayLetsGoToTheBar", SqlDbType.NVarChar, model.Bar)}
//                    }
//            };
//
//            var actual = SystemUnderTest.BuildFields(model:model);
//
//            Asserter.AssertEquality(expected, actual, new[] {"FieldMappings", "Joins", "JoinType"});
//            Expression<Action<KeyValuePair<string, FieldParameterMapping>, KeyValuePair<string, FieldParameterMapping>>> expression =
//                (e, a) => CompareFieldParameterInfos(e, a, f => Assert.IsNotNull(f, null));
//
//            Asserter.AssertEquality(expected.FieldMappings, actual.FieldMappings, additionalParameters:
//                new Dictionary<string, object>
//                {
//                    {Norml.Tests.Common.Constants.ParameterNames.ComparisonDelegate, expression}
//                });
        }

        [Test]
        public void WillReturnSelectedFieldsWithInstance()
        {
            TableObjectMapping expected = null;
            TestFixture model = null;

            throw new NotImplementedException();
//            model = ObjectCreator.CreateNew<TestFixture>();
//
//            expected = new TableObjectMapping
//            {
//                TableName = "dbo.TestTable", 
//                FieldMappings = new Dictionary<string, FieldParameterMapping>
//                    {
//                        {"Id", new FieldParameterMapping("TestFixtureId", "@id", SqlDbType.Int, model.Id, true)},
//                        {"Bar", new FieldParameterMapping("PioneerSquareBar", "@itsFridayLetsGoToTheBar", SqlDbType.NVarChar, model.Bar)}
//                    }
//            };
//
//            var actual = SystemUnderTest.BuildFields(new[] {"Id", "Bar"}, model: model);
//            Expression<Action<KeyValuePair<string, FieldParameterMapping>, KeyValuePair<string, FieldParameterMapping>>> expression =
//                (e, a) => CompareFieldParameterInfos(e, a, f => Assert.IsNotNull(f, null));
//
//            Asserter.AssertEquality(expected, actual, new[] {"FieldMappings", "Joins", "JoinType"});
//
//            Asserter.AssertEquality(expected.FieldMappings, actual.FieldMappings, additionalParameters:
//                new Dictionary<string, object>
//                {
//                    {Norml.Tests.Common.Constants.ParameterNames.ComparisonDelegate, expression}
//                });
        }

        [Test]
        public void WillSetTableName()
        {
            string tableName = null;

            tableName = DataGenerator.GenerateString();

            var actual = SystemUnderTest.BuildFields<TestFixture>(tableName: tableName);

            Asserter.AssertEquality(tableName, actual.TableName);
        }

        [Test]
        public void WillSetAlias()
        {
            string alias = null;

            alias = DataGenerator.GenerateString();

            var actual = SystemUnderTest.BuildFields<TestFixture>(alias: alias);

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
    }
}