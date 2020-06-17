using System;
using System.Collections.Generic;
using Moq;
using Norml.Core.Data.QueryBuilders;
using Norml.Core.Data.QueryBuilders.Strategies;
using Norml.Core.Tests.Common.Base;
using NUnit.Framework;

namespace Norml.Core.Data.Tests.SqlQueryBuilderTests
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
