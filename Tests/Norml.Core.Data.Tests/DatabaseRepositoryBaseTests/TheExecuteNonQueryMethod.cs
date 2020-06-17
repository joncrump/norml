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
    public class TheExecuteNonQueryMethod : MockTestBase<TestableDatabaseRepository>
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
                    () => SystemUnderTest.ExecuteNonQuery(null))
                .AndVerifyHasParameter("queryInfo");
        }

        [Test]
        public void WillInvokeDatabaseCreateCommandTextMethod()
        {
            SystemUnderTest.ExecuteNonQuery(Mock.Of<QueryInfo>());

            _databaseWrapper
                .Verify(x => x.CreateCommandText(It.IsAny<string>(), QueryType.Text),
                    Times.Once);
        }

        [Test]
        public void WillNotInvokeDatabaseWithParametersMethodIfParametersAreNull()
        {
            QueryInfo queryInfo = null;

            queryInfo = new QueryInfo { Parameters = null };

            SystemUnderTest.ExecuteNonQuery(queryInfo);

            _databaseWrapper
                .Verify(x => x.WithParameters(It.IsAny<IEnumerable<IDbDataParameter>>()),
                    Times.AtMostOnce);
        }

        [Test]
        public void WillNotInvokeDatabaseWithParametersMethodIfParametersAreEmpty()
        {
            QueryInfo queryInfo = null;

            queryInfo = new QueryInfo
            {
                Parameters = Enumerable.Empty<IDbDataParameter>()
            };

            SystemUnderTest.ExecuteNonQuery(queryInfo);

            _databaseWrapper
                .Verify(x => x.WithParameters(It.IsAny<IEnumerable<IDbDataParameter>>()),
                    Times.AtMostOnce);
        }

        [Test]
        public void WillInvokeDatabaseWithParametersMethodIfParametersHaveValues()
        {
            QueryInfo queryInfo = null;

            queryInfo = new QueryInfo
            {
                Parameters = new List<SqlParameter>
                {
                    new SqlParameter()
                }
            };

            SystemUnderTest.ExecuteNonQuery(queryInfo);

            _databaseWrapper
                .Verify(x => x.WithParameters(It.IsAny<IEnumerable<IDbDataParameter>>()),
                    Times.AtLeastOnce);
        }

        [Test]
        public void WillInvokeDatabaseExecuteNonQueryMethod()
        {
            SystemUnderTest.ExecuteNonQuery(Mock.Of<QueryInfo>());

            _databaseWrapper
                .Verify(x => x.ExecuteNonQuery(), Times.AtLeastOnce);
        }
    }
}
