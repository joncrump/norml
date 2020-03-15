using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Norml.Common.Data.QueryBuilders;
using Norml.Common.Data.QueryBuilders.Strategies;
using Norml.Tests.Common.Base;

namespace Norml.Common.Data.Tests.SqlQueryBuilderTests
{
    [TestFixture]
    public class TheBuildInsertQueryMethod : MockTestBase<SqlQueryBuilder>
    {
        protected override void Setup()
        {
            base.Setup();

            Mocks.Get<IQueryBuilderStrategyFactory>()
                .Setup(x => x.GetBuilderStrategy(QueryKind.Insert))
                .Returns(new Mock<IQueryBuilderStrategy>().Object);
        }

        [Test]
        public void WillThrowArgumentNullExceptionIfModelIsNull()
        {
            Asserter
                .AssertException<ArgumentNullException>(
                    () => SystemUnderTest.BuildInsertQuery<TestClass>(null, It.IsAny<bool>(), It.IsAny<bool>(),
                        It.IsAny<IEnumerable<string>>(), It.IsAny<string>()))
                .AndVerifyHasParameter("model");
        }

        [Test]
        public void WillInvokeQueryBuilderStrategyFactory()
        {
            SystemUnderTest.BuildInsertQuery(Mock.Of<TestClass>(), It.IsAny<bool>(),
                It.IsAny<bool>(), It.IsAny<IEnumerable<string>>(), It.IsAny<string>());

            Mocks.Get<IQueryBuilderStrategyFactory>()
                .Verify(x => x.GetBuilderStrategy(QueryKind.Insert), Times.Once);
        }
    }
}
