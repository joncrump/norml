using Moq;
using NUnit.Framework;
using Norml.Common.Data.QueryBuilders;
using Norml.Common.Data.QueryBuilders.Strategies;
using Norml.Tests.Common.Base;

namespace Norml.Common.Data.Tests.SqlQueryBuilderTests
{
    [TestFixture]
    public class TheBuildCountQueryMethod : MockTestBase<SqlQueryBuilder>
    {
        [Test]
        public void WillInvokeQueryBuilderStrategyFactory()
        {
            Mocks.Get<IQueryBuilderStrategyFactory>()
                .Setup(x => x.GetBuilderStrategy(QueryKind.Count))
                .Returns(new Mock<IQueryBuilderStrategy>().Object);
            SystemUnderTest.BuildCountQuery<TestClass>();

            Mocks.Get<IQueryBuilderStrategyFactory>()
                .Verify(x => x.GetBuilderStrategy(QueryKind.Count), Times.Once);
        }
    }
}
