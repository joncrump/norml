using System;
using System.Collections.Generic;
using System.Data;
using Moq;
using Norml.Core.Data.Repositories.Strategies;
using Norml.Core.Tests.Common.Base;
using NUnit.Framework;

namespace Norml.Core.Data.Tests.DatabaseRepositoryBaseTests
{
    [TestFixture]
    public class TheExecuteSingleMethod : MockTestBase<TestableDatabaseRepository>
    {
        private Mock<IDatabaseWrapper> _databaseWrapper;

        protected override void Setup()
        {
            base.Setup();

            var strategy = new Mock<IBuilderStrategy>();
            _databaseWrapper = new Mock<IDatabaseWrapper>();

            _databaseWrapper.Setup(x => x.CreateCommandText(It.IsAny<string>(), It.IsAny<QueryType>()))
                .Returns(_databaseWrapper.Object);

            _databaseWrapper.Setup(x => x.WithParameters(It.IsAny<IEnumerable<IDbDataParameter>>()))
                .Returns(_databaseWrapper.Object);

            Mocks.Get<IDatabaseFactory>()
                .Setup(x => x.GetDatabase(It.IsAny<string>()))
                .Returns(_databaseWrapper.Object);

            Mocks.Get<IBuilderStrategyFactory>()
                .Setup(x => x.GetStrategy(It.IsAny<BuildMode>()))
                .Returns(strategy.Object);
        }

        [Test]
        public void WillThrowArgumentNullExceptionIfQueryInfoIsNull()
        {
            Asserter
                .AssertException<ArgumentNullException>(
                    () => SystemUnderTest.ExecuteSingle<object>(null, It.IsAny<BuildMode>()))
                .AndVerifyHasParameter("queryInfo");
        }

        [Test]
        public void WillInvokeBuilderStrategyFactory()
        {
            SystemUnderTest.ExecuteSingle<object>(Mock.Of<QueryInfo>(), BuildMode.Joined);

            Mocks.Get<IBuilderStrategyFactory>()
                .Verify(x => x.GetStrategy(BuildMode.Joined), Times.Once);
        }

        [Test]
        public void WillInvokeDatabaseCreateCommandText()
        {
            SystemUnderTest.ExecuteSingle<object>(Mock.Of<QueryInfo>());

            _databaseWrapper
                .Verify(x => x.CreateCommandText(It.IsAny<string>(), QueryType.Text),
                    Times.AtLeastOnce);
        }

        [Test]
        public void WillInvokeDatabaseWithParameters()
        {
            SystemUnderTest.ExecuteSingle<object>(Mock.Of<QueryInfo>());

            _databaseWrapper
                .Verify(x => x.WithParameters(It.IsAny<IEnumerable<IDbDataParameter>>()),
                    Times.AtLeastOnce);
        }

        [Test]
        public void WillInvokeExecuteSingle()
        {
            SystemUnderTest.ExecuteSingle<object>(Mock.Of<QueryInfo>());

            _databaseWrapper
                .Verify(x => x.ExecuteSingle<object>(It.IsAny<IBuilderStrategy>(),
                    It.IsAny<IEnumerable<TableObjectMapping>>()), Times.AtLeastOnce);
        }
    }
}
