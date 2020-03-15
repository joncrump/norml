using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using Moq;
using Norml.Common.Data.QueryBuilders.Strategies.TSql;
using Norml.Common.Extensions;
using NUnit.Framework;
using Norml.Tests.Common.Base;

namespace Norml.Common.Data.Tests.QueryBuilders.Strategies.TSql.DeleteQueryBuilderStrategyTests
{
    [TestFixture]
    public class TheBuildQueryMethod : MockTestBase<DeleteQueryBuilderStrategy>
    {
        protected override void Setup()
        {
            base.Setup();

            Mocks.Get<IFieldHelper>()
                .Setup(x => x.BuildFields(It.IsAny<IEnumerable<string>>(), It.IsAny<string>(),
                    It.IsAny<DummyClass>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new TableObjectMapping
                {
                    TableName = DataGenerator.GenerateString(),
                    FieldMappings = new Dictionary<string, FieldParameterMapping>()
                });
        }

        [Test]
        public void WillThrowArgumentNullExceptionIfParametersAreNull()
        {
            Asserter
                .AssertException<ArgumentNullException>(
                    () => SystemUnderTest.BuildQuery<TestClass>(null))
                .AndVerifyHasParameter("parameters");
        }

        [Test]
        public void WillBuildDeleteQueryWithPredicate()
        {
            QueryInfo expected = null;
            TestClass testClass;
            IEnumerable<IDbDataParameter> expectedParameters = null;

            testClass = ObjectCreator.CreateNew<TestClass>();
            var whereClause = DataGenerator.GenerateString();
            var expectedQuery = "DELETE FROM dbo.TestTable WHERE {0};"
                .FormatString(whereClause);
            expectedParameters = new List<IDbDataParameter>
            {
                new SqlParameter("@id", SqlDbType.Int) {Value = testClass.Id},
            };
            Mocks.Get<IPredicateBuilder>()
                .Setup(x => x.BuildContainer(It.IsAny<Expression<Func<TestClass, bool>>>(), It.IsAny<Type>(), true, It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new QueryContainer(whereClause, expectedParameters));

            expected = new QueryInfo(expectedQuery, It.IsAny<TableObjectMapping>(), expectedParameters);

            Mocks.Get<IFieldHelper>()
                .Setup(x => x.BuildFields(It.IsAny<IEnumerable<string>>(), It.IsAny<string>(), It.IsAny<TestClass>(), It.IsAny<bool>(), 
                    It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new TableObjectMapping
                {
                    TableName = "dbo.TestTable",
                    FieldMappings = new Dictionary<string, FieldParameterMapping>
                        {
                            {"Id", new FieldParameterMapping("Id", "@id", SqlDbType.Int)}
                        }
                });

            Expression<Func<TestClass, bool>> predicate = t => t.Bar == "Joe's Bar";
            dynamic parameters = new ExpandoObject();

            parameters.Predicate = predicate;
            parameters.TableName = null;

            QueryInfo actual = SystemUnderTest.BuildQuery<TestClass>(parameters);
            Expression<Action<SqlParameter, SqlParameter>> expression =
                (e, a) => CompareSqlParameters(e, a);

            Asserter.AssertEquality(expected, actual, new[] { "Parameters", "tableObjectMappings" });
            Asserter.AssertEquality(expectedParameters.Select(p =>
            {
                var s = (SqlParameter)p;
                return new { s.ParameterName, s.Value, s.SqlDbType };
            }),
                actual.Parameters.SafeSelect(p =>
                {
                    var s = (SqlParameter)p;
                    return new { s.ParameterName, s.Value, s.SqlDbType };
                }),
            propertiesToIgnore: new[] {"Value", "SqlDbType"},
            additionalParameters: new Dictionary<string, object>
            {
                {
                    Norml.Tests.Common.Constants.ParameterNames.AssertDelegate,
                    expression
                }
            });
        }

        private void CompareSqlParameters(SqlParameter expected, SqlParameter actual)
        {
            Asserter.AssertEquality(expected, actual, propertiesToIgnore: new[] { "Value", "SqlDbType" });
            Asserter.AssertEquality(expected.SqlDbType, actual.SqlDbType);
        }
    }
}
