using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Moq;
using Norml.Core.Data.Repositories.Strategies;
using Norml.Core.Tests.Common.Base;
using NUnit.Framework;

namespace Norml.Core.Data.Tests.DatabaseRepositoryBaseTests
{
    [TestFixture]
    public class TheExecuteTransformMethod : MockTestBase<TestableDatabaseRepository>
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
                    () =>
                        SystemUnderTest.ExecuteTransform(null, It.IsAny<Func<IDataReader, object>>(),
                            It.IsAny<Action<object>>()))
                .AndVerifyHasParameter("queryInfo");
        }

        [Test]
        public void WillThrowArgumentNullExceptionIfBuilderDelegateIsNull()
        {
            Asserter
                .AssertException<ArgumentNullException>(
                    () => SystemUnderTest.ExecuteTransform(Mock.Of<QueryInfo>(), null,
                        It.IsAny<Action<object>>()))
                .AndVerifyHasParameter("builderDelegate");
        }

        [Test]
        public void WillThrowArgumentNullExceptionIfTransformActionIsNull()
        {
            Func<IDataReader, object> builderDelegate = r => r;

            Asserter
                .AssertException<ArgumentNullException>(
                    () => SystemUnderTest.ExecuteTransform(Mock.Of<QueryInfo>(), builderDelegate,
                        null))
                .AndVerifyHasParameter("transformAction");
        }

        [Test]
        public void WillInvokeDatabaseCreateCommandTextMethod()
        {
            Func<IDataReader, object> builderDelegate = r => r;
            Action<object> transformAction = FakeMethod;

            SystemUnderTest.ExecuteTransform(Mock.Of<QueryInfo>(), builderDelegate, 
                transformAction);

            _databaseWrapper
                .Verify(x => x.CreateCommandText(It.IsAny<string>(), QueryType.Text),
                    Times.AtLeastOnce);
        }

        [Test]
        public void WillNotInvokeDatabaseWithParametersMethodIfParametersAreNull()
        {
            var queryInfo = new QueryInfo {Parameters = null};

            object BuilderDelegate(IDataReader r) => r;
            Action<object> transformAction = FakeMethod;

            SystemUnderTest.ExecuteTransform(queryInfo, BuilderDelegate, transformAction);

            _databaseWrapper
                .Verify(x => x.WithParameters(It.IsAny<IEnumerable<IDbDataParameter>>()),
                    Times.AtMostOnce);
        }

        [Test]
        public void WillNotInvokeDatabaseWithParametersMethodIfParametersAreEmpty()
        {
            var queryInfo = new QueryInfo
            {
                Parameters = Enumerable.Empty<IDbDataParameter>()
            };

            object BuilderDelegate(IDataReader r) => r;
            Action<object> transformAction = FakeMethod;

            SystemUnderTest.ExecuteTransform(queryInfo, BuilderDelegate, transformAction);

            _databaseWrapper
                .Verify(x => x.WithParameters(It.IsAny<IEnumerable<IDbDataParameter>>()),
                    Times.AtMostOnce);
        }

        [Test]
        public void WillInvokeDatabaseWithParametersMethodIfParametersHaveValues()
        {
            var queryInfo = new QueryInfo
            {
                Parameters = new List<SqlParameter>
                {
                    new SqlParameter()
                }
            };

            object BuilderDelegate(IDataReader r) => r;
            Action<object> transformAction = FakeMethod;

            SystemUnderTest.ExecuteTransform(queryInfo, BuilderDelegate, transformAction);

            _databaseWrapper
                .Verify(x => x.WithParameters(It.IsAny<IEnumerable<IDbDataParameter>>()),
                    Times.AtLeastOnce);
        }

        [Test]
        public void WillInvokeDatabaseExecuteTransformMethod()
        {
            Func<IDataReader, object> builderDelegate = r => r;
            Action<object> transformAction = FakeMethod;

            SystemUnderTest.ExecuteTransform(Mock.Of<QueryInfo>(), 
                builderDelegate, transformAction);

            _databaseWrapper
                .Verify(x => x.ExecuteTransform(It.IsAny<Func<IDataReader, object>>(),
                    It.IsAny<Action<object>>()), Times.AtLeastOnce);
        }

        private void FakeMethod(object value)
        {
        }
    }
}
