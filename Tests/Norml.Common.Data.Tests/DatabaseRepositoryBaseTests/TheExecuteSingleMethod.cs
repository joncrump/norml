using System;
using Moq;
using NUnit.Framework;
using Norml.Common.Data.Repositories.Strategies;
using Norml.Tests.Common.Base;

namespace Norml.Common.Data.Tests.DatabaseRepositoryBaseTests
{
    [TestFixture]
    public class TheExecuteSingleMethod : MockTestBase<TestableDatabaseRepository>
    {
        protected override void Setup()
        {
            base.Setup();

            var strategy = new Mock<IBuilderStrategy>();

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

            throw new NotImplementedException();
//            MockDatabase
//                .Verify(x => x.CreateCommandText(It.IsAny<string>(), QueryType.Text), 
//                    Times.Once);
        }

        [Test]
        public void WillInvokeDatabaseWithParameters()
        {
            SystemUnderTest.ExecuteSingle<object>(Mock.Of<QueryInfo>());

            throw new NotImplementedException();
//            MockDatabase
//                .Verify(x => x.WithParameters(It.IsAny<IEnumerable<IDbDataParameter>>()), 
//                    Times.Once);
        }

        [Test]
        public void WillInvokeExecuteSingle()
        {
            SystemUnderTest.ExecuteSingle<object>(Mock.Of<QueryInfo>());

            throw new NotImplementedException();
//            MockDatabase
//                .Verify(x => x.ExecuteSingle<object>(It.IsAny<IBuilderStrategy>(), 
//                    It.IsAny<IEnumerable<TableObjectMapping>>()), Times.Once);
        }
    }
}
