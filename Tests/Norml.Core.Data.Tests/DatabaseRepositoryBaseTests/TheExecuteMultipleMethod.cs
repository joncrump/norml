using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Moq;
using NUnit.Framework;
using Norml.Common.Data.Repositories.Strategies;
using Norml.Tests.Common.Base;

namespace Norml.Common.Data.Tests.DatabaseRepositoryBaseTests
{
    [TestFixture]
    public class TheExecuteMultipleMethod : MockTestBase<TestableDatabaseRepository>
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
                    () => SystemUnderTest.ExecuteMultiple<object>(null, It.IsAny<BuildMode>()))
                .AndVerifyHasParameter("queryInfo");
        }

        [Test]
        public void WillInvokeDatabaseCreateCommandText()
        {
            SystemUnderTest.ExecuteMultiple<object>(Mock.Of<QueryInfo>(), BuildMode.Joined);

            throw new NotImplementedException();
//            MockDatabase
//                .Verify(x => x.CreateCommandText(It.IsAny<string>(), QueryType.Text),
//                    Times.Once);
        }

        [Test]
        public void WillInvokeTheDatabaseWithParametersMethod()
        {
            SystemUnderTest.ExecuteMultiple<object>(Mock.Of<QueryInfo>(), It.IsAny<BuildMode>());

            throw new NotImplementedException();
//                MockDatabase
//                    .Verify(x => x.WithParameters(It.IsAny<IEnumerable<IDbDataParameter>>()), 
//                        Times.Once);
        }

        [Test]
        public void WillInvokeDatabaseExecuteMultipleMethod()
        {
            SystemUnderTest.ExecuteMultiple<object>(Mock.Of<QueryInfo>(), It.IsAny<BuildMode>());

//                MockDatabase
//                    .Verify(x => x.ExecuteMultiple<object>(It.IsAny<IBuilderStrategy>(), 
//                        It.IsAny<IEnumerable<TableObjectMapping>>()), Times.Once);
//            });
        }

        [Test]
        public void WillInvokeBuilderStrategyFactory()
        {
            SystemUnderTest.ExecuteMultiple<object>(Mock.Of<QueryInfo>(), BuildMode.Joined);

//                Mocks.Get<Mock<IBuilderStrategyFactory>>()
//                    .Verify(x => x.GetStrategy(BuildMode.Joined), Times.Once);
//            });
        }
    }
}