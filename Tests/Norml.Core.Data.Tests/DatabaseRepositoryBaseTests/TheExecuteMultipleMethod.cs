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
    public class TheExecuteMultipleMethod : MockTestBase<TestableDatabaseRepository>
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
                    () => SystemUnderTest.ExecuteMultiple<object>(null, It.IsAny<BuildMode>()))
                .AndVerifyHasParameter("queryInfo");
        }

        [Test]
        public void WillInvokeDatabaseCreateCommandText()
        {
            SystemUnderTest.ExecuteMultiple<object>(Mock.Of<QueryInfo>(), BuildMode.Joined);

            _databaseWrapper
                .Verify(x => x.CreateCommandText(It.IsAny<string>(), QueryType.Text),
                    Times.AtLeastOnce);
        }

        [Test]
        public void WillInvokeTheDatabaseWithParametersMethod()
        {
            SystemUnderTest.ExecuteMultiple<object>(Mock.Of<QueryInfo>(), It.IsAny<BuildMode>());

            _databaseWrapper
                .Verify(x => x.WithParameters(It.IsAny<IEnumerable<IDbDataParameter>>()),
                    Times.AtLeastOnce);
        }

        [Test]
        public void WillInvokeDatabaseExecuteMultipleMethod()
        {
            SystemUnderTest.ExecuteMultiple<object>(Mock.Of<QueryInfo>(), It.IsAny<BuildMode>());

            _databaseWrapper
                .Verify(x => x.ExecuteMultiple<object>(It.IsAny<IBuilderStrategy>(),
                    It.IsAny<IEnumerable<TableObjectMapping>>()), Times.AtLeastOnce);
        }

        [Test]
        public void WillInvokeBuilderStrategyFactory()
        {
            SystemUnderTest.ExecuteMultiple<object>(Mock.Of<QueryInfo>(), BuildMode.Joined);

            Mocks.Get<IBuilderStrategyFactory>()
                .Verify(x => x.GetStrategy(BuildMode.Joined), Times.AtLeastOnce);
        }
    }
}