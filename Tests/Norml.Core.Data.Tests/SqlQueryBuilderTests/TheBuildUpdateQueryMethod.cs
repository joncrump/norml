using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Norml.Core.Data.Tests.SqlQueryBuilderTests
{
    [TestFixture]
    public class TheBuildUpdateQueryMethod : MockTestBase<SqlQueryBuilder>
    {
        protected override void Setup()
        {
            base.Setup();

            Mocks.Get<IQueryBuilderStrategyFactory>()
                .Setup(x => x.GetBuilderStrategy(QueryKind.Update))
                .Returns(new Mock<IQueryBuilderStrategy>().Object);
        }

        [Test]
        public void WillThrowArgumentNullExceptionIfModelIsNull()
        {
            Asserter.AssertException<ArgumentNullException>(
                () => SystemUnderTest.BuildUpdateQuery(null,
                    It.IsAny<Expression<Func<TestFixture, bool>>>(),
                    It.IsAny<IEnumerable<string>>(), It.IsAny<string>()))
            .AndVerifyMessageContains("model");
        }

        [Test]
        public void WillThrowArgumentNullExceptionIfPredicateIsNull()
        {
            Asserter.AssertException<ArgumentNullException>(
                () => SystemUnderTest.BuildUpdateQuery(Mock.Of<TestFixture>(),
                    null,
                    It.IsAny<IEnumerable<string>>(), It.IsAny<string>()))
            .AndVerifyMessageContains("predicate");
        }

        [Test]
        public void WillInvokeQueryBuilderStrategyFactory()
        {
            Expression<Func<TestFixture, bool>> predicate = t => t.Id == 5;

            SystemUnderTest.BuildUpdateQuery(Mock.Of<TestFixture>(), predicate,
                It.IsAny<IEnumerable<string>>(), It.IsAny<string>());

            Mocks.Get<IQueryBuilderStrategyFactory>()
                .Verify(x => x.GetBuilderStrategy(QueryKind.Update), Times.Once);
        }
    }
}
