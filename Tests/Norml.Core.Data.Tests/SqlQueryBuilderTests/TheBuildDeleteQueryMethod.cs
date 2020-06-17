using System;
using System.Linq.Expressions;
using Moq;
using Norml.Core.Data.QueryBuilders;
using Norml.Core.Data.QueryBuilders.Strategies;
using Norml.Core.Tests.Common.Base;
using NUnit.Framework;

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
                    () => SystemUnderTest.BuildDeleteQuery<TestClass>(null, It.IsAny<string>()))
                .AndVerifyMessageContains("predicate");
        }

        [Test]
        public void WillInvokeQueryBuilderStrategy()
        {
            Expression<Func<TestClass, bool>> predicate = t => t.Id == 5;

            SystemUnderTest.BuildDeleteQuery(predicate);

            Mocks.Get<QueryBuilderStrategyFactory>()
                .Verify(x => x.GetBuilderStrategy(QueryKind.Delete), Times.Once);
        }
    }
}