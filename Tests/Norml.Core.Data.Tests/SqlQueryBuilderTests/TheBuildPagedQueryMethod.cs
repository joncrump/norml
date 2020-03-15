
using Moq;
using NUnit.Framework;
using Norml.Common.Data.QueryBuilders;
using Norml.Common.Data.QueryBuilders.Strategies;
using Norml.Tests.Common.Base;

namespace Norml.Common.Data.Tests.SqlQueryBuilderTests
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
            SystemUnderTest.BuildPagedQuery<TestClass>(null);

            Mocks.Get<IQueryBuilderStrategyFactory>()
                .Verify(x => x.GetBuilderStrategy(QueryKind.SelectSingleTable), Times.Once);
        }

        [Test]
        public void WillNotInvokeSelectSingleStrategyIfPagingInfoIsNotNull()
        {
            SystemUnderTest.BuildPagedQuery<TestClass>(ObjectCreator.CreateNew<PagingInfo>());

            Mocks.Get<IQueryBuilderStrategyFactory>()
               .Verify(x => x.GetBuilderStrategy(QueryKind.SelectSingleTable), Times.Never());
        }

        [Test]
        public void WillInvokePagedSingleStrategy()
        {
            SystemUnderTest.BuildPagedQuery<TestClass>(ObjectCreator.CreateNew<PagingInfo>());

            Mocks.Get<IQueryBuilderStrategyFactory>()
                .Verify(x => x.GetBuilderStrategy(QueryKind.PagedSingle), Times.Once);
        }
    }
}
