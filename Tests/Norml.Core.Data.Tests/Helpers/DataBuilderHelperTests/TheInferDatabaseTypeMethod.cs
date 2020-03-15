using NUnit.Framework;
using System;
using System.Data;
using Norml.Common.Data.Helpers;
using Norml.Tests.Common.Base;

namespace Norml.Common.Data.Tests.Helpers.DataBuilderHelperTests
{
    [TestFixture]
    public class TheInferDatabaseTypeMethod : MockTestBase<DataBuilderHelper>
    {
        [TestCase(typeof(long), SqlDbType.BigInt)]
        [TestCase(typeof(bool), SqlDbType.Bit)]
        [TestCase(typeof(DateTime), SqlDbType.DateTime)]
        [TestCase(typeof(decimal), SqlDbType.Decimal)]
        [TestCase(typeof(float), SqlDbType.Float)]
        [TestCase(typeof(object), SqlDbType.Image)]
        [TestCase(typeof(int), SqlDbType.Int)]
        [TestCase(typeof(char), SqlDbType.NChar)]
        [TestCase(typeof(string), SqlDbType.NVarChar)]
        [TestCase(typeof(Guid), SqlDbType.UniqueIdentifier)]
        [TestCase(typeof(short), SqlDbType.SmallInt)]
        [TestCase(typeof(byte), SqlDbType.TinyInt)]
        [TestCase(typeof(DateTimeOffset), SqlDbType.DateTimeOffset)]
        public void WillReturnExpectedMappings(Type type, SqlDbType expected)
        {
            var actual = SystemUnderTest.InferDatabaseType(type);
            Asserter.AssertEquality(expected, actual);
        }
    }
}
