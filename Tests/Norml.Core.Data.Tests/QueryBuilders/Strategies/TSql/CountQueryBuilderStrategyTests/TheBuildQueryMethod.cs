using System;
using Moq;
using Norml.Core.Data.QueryBuilders.Strategies.TSql;
using Norml.Core.Extensions;
using Norml.Core.Tests.Common.Base;
using NUnit.Framework;

namespace Norml.Core.Data.Tests.QueryBuilders.Strategies.TSql.CountQueryBuilderStrategyTests
{
    [TestFixture]
    public class TheBuildQueryMethod : MockTestBase<CountQueryBuilderStrategy>
    {
        [Test]
        public void WillThrowInvalidOperationExceptionIfTypeHasNoCountMetadataAttribute()
        {
            Asserter
                .AssertException<InvalidOperationException>(
                    () => SystemUnderTest.BuildQuery<DummyCountClassWithNoCountAttributes>(It.IsAny<dynamic>()))
                .AndVerifyMessageContains("Could not build query. Type {0} does not have a count attribute."
                    .FormatString(typeof (DummyCountClassWithNoCountAttributes).ToString()));
        }

        [Test]
        public void WillThrowInvalidOperationExceptionIfTypeHasNoTableAttribute()
        {
            Asserter
                .AssertException<InvalidOperationException>(
                    () => SystemUnderTest.BuildQuery<DummyCountClassWithNoAttributes>(It.IsAny<dynamic>()))
                .AndVerifyMessageContains("Could not build query. Type {0} does not have a table attribute."
                    .FormatString(typeof (DummyCountClassWithNoAttributes)));
        }

        [Test]
        public void WillBuildCountQuery()
        {
            QueryInfo expected = null;

            const string query = "SELECT COUNT(Id) AS IdCount FROM dbo.SomeTable;";

            expected = new QueryInfo(query);

            var actual = SystemUnderTest.BuildQuery<DummyCountClassWithCountAttributes>(It.IsAny<dynamic>());

            Assert.IsNotNull(actual);
            Asserter.AssertEquality(expected.Query, actual.Query);
            Assert.IsNull(actual.Parameters);
        }

        public class DummyCountClassWithNoAttributes
        {
        }

        public class DummyCountClassWithCountAttributes
        {
            public int Id { get; set; }
        }

        public class DummyCountClassWithNoCountAttributes
        {
            public int Id { get; set; }
        }
    }
}
