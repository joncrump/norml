using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using Moq;
using NUnit.Framework;
using Norml.Common.Data.Mappings;
using Norml.Common.Extensions;
using Norml.Tests.Common.Base;

namespace Norml.Common.Data.Tests.DatabaseQueryPredicateBuilderTests
{
    [TestFixture]
    public class TheBuildContainerMethod : MockTestBase<DatabaseQueryPredicateBuilder>
    {
        protected override void Setup()
        {
            base.Setup();

            var mapper = new Mock<IDataMapper>();

            mapper
                .Setup(x => x.GetMappingForType(typeof(TestClass)))
                .Returns(GetTestClassTypeMapping());

            Mocks.Get<IObjectMapperFactory>()
                .Setup(x => x.GetMapper(It.IsAny<MappingKind>()))
                .Returns(mapper.Object);
        }

        [Test]
        public void WillPassParameterChecks()
        {
            DoMethodTests<DatabaseQueryPredicateBuilder>("BuildContainer");
        }

        [Test]
        public void WillBuildContainer()
        {
            Expression<Func<TestClass, bool>> expression = null;
            QueryContainer expected = null;

            var id = DataGenerator.GenerateInteger();
            var testClass = ObjectCreator.CreateNew<TestClass>();
            expression = t => t.Id == id && t.Bar == testClass.Bar || t.Foo == "Margaritas";

            expected = new QueryContainer(
                "TestClassId = @id AND PioneerSquareBar = @itsFridayLetsGoToTheBar OR SomeFoo = @fooParameter",
                new List<IDbDataParameter>
                {
                    new SqlParameter
                    {
                        ParameterName = "@id",
                        SqlDbType = SqlDbType.Int,
                        Value = id
                    },
                    new SqlParameter
                    {
                        ParameterName = "@itsFridayLetsGoToTheBar",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = testClass.Bar
                    },
                    new SqlParameter
                    {
                        ParameterName = "@fooParameter",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = "Margaritas"
                    }
                });

            var actual = SystemUnderTest.BuildContainer(expression, typeof(TestClass));
            Expression<Action<SqlParameter, SqlParameter>> compareExpression =
                (e, a) => CompareParameters(e, a);

            Asserter.AssertEquality(expected, actual, new[] {"Parameters", "OrderByClause"});

            Asserter.AssertEquality(
                expected.Parameters.Select(p => (SqlParameter)p), 
                actual.Parameters.Select(p => (SqlParameter)p), 
                additionalParameters:new Dictionary<string, object>
                {
                    {
                        Norml.Tests.Common.Constants.ParameterNames.ComparisonDelegate, 
                        compareExpression
                    }
                });
        }

        [Test]
        public void WillBuildContainerWithPrefix()
        {
            Expression<Func<TestClass, bool>> expression = null;
            QueryContainer expected = null;


            var id = DataGenerator.GenerateInteger();
            var testClass = ObjectCreator.CreateNew<TestClass>();
            expression = t => t.Id == id && t.Bar == testClass.Bar || t.Foo == "Margaritas";

            expected = new QueryContainer(
                "t1.t1_TestClassId = @id AND t1.t1_PioneerSquareBar = @itsFridayLetsGoToTheBar OR t1.t1_SomeFoo = @fooParameter",
                new List<IDbDataParameter>
                {
                    new SqlParameter
                    {
                        ParameterName = "@id",
                        SqlDbType = SqlDbType.Int,
                        Value = id
                    },
                    new SqlParameter
                    {
                        ParameterName = "@itsFridayLetsGoToTheBar",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = testClass.Bar
                    },
                    new SqlParameter
                    {
                        ParameterName = "@fooParameter",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = "Margaritas"
                    }
                });

            var actual = SystemUnderTest.BuildContainer(expression, typeof(TestClass), tableAlias:"t1", fieldPrefix:"t1_");
            Expression<Action<SqlParameter, SqlParameter>> compareExpression =
                (e, a) => CompareParameters(e, a);

            Asserter.AssertEquality(expected, actual, new[] { "Parameters", "OrderByClause" });

            Asserter.AssertEquality(
                expected.Parameters.Select(p => (SqlParameter)p),
                actual.Parameters.Select(p => (SqlParameter)p),
                additionalParameters: new Dictionary<string, object>
                {
                    {
                        Norml.Tests.Common.Constants.ParameterNames.ComparisonDelegate, 
                        compareExpression
                    }
                });
        }

        [Test]
        public void WillThrowInvalidOperationExceptionIfPropertyHasNoMetadataAttributes()
        {
            DummyClass parameterClass = null;
            var comparisonClass = ObjectCreator.CreateNew<DummyClass>();

            Expression<Func<DummyClass, bool>> expression = d => d.SomeProperty == comparisonClass.SomeProperty;

            Asserter
                .AssertException<InvalidOperationException>(
                    () => SystemUnderTest.BuildContainer(expression, typeof(TestClass)))
                .AndVerifyMessageContains("Cannot build query.  Property has no metadata attributes: SomeProperty");
        }

        [Test]
        public void WillHandleStartsWithClauses()
        {
            Expression<Func<TestClass, bool>> expression = null;
            QueryContainer expected = null;

                    expression = t => t.Bar.StartsWith("Hink");

                    expected = new QueryContainer(
                        "PioneerSquareBar LIKE ('Hink%')");

                    var actual = SystemUnderTest.BuildContainer(expression, typeof(TestClass));

                    Asserter.AssertEquality(expected, actual, new[] { "Parameters", "OrderByClause" });
        }

        [Test]
        public void WillHandleStartsWithClausesWithPrefix()
        {
            Expression<Func<TestClass, bool>> expression = null;
            QueryContainer expected = null;

            expression = t => t.Bar.StartsWith("Hink");

            expected = new QueryContainer(
                "t1.t1_PioneerSquareBar LIKE ('Hink%')");

            var actual = SystemUnderTest.BuildContainer(expression, typeof(TestClass), 
                tableAlias:"t1", fieldPrefix:"t1_");

            Asserter.AssertEquality(expected, actual, new[] { "Parameters", "OrderByClause" });
        }

        [Test]
        public void WillHandleContainsClauses()
        {
            Expression<Func<TestClass, bool>> expression = null;
            QueryContainer expected = null;

            expression = t => t.Bar.Contains("yikes");

            expected = new QueryContainer(
                "PioneerSquareBar LIKE ('%yikes%')");

            var actual = SystemUnderTest.BuildContainer(expression, typeof(TestClass));

            Asserter.AssertEquality(expected, actual, new[] { "Parameters", "OrderByClause" });
        }

        [Test]
        public void WillHandleContainsClausesWithPrefix()
        {
            Expression<Func<TestClass, bool>> expression = null;
            QueryContainer expected = null;

            expression = t => t.Bar.Contains("yikes");

            expected = new QueryContainer(
                "t1.t1_PioneerSquareBar LIKE ('%yikes%')");
       
            var actual = SystemUnderTest.BuildContainer(expression, typeof(TestClass), 
                tableAlias:"t1", fieldPrefix:"t1_");

            Asserter.AssertEquality(expected, actual, new[] { "Parameters", "OrderByClause" });
        }

        [Test]
        public void WillHandleEndsWithClauses()
        {
            Expression<Func<TestClass, bool>> expression = null;
            QueryContainer expected = null;

            expression = t => t.Bar.EndsWith("yikes");

            expected = new QueryContainer(
                "PioneerSquareBar LIKE ('%yikes')");
        
            var actual = SystemUnderTest.BuildContainer(expression, typeof(TestClass));

            Asserter.AssertEquality(expected, actual, new[] { "Parameters", "OrderByClause" });
        }

        [Test]
        public void WillHandleEndsWithClausesWithPrefix()
        {
            Expression<Func<TestClass, bool>> expression = null;
            QueryContainer expected = null;

            expression = t => t.Bar.EndsWith("yikes");

            expected = new QueryContainer(
                "t1.t1_PioneerSquareBar LIKE ('%yikes')");
        
            var actual = SystemUnderTest.BuildContainer(expression, typeof(TestClass), 
                tableAlias:"t1", fieldPrefix:"t1_");

            Asserter.AssertEquality(expected, actual, new[] { "Parameters", "OrderByClause" });
        }

        [Test]
        public void WillHandleStringEqualsClauses()
        {
            Expression<Func<TestClass, bool>> expression = null;
            QueryContainer expected = null;

            expression = t => t.Bar.Equals("yikes");

            expected = new QueryContainer(
                "PioneerSquareBar = 'yikes'");
       
            var actual = SystemUnderTest.BuildContainer(expression, typeof(TestClass));

            Asserter.AssertEquality(expected, actual, new[] { "Parameters", "OrderByClause" });
        }

        [Test]
        public void WillHandleStringEqualsClausesWithPrefix()
        {
            Expression<Func<TestClass, bool>> expression = null;
            QueryContainer expected = null;

            expression = t => t.Bar.Equals("yikes");

            expected = new QueryContainer(
                "t1.t1_PioneerSquareBar = 'yikes'");

            var actual = SystemUnderTest.BuildContainer(expression, typeof(TestClass), 
                tableAlias:"t1", fieldPrefix:"t1_");

            Asserter.AssertEquality(expected, actual, new[] { "Parameters", "OrderByClause" });
        }

        [Test]
        public void WillHandleIntEqualsClauses()
        {
            Expression<Func<TestClass, bool>> expression = null;
            QueryContainer expected = null;

            expression = t => t.Id.Equals(5);

            expected = new QueryContainer(
                "TestClassId = 5");

            var actual = SystemUnderTest.BuildContainer(expression, typeof(TestClass));

            Asserter.AssertEquality(expected, actual, new[] { "Parameters", "OrderByClause" });
        }

        [Test]
        public void WillHandleIntEqualsClausesWithPrefix()
        {
            Expression<Func<TestClass, bool>> expression = null;
            QueryContainer expected = null;

            expression = t => t.Id.Equals(5);

            expected = new QueryContainer(
                "t1.t1_TestClassId = 5");
       
            var actual = SystemUnderTest.BuildContainer(expression, typeof(TestClass), 
                tableAlias:"t1", fieldPrefix:"t1_");

            Asserter.AssertEquality(expected, actual, new[] { "Parameters", "OrderByClause" });
        }

        [Test]
        public void WillHandleEqualsNullClause()
        {
            QueryContainer expected = null;
            Expression<Func<TestClass, bool>> expression = null;

            expression = t => t.Bar.Equals(null);

            expected = new QueryContainer("PioneerSquareBar IS NULL");
        
            var actual = SystemUnderTest.BuildContainer(expression, typeof (TestClass));

            Asserter.AssertEquality(expected, actual, new[] { "Parameters", "OrderByClause" });
        }

        [Test]
        public void WillHandleEqualsNullClauseWithPrefix()
        {
            QueryContainer expected = null;
            Expression<Func<TestClass, bool>> expression = null;

            expression = t => t.Bar.Equals(null);

            expected = new QueryContainer("t1.t1_PioneerSquareBar IS NULL");
       
            var actual = SystemUnderTest.BuildContainer(expression, typeof(TestClass), 
                tableAlias:"t1", fieldPrefix:"t1_");

            Asserter.AssertEquality(expected, actual, new[] { "Parameters", "OrderByClause" });
        }

        [Test]
        public void WillHandleDateTime()
        {
            Expression expression = null;
            string expected = null;

            var dateClass = ObjectCreator.CreateNew<DateClass>();

            var parameterExpression = Expression.Parameter(typeof (DateClass), "d");
            var propertyInfo = typeof (DateClass).GetProperty("SomeDateTime");
            var memberExpression = Expression.MakeMemberAccess(parameterExpression, propertyInfo);
            var dateValue = dateClass.SomeDateTime;

            var dayExpression = Build(memberExpression,
                           Expression.Constant(dateValue.Day), "Day", "Equals", typeof(Int32));
            var monthExpression = Build(memberExpression,
                Expression.Constant(dateValue.Month), "Month", "Equals", typeof(Int32));
            var yearExpression = Build(memberExpression,
                Expression.Constant(dateValue.Year), "Year", "Equals", typeof(Int32));

            var andExpression = Expression.AndAlso(dayExpression, monthExpression);

            expression = Expression.AndAlso(andExpression, yearExpression);

            expected = "DAY(SomeDate) = {0} AND MONTH(SomeDate) = {1} AND YEAR(SomeDate) = {2}"
                .FormatString(dateValue.Day.ToString(), dateValue.Month.ToString(), dateValue.Year.ToString());

            var mapper = new Mock<IDataMapper>();

            mapper
                .Setup(x => x.GetMappingForType(typeof(DateClass)))
                .Returns(GetDateMapping);

            Mocks
                .Get<IObjectMapperFactory>()
                .Setup(x => x.GetMapper(It.IsAny<MappingKind>()))
                .Returns(mapper.Object);

            var actual = SystemUnderTest.BuildContainer(expression, typeof(DateClass));

            Asserter.AssertEquality(new QueryContainer(expected), actual, new[] { "Parameters", "OrderByClause" });
        }

        [Test]
        public void WillHandleDateTimeWithPrefix()
        {
            Expression expression = null;
            string expected = null;

            var dateClass = ObjectCreator.CreateNew<DateClass>();

            var parameterExpression = Expression.Parameter(typeof(DateClass), "d");
            var propertyInfo = typeof(DateClass).GetProperty("SomeDateTime");
            var memberExpression = Expression.MakeMemberAccess(parameterExpression, propertyInfo);
            var dateValue = dateClass.SomeDateTime;

            var dayExpression = Build(memberExpression,
                           Expression.Constant(dateValue.Day), "Day", "Equals", typeof(Int32));
            var monthExpression = Build(memberExpression,
                Expression.Constant(dateValue.Month), "Month", "Equals", typeof(Int32));
            var yearExpression = Build(memberExpression,
                Expression.Constant(dateValue.Year), "Year", "Equals", typeof(Int32));

            var andExpression = Expression.AndAlso(dayExpression, monthExpression);

            expression = Expression.AndAlso(andExpression, yearExpression);

            var mapper = new Mock<IDataMapper>();

            mapper
                .Setup(x => x.GetMappingForType(typeof(DateClass)))
                .Returns(GetDateMapping);

            Mocks
                .Get<IObjectMapperFactory>()
                .Setup(x => x.GetMapper(It.IsAny<MappingKind>()))
                .Returns(mapper.Object);

            expected = "DAY(t1.t1_SomeDate) = {0} AND MONTH(t1.t1_SomeDate) = {1} AND YEAR(t1.t1_SomeDate) = {2}"
                .FormatString(dateValue.Day.ToString(), dateValue.Month.ToString(), dateValue.Year.ToString());

            var actual = SystemUnderTest.BuildContainer(expression, typeof(DateClass), tableAlias:"t1", fieldPrefix:"t1_");

            Asserter.AssertEquality(new QueryContainer(expected), actual, new[] { "Parameters", "OrderByClause" });
        }

        [Test]
        public void WillHandleDateTimeWithParameter()
        {
            Expression<Func<DateClass, bool>> expression = null;
            string expected = null;
            DateClass dateClass = null;

            dateClass = ObjectCreator.CreateNew<DateClass>();

            var dateValue = dateClass.SomeDateTime;

            expression = d => d.Name == dateClass.Name && d.SomeDateTime.Day.Equals(dateValue.Day)
                              && d.SomeDateTime.Month.Equals(dateValue.Month) &&
                              d.SomeDateTime.Year.Equals(dateValue.Year);

            var mapper = new Mock<IDataMapper>();

            mapper
                .Setup(x => x.GetMappingForType(typeof(DateClass)))
                .Returns(GetDateMapping);

            Mocks
                .Get<IObjectMapperFactory>()
                .Setup(x => x.GetMapper(It.IsAny<MappingKind>()))
                .Returns(mapper.Object);

            expected = "SomeName = @someName AND DAY(SomeDate) = {0} AND MONTH(SomeDate) = {1} AND YEAR(SomeDate) = {2}"
                .FormatString(dateValue.Day.ToString(), dateValue.Month.ToString(), dateValue.Year.ToString());

            var actual = SystemUnderTest.BuildContainer(expression, typeof(DateClass));
            Expression<Action<SqlParameter, SqlParameter>> compareExpression =
                (e, a) => CompareParameters(e, a);

            Asserter.AssertEquality(new QueryContainer(expected), actual, propertiesToIgnore: new[] { "Parameters", "OrderByClause" });

            Asserter.AssertEquality(new List<SqlParameter>
            {
                new SqlParameter("@someName", SqlDbType.NVarChar)
                {
                    Value = dateClass.Name
                }
            }, actual.Parameters
                    .Select(p => (SqlParameter)p), 
            additionalParameters: new Dictionary<string, object>
            {
                {
                    Norml.Tests.Common.Constants.ParameterNames.ComparisonDelegate,
                    compareExpression
                }
            });
        }

        private Expression Build(MemberExpression memberExpression, ConstantExpression constantExpression,
           string propertyName, string methodName, Type methodType)
        {
            var property = typeof(DateTime).GetProperty(propertyName);
            var membExpression = Expression.MakeMemberAccess(memberExpression, property);
            var methodInfo = methodType.GetMethod(methodName, new[] { methodType });

            var expression = Expression.Call(membExpression, methodInfo, constantExpression);

            return expression;
        }

        private class DateClass
        {
            public string Name { get; set; }
            public DateTime SomeDateTime { get; set; }
        }

        private class DummyClass
        {
            public int SomeProperty { get; set; }
        }

        private void CompareParameters(SqlParameter expected, SqlParameter actual)
        {
            Asserter.AssertEquality(expected.ParameterName, actual.ParameterName);
            Asserter.AssertEquality(expected.SqlDbType, actual.SqlDbType);
            Assert.AreEqual(expected.Value, actual.Value);
        }

        private TypeMapping GetTestClassTypeMapping()
        {
            return new TypeMapping
            {
                Type = typeof(TestClass),
                DataSource = "dbo.TestTable",
                PropertyMappings = GetPropertyMappings()
            };
        }

        private IList<PropertyMapping> GetPropertyMappings()
        {
            return new List<PropertyMapping>
            {
                new PropertyMapping
                {
                    IsIdentity = true,
                    DatabaseType = SqlDbType.Int,
                    Field = "TestClassId",
                    AllowDbNull = false, 
                    IsPrimaryKey = true,
                    IsPrimitive = true,
                    MappedType = typeof(int),
                    PropertyName = "Id",
                    ParameterName = "@id"
                },
                new PropertyMapping
                {
                    DatabaseType = SqlDbType.NVarChar,
                    Field = "SomeFoo",
                    AllowDbNull = false,
                    IsPrimitive = false,
                    MappedType = typeof(string),
                    PropertyName = "Foo",
                    ParameterName = "@fooParameter"
                },
                new PropertyMapping
                {
                    DatabaseType = SqlDbType.NVarChar,
                    Field = "PioneerSquareBar",
                    IsPrimitive = false,
                    MappedType = typeof(string),
                    PropertyName = "Bar",
                    ParameterName = "@itsFridayLetsGoToTheBar"
                }
            };
        }

        private TypeMapping GetDateMapping()
        {
            return new TypeMapping
            {
                DataSource = "dbo.Date",
                Type = typeof(DateClass),
                PropertyMappings = new List<PropertyMapping>
                {
                    new PropertyMapping
                    {
                        DatabaseType = SqlDbType.DateTime,
                        Field = "SomeDate",
                        AllowDbNull = false,
                        IsPrimitive = false,
                        PropertyName = "SomeDateTime"
                    },
                    new PropertyMapping
                    {
                        DatabaseType = SqlDbType.NVarChar,
                        Field = "Name",
                        AllowDbNull = true,
                        PropertyName = "Name"
                    }
                }
            };
        }
    }
}