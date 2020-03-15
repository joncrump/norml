using System;
using System.Linq.Expressions;

namespace Norml.Core.Data.Tests.SqlQueryBuilderTests
{
    [TestFixture]
    public class TheBuildDeleteQueryMethod : MockTestBase<SqlQueryBuilder>
    {
        protected override void Setup()
        {
            base.Setup();

            Mocks.Get<IQueryBuilderStrategyFactory>()
                .Setup(x => x.GetBuilderStrategy(QueryKind.Delete))
                .Returns(new Mock<IQueryBuilderStrategy>().Object);
        }
        
        [Test]
        public void WillThrowArgumentNullExceptionIfPredicateIsNull()
        {
            Asserter
                .AssertException<ArgumentNullException>(
                    () => SystemUnderTest.BuildDeleteQuery<TestFixture>(null, It.IsAny<string>()))
                .AndVerifyMessageContains("predicate");
        }

        [Test]
        public void WillInvokeQueryBuilderStrategy()
        {
            Expression<Func<TestFixture, bool>> predicate = t => t.Id == 5;

            SystemUnderTest.BuildDeleteQuery(predicate);

            Mocks.Get<QueryBuilderStrategyFactory>()
                .Verify(x => x.GetBuilderStrategy(QueryKind.Delete), Times.Once);
        }
    }
}