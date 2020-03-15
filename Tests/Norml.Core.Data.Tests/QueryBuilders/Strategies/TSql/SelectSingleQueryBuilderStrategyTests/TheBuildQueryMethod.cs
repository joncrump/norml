using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using Moq;
using NUnit.Framework;
using Norml.Common.Data.QueryBuilders.Strategies.TSql;
using Norml.Common.Extensions;
using Norml.Tests.Common.Base;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Norml.Common.Data.Tests.QueryBuilders.Strategies.TSql.SelectSingleQueryBuilderStrategyTests
{
    [TestFixture]
    public class TheBuildQueryMethod : MockTestBase<SelectSingleQueryBuilderStrategy>
    {
        [Test]
        public void WillBuildSelectQueryWithoutPredicate()
        {
            QueryInfo expected = null;

            var expectedQuery = "SELECT [TestClassId], [SomeFoo], [PioneerSquareBar] FROM dbo.TestTable (NOLOCK);";
            expected = new QueryInfo(expectedQuery);

            Mocks.Get<IFieldHelper>()
                .Setup(x => x.BuildFields(It.IsAny<IEnumerable<string>>(), It.IsAny<string>(), It.IsAny<TestClass>(),
                    It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new TableObjectMapping
                {
                    TableName  = "dbo.TestTable",
                    FieldMappings = new Dictionary<string, FieldParameterMapping>
                        {
                            {"Id", new FieldParameterMapping("TestClassId", "@id", SqlDbType.Int, DBNull.Value)},
                            {"Foo", new FieldParameterMapping("SomeFoo", "@fooParameter", SqlDbType.NVarChar, DBNull.Value)},
                            {"Bar", new FieldParameterMapping("PioneerSquareBar", "@itsFridayLetsGoToTheBar", SqlDbType.NVarChar, DBNull.Value)}
                        }
                });

            Expression<Func<TestClass, bool>> predicate = null;

            dynamic parameters = new ExpandoObject();
            parameters.CanDirtyRead = true;
            parameters.IncludeParameters = true;
            parameters.DesiredFields = null;
            parameters.TableName = null;
            parameters.Predicate = predicate;

            QueryInfo actual = SystemUnderTest.BuildQuery<TestClass>(parameters);

            Asserter.AssertEquality(expected, actual, new[] { "Parameters", "tableObjectMappings" });
            Assert.IsTrue(actual.Parameters.IsNullOrEmpty());
        }

        [Test]
        public void WillBuildSelectQueryWithPredicate()
        {
            QueryInfo expected = null;
            TestClass testClass = null;
            IEnumerable<IDbDataParameter> expectedParameters = null;

            testClass = ObjectCreator.CreateNew<TestClass>();
            var whereClause = "{0}".FormatString(DataGenerator.GenerateString());
            var expectedQuery = "SELECT [TestClassId], [SomeFoo], [PioneerSquareBar] FROM dbo.TestTable (NOLOCK) WHERE {0};"
                .FormatString(whereClause);
            expectedParameters = new List<IDbDataParameter>
            {
                new SqlParameter("@id", SqlDbType.Int) {Value = testClass.Id},
            };
            Mocks.Get<IPredicateBuilder>()
                .Setup(x => x.BuildContainer(It.IsAny<Expression<Func<TestClass, bool>>>(), It.IsAny<Type>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new QueryContainer(whereClause, expectedParameters));

            expected = new QueryInfo(expectedQuery, It.IsAny<IEnumerable<TableObjectMapping>>(), expectedParameters);

            Mocks.Get<IFieldHelper>()
               .Setup(x => x.BuildFields(It.IsAny<IEnumerable<string>>(), It.IsAny<string>(),
                   It.IsAny<TestClass>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>()))
               .Returns(new TableObjectMapping
               {
                    TableName  = "dbo.TestTable",
                    FieldMappings = new Dictionary<string, FieldParameterMapping>
                        {
                            {"Id", new FieldParameterMapping("TestClassId", "@id", SqlDbType.Int, testClass.Id)},
                            {"Foo", new FieldParameterMapping("SomeFoo", "@fooParameter", SqlDbType.NVarChar, testClass.Foo)},
                            {"Bar", new FieldParameterMapping(
                                "PioneerSquareBar", "@itsFridayLetsGoToTheBar", SqlDbType.NVarChar, testClass.Bar)}
                        }
               });

            Expression<Func<TestClass, bool>> predicate = t => t.Id == testClass.Id;

            dynamic parameters = new ExpandoObject();
            parameters.CanDirtyRead = true;
            parameters.IncludeParameters = true;
            parameters.DesiredFields = null;
            parameters.TableName = null;
            parameters.Predicate = predicate;

            QueryInfo actual = SystemUnderTest.BuildQuery<TestClass>(parameters);
            Expression<Action<SqlParameter, SqlParameter>> expression = (e, a) => CompareParameters(e, a);

            Asserter.AssertEquality(expected, actual, new[] { "Parameters", "tableObjectMappings" });
            Asserter.AssertEquality(
                expectedParameters.Select(p => (SqlParameter)p),
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
