using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Moq;
using Norml.Core.Data.QueryBuilders;
using Norml.Core.Data.QueryBuilders.Strategies;
using Norml.Core.Tests.Common.Base;
using NUnit.Framework;

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
                    It.IsAny<Expression<Func<TestClass, bool>>>(),
                    It.IsAny<IEnumerable<string>>(), It.IsAny<string>()))
            .AndVerifyMessageContains("model");
        }

        [Test]
        public void WillThrowArgumentNullExceptionIfPredicateIsNull()
        {
            Asserter.AssertException<ArgumentNullException>(
                () => SystemUnderTest.BuildUpdateQuery(Mock.Of<TestClass>(),
                    null,
                    It.IsAny<IEnumerable<string>>(), It.IsAny<string>()))
            .AndVerifyMessageContains("predicate");
        }

        [Test]
        public void WillInvokeQueryBuilderStrategyFactory()
        {
            Expression<Func<TestClass, bool>> predicate = t => t.Id == 5;

            SystemUnderTest.BuildUpdateQuery(Mock.Of<TestClass>(), predicate,
                It.IsAny<IEnumerable<string>>(), It.IsAny<string>());

            Mocks.Get<IQueryBuilderStrategyFactory>()
                .Verify(x => x.GetBuilderStrategy(QueryKind.Update), Times.Once);
        }
    }
}
