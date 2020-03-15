using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Moq;
using NUnit.Framework;
using Norml.Tests.Common.Base;

namespace Norml.Common.Data.Tests.FieldHelperTests
{
    [TestFixture]
    public class TheExtractParametersMethod : MockTestBase<FieldHelper>
    {
        [Test]
        public void WillReturnEmptyEnumerableIfFieldContainersAreNull()
        {
            var actual = SystemUnderTest.ExtractParameters(null, It.IsAny<bool>());

            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Count());
        }

        [Test]
        public void WillReturnEmptyEnumerableIfFieldContainersValuesAreNull()
        {
            var actual =
                SystemUnderTest.ExtractParameters(new TableObjectMapping
                {
                    TableName = DataGenerator.GenerateString()
                }, It.IsAny<bool>());

            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Count());
        }

        [Test]
        public void WillReturnEmptyEnumerableIfFieldContainersValuesAreEmpty()
        {
            var actual =
                SystemUnderTest.ExtractParameters(new TableObjectMapping
                {
                    TableName = DataGenerator.GenerateString(),
                    FieldMappings = new Dictionary<string, FieldParameterMapping>()
                }, It.IsAny<bool>());

            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Count());
        }

        [Test]
        public void WillReturnAllParametersIfIgnoreIdentityIsFalse()
        {
            IEnumerable<IDbDataParameter> expected = null;
            TableObjectMapping values = null;

            var id = DataGenerator.GenerateInteger();
            var barRescue = DataGenerator.GenerateDateTime();
            var baz = DataGenerator.GenerateString();

            throw new NotImplementedException();
            
//            values = new TableObjectMapping
//            {
//                TableName = DataGenerator.GenerateString(),
//                FieldMappings = new Dictionary<string, FieldParameterMapping>
//                    {
//                        {"Id", new FieldParameterMapping("TestId", "@theId", SqlDbType.Int, id, true)},
//                        {"Bar", new FieldParameterMapping("TestBar", "@barRescue", SqlDbType.DateTime, barRescue)},
//                        {"Baz", new FieldParameterMapping("TestBaz", "@baz", SqlDbType.NVarChar, baz)},
//                        {"Bak", new FieldParameterMapping("TestBak", "@bak", SqlDbType.NVarChar)}
//                    }
//            };
                
//            expected = new List<IDbDataParameter>
//            {
//                new SqlParameter("@theId", SqlDbType.Int) {Value = id},
//                new SqlParameter("@barRescue", SqlDbType.DateTime) {Value = barRescue},
//                new SqlParameter("@baz", SqlDbType.NVarChar) {Value = baz},
//                new SqlParameter("@bak", SqlDbType.NVarChar) {Value = DBNull.Value}
//            };
//
//            var actual = SystemUnderTest.ExtractParameters(values, false);
//            Expression<Action<IDbDataParameter, IDbDataParameter>> expression =
//                (e, a) => CompareParameters(e, a);
//            
//            Asserter.AssertEquality(expected, actual, additionalParameters:
//                new Dictionary<string, object>
//                {
//                    {
//                        Norml.Tests.Common.Constants.ParameterNames.ComparisonDelegate, 
//                        expression
//                    }
//                });
        }

        [Test]
        public void WillReturnAllParametersIfIgnoreIdentityIsTrue()
        {
            IEnumerable<IDbDataParameter> expected = null;
            TableObjectMapping values = null;

            throw new NotImplementedException();
            
//            var id = DataGenerator.GenerateInteger();
//            var barRescue = DataGenerator.GenerateDateTime();
//            var baz = DataGenerator.GenerateString();
//
//            values = new TableObjectMapping
//            {
//                TableName = DataGenerator.GenerateString(),
//                FieldMappings = new Dictionary<string, FieldParameterMapping>
//                    {
//                        {"Id", new FieldParameterMapping("TestId", "@theId", SqlDbType.Int, id, true)},
//                        {"Bar", new FieldParameterMapping("TestBar", "@barRescue", SqlDbType.DateTime, barRescue)},
//                        {"Baz", new FieldParameterMapping("TestBaz", "@baz", SqlDbType.NVarChar, baz)},
//                        {"Bak", new FieldParameterMapping("TestBak", "@bak", SqlDbType.NVarChar)}
//                    }
//            };
//                
//            expected = new List<IDbDataParameter>
//            {
//                new SqlParameter("@barRescue", SqlDbType.DateTime) {Value = barRescue},
//                new SqlParameter("@baz", SqlDbType.NVarChar) {Value = baz},
//                new SqlParameter("@bak", SqlDbType.NVarChar) {Value = DBNull.Value}
//            };
//
//            var actual = SystemUnderTest.ExtractParameters(values, true);
//            Expression<Action<IDbDataParameter, IDbDataParameter>> expression =
//                (e, a) => CompareParameters(e, a);
//
//            Asserter.AssertEquality(expected, actual, additionalParameters:
//                new Dictionary<string, object>
//                {
//                    {
//                        Norml.Tests.Common.Constants.ParameterNames.ComparisonDelegate, 
//                        expression
//                    }
//                });
        }

        private void CompareParameters(IDbDataParameter expected, IDbDataParameter actual)
        {
            Asserter.AssertEquality(expected.ParameterName, actual.ParameterName);
            Asserter.AssertEquality(expected.DbType, actual.DbType);
            Assert.AreEqual(expected.Value, actual.Value);
        }
    }
}
