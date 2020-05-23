﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;

namespace Norml.Core.Data.Tests.QueryBuilders.Strategies.TSql.UpdateQueryBuilderStrategyTests
{
    [TestFixture]
    public class TheBuildQueryMethod : MockTestBase<UpdateQueryBuilderStrategy>
    {
        [Test]
        public void WillBuildUpdateQueryWithPredicate()
        {
            QueryInfo expected = null;
            TestFixture TestFixture = null;
            IEnumerable<IDbDataParameter> expectedParameters = null;

            TestFixture = ObjectCreator.CreateNew<TestFixture>();
            var whereClause = DataGenerator.GenerateString();
            var expectedQuery = "UPDATE dbo.TestTable SET [TestFixtureId] = @id, [SomeFoo] = @fooParameter, [PioneerSquareBar] = @itsFridayLetsGoToTheBar WHERE {0};"
                .FormatString(whereClause);
            Mocks.Get<IPredicateBuilder>()
                .Setup(x => x.BuildContainer(It.IsAny<Expression<Func<TestFixture, bool>>>(), It.IsAny<Type>(), true, It.IsAny<string>(), 
                    It.IsAny<string>()))
                .Returns(new QueryContainer(whereClause, expectedParameters));
            expectedParameters = new List<IDbDataParameter>
            {
                new SqlParameter("@id", SqlDbType.Int) {Value = TestFixture.Id},
                new SqlParameter("@fooParameter", SqlDbType.NVarChar) {Value = TestFixture.Foo},
                new SqlParameter("@itsFridayLetsGoToTheBar", SqlDbType.NVarChar) {Value = TestFixture.Bar}
            };
            expected = new QueryInfo(expectedQuery, Mock.Of<TableObjectMapping>(), expectedParameters);

            Mocks.Get<IFieldHelper>()
                .Setup(x => x.BuildFields(It.IsAny<IEnumerable<string>>(), It.IsAny<string>(), TestFixture, It.IsAny<bool>(), 
                    It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new TableObjectMapping
                {
                    TableName = "dbo.TestTable",
                    FieldMappings = new Dictionary<string, FieldParameterMapping>
                        {
                            {"Id", new FieldParameterMapping("TestFixtureId", "@id", SqlDbType.Int, TestFixture.Id)},
                            {"Foo", new FieldParameterMapping("SomeFoo", "@fooParameter", SqlDbType.NVarChar, TestFixture.Foo)},
                            {"Bar", new FieldParameterMapping("PioneerSquareBar", "@itsFridayLetsGoToTheBar", 
                                    SqlDbType.NVarChar, TestFixture.Bar)}
                        }
                });

            Mocks.Get<IFieldHelper>()
                .Setup(x => x.ExtractParameters(It.IsAny<TableObjectMapping>(), It.IsAny<bool>()))
                .Returns(expectedParameters);

            Expression<Func<TestFixture, bool>> predicate = t => t.Bar == "Joe's Bar";
            dynamic parameters = new ExpandoObject();
            
            parameters.Predicate = predicate;
            parameters.Model = TestFixture;
            parameters.CanDirtyRead = true;
            parameters.IncludeParameters = null;
            parameters.TableName = null;
            parameters.DesiredFields = null;

            QueryInfo actual = SystemUnderTest.BuildQuery<TestFixture>(parameters);
            Expression<Action<SqlParameter, SqlParameter>> expression = (e, a) => CompareParameters(e, a);

            Asserter.AssertEquality(expected, actual, new[] { "Parameters", "tableObjectMappings" });
            Asserter.AssertEquality(expectedParameters.Select(p => (SqlParameter)p),
                actual.Parameters.SafeSelect(p => (SqlParameter)p),
                additionalParameters: new Dictionary<string, object>
                {
                    {
                        Norml.Tests.Common.Constants.ParameterNames.ComparisonDelegate, 
                        expression
                    }
                });
        }

        private void CompareParameters(SqlParameter expected, SqlParameter actual)
        {
            Asserter.AssertEquality(expected, actual, new[]
            {
                "Value", "SqlDbType", "DbType", "SqlValue", "SourceVersion",
                "CompareInfo", "Direction"
            });
            Asserter.AssertEquality(expected.SqlDbType, actual.SqlDbType);
        }
    }
}