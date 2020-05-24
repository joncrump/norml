using Moq;
using Norml.Core.Data.QueryBuilders;
using Norml.Core.Data.QueryBuilders.Strategies;
using Norml.Core.Tests.Common.Base;
using NUnit.Framework;

namespace Norml.Core.Data.Tests.SqlQueryBuilderTests
{
    [TestFixture]
    public class TheBuildPagedQueryMethod : MockTestBase<SqlQueryBuilder>
    {
        protected override void Setup()
        {
            base.Setup();

            Mocks.Get<IQueryBuilderStrategyFactory>()
                .Setup(x => x.GetBuilderStrategy(It.IsAny<QueryKind>()))
                .Returns(new Mock<IQueryBuilderStrategy>().Object);
        }

        [Test]
        public void WillInvokeSelectSingleStrategyIfPagingInfoIsNull()
        {
            SystemUnderTest.BuildPagedQuery<TestFixture>(null);

            Mocks.Get<IQueryBuilderStrategyFactory>()
                .Verify(x => x.GetBuilderStrategy(QueryKind.SelectSingleTable), Times.Once);
        }

        [Test]
        public void WillNotInvokeSelectSingleStrategyIfPagingInfoIsNotNull()
        {
            SystemUnderTest.BuildPagedQuery<TestFixture>(ObjectCreator.CreateNew<PagingInfo>());

            Mocks.Get<IQueryBuilderStrategyFactory>()
               .Verify(x => x.GetBuilderStrategy(QueryKind.SelectSingleTable), Times.Never());
        }

        [Test]
        public void WillInvokePagedSingleStrategy()
        {
            SystemUnderTest.BuildPagedQuery<TestFixture>(ObjectCreator.CreateNew<PagingInfo>());

            Mocks.Get<IQueryBuilderStrategyFactory>()
                .Verify(x => x.GetBuilderStrategy(QueryKind.PagedSingle), Times.Once);
        }
    }
}
