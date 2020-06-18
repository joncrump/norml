using System;
using Microsoft.Extensions.Configuration;
using Moq;
using Norml.Core.Data.Mappings;
using Norml.Core.Data.QueryBuilders.Strategies.TSql;
using Norml.Core.Extensions;
using Norml.Core.Tests.Common.Base;
using NUnit.Framework;

namespace Norml.Core.Data.Tests.QueryBuilders.Strategies.TSql.CountQueryBuilderStrategyTests
{
    [TestFixture]
    public class TheBuildQueryMethod : MockTestBase<CountQueryBuilderStrategy>
    {
        protected override void Setup()
        {
            base.Setup();

            Mocks.Get<IConfiguration>()
                .Setup(x => x[Constants.Configuration.MappingKind])
                .Returns("Attribute");
        }

        [Test]
        public void WillThrowInvalidOperationExceptionIfTypeHasNoCountMetadataAttribute()
        {
            var mapper = new Mock<IDataMapper>();
            var typeMapper = new TypeMapping
            {
                DataSource = DataGenerator.GenerateString()
            };

            mapper
                .Setup(x => x.GetMappingFor<TestClass>())
                .Returns(typeMapper);

            Mocks.Get<IObjectMapperFactory>()
                .Setup(x => x.GetMapper(It.IsAny<MappingKind>()))
                .Returns(mapper.Object);

            Asserter
                .AssertException<InvalidOperationException>(
                    () => SystemUnderTest.BuildQuery<TestClass>(It.IsAny<dynamic>()))
                .AndVerifyMessageContains("Could not build query. Type {0} does not have a count mapping."
                    .FormatString(typeof (TestClass).ToString()));
        }

        [Test]
        public void WillThrowInvalidOperationExceptionIfTypeHasNoTableAttribute()
        {
            var mapper = new Mock<IDataMapper>();
            var typeMapper = new TypeMapping();

            mapper
                .Setup(x => x.GetMappingFor<TestClass>())
                .Returns(typeMapper);

            Mocks.Get<IObjectMapperFactory>()
                .Setup(x => x.GetMapper(It.IsAny<MappingKind>()))
                .Returns(mapper.Object);

            Asserter
                .AssertException<InvalidOperationException>(
                    () => SystemUnderTest.BuildQuery<TestClass>(It.IsAny<dynamic>()))
                .AndVerifyMessageContains("Could not build query. Type {0} does not have a data source mapping."
                    .FormatString(typeof (TestClass)));
        }

        [Test]
        public void WillBuildCountQuery()
        {
            const string query = "SELECT COUNT(Id) AS IdCount FROM dbo.SomeTable;";

            var expected = new QueryInfo(query);

            var mapper = new Mock<IDataMapper>();
            var typeMapper = new TypeMapping
            {
                CountField = "Id",
                CountAlias = "IdCount",
                DataSource = "dbo.SomeTable"
            };

            mapper
                .Setup(x => x.GetMappingFor<TestClass>())
                .Returns(typeMapper);

            Mocks.Get<IObjectMapperFactory>()
                .Setup(x => x.GetMapper(It.IsAny<MappingKind>()))
                .Returns(mapper.Object);

            var actual = SystemUnderTest.BuildQuery<TestClass>(It.IsAny<dynamic>());

            Assert.IsNotNull(actual);
            Asserter.AssertEquality(expected.Query, actual.Query);
            Assert.IsNull(actual.Parameters);
        }

        public class TestClass
        {
            public int Id { get; set; }
        }
    }
}
