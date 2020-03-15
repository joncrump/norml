using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using Moq;
using Norml.Common.Data.QueryBuilders.Strategies.TSql;
using NUnit.Framework;
using Norml.Tests.Common.Base;

namespace Norml.Common.Data.Tests.QueryBuilders.Strategies.TSql.InsertQueryBuilderStrategyTests
{
    [TestFixture]
    public class TheBuildQueryMethod : MockTestBase<InsertQueryBuilderStrategy>
    {
        [Test]
        public void WillThrowArgumentNullExceptionIfParametersIsNull()
        {
            Asserter
                .AssertException<ArgumentNullException>(
                    () => SystemUnderTest.BuildQuery<TestClass>());
        }

        [Test]
        public void WillBuildInsertQuery()
        {
            QueryInfo expected = null;
            TestClass testClass = null;
            IEnumerable<IDbDataParameter> expectedParameters = null;

            testClass = ObjectCreator.CreateNew<TestClass>();
            var expectedQuery = "INSERT dbo.TestTable ([TestClassId], [SomeFoo], [PioneerSquareBar]) VALUES (@id, @fooParameter, @itsFridayLetsGoToTheBar); SELECT SCOPE_IDENTITY() AS Id;";
            expectedParameters = new List<IDbDataParameter>
            {
                new SqlParameter("@id", SqlDbType.Int) {Value = testClass.Id},
                new SqlParameter("@fooParameter", SqlDbType.NVarChar) {Value = testClass.Foo},
                new SqlParameter("@itsFridayLetsGoToTheBar", SqlDbType.NVarChar) {Value = testClass.Bar}
            };
            expected = new QueryInfo(expectedQuery, Mock.Of<TableObjectMapping>(), expectedParameters);

            Mocks.Get<IFieldHelper>()
                .Setup(x => x.BuildFields(It.IsAny<IEnumerable<string>>(), It.IsAny<string>(), testClass, It.IsAny<bool>(), 
                    It.IsAny<string>(), It.IsAny<string>()))
                .Returns(() => new TableObjectMapping
                {
                    TableName = "dbo.TestTable",
                    FieldMappings = new Dictionary<string, FieldParameterMapping>
                        {
                            {"Id", new FieldParameterMapping("TestClassId", "@id", SqlDbType.Int, testClass.Id, true)},
                            {"Foo", new FieldParameterMapping("SomeFoo", "@fooParameter", SqlDbType.NVarChar, testClass.Foo)},
                            {"Bar", new FieldParameterMapping("PioneerSquareBar", "@itsFridayLetsGoToTheBar", SqlDbType.NVarChar, 
                                testClass.Bar)}
                        }
                });
                    
            Mocks.Get<IFieldHelper>()
                .Setup(x => x.ExtractParameters(It.IsAny<TableObjectMapping>(),
                    It.IsAny<bool>()))
                .Returns(expectedParameters);

            dynamic parameters = new ExpandoObject();

            parameters.ReturnNewId = true;
            parameters.IgnoreIdentity = true;
            parameters.DesiredFields = null;
            parameters.TableName = null;
            parameters.Model = testClass;

            QueryInfo actual = SystemUnderTest.BuildQuery<TestClass>(parameters);
            var expression = ConstructComparisonDelegate();

            Asserter.AssertEquality(expected, actual, new[] { "Parameters", "tableObjectMappings" });

            Asserter.AssertEquality(
                expectedParameters
                    .Select(p =>
                    {
                        var s = (SqlParameter)p;
                        return (object)new { s.ParameterName, s.Value, s.SqlDbType };
                    }),
                actual.Parameters
                    .Select(p =>
                    {
                        var s = (SqlParameter)p;
                        return (object)new { s.ParameterName, s.Value, s.SqlDbType };
                    }),
                additionalParameters:
                    new Dictionary<string, object>
                    {
                        {
                            Norml.Tests.Common.Constants.ParameterNames.ComparisonDelegate, 
                            expression
                        }
                    });
        }

        private void VerifyParameters(object expected, object actual)
        {
            Asserter.AssertEquality(expected, actual,
                new[]
                {
                    "SqlDbType", "Value"
                });

            var type = expected.GetType();
            var property = type.GetProperty("SqlDbType");
            Asserter.AssertEquality((SqlDbType)property.GetValue(expected), (SqlDbType)property.GetValue(actual));
        }

        private Expression<Action<object, object>> ConstructComparisonDelegate()
        {
            Expression<Action<object, object>> actionExpression =
                (e, a) => VerifyParameters(e, a);

            return actionExpression;
        }
    }
}
