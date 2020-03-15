using System;
using System.Data;
using System.Linq.Expressions;

namespace Norml.Core.Data.Tests.ReflectionBasedDataReaderBuilderTests
{
    [TestFixture]
    public class TheBuildMethod : MockTestBase<ReflectionBasedDataReaderBuilder>
    {
        [Test]
        public void WillThrowArgumentNullExceptionIfDataReaderIsNull()
        {
            Asserter
                .AssertException<ArgumentNullException>(
                        () => SystemUnderTest.Build<object>(null))
                .AndVerifyHasParameter("dataSource");
        }

        [Test]
        public void WillBuildItemWithoutValueFactoriesAndNoPrefix()
        {
            TestFixture expected = null;
            MockDataReader reader = null;

            expected = ObjectCreator.CreateNew<TestFixture>();
            reader = new MockDataReaderHelper().BuildMockDataReader(new[] {expected});
        
            var actual = SystemUnderTest.Build<TestFixture>(reader);

            Asserter.AssertEquality(expected, actual);
        }

        [Test]
        public void WillBuildItemWithoutValueFactoriesAndPrefix()
        {
            TestFixture expected = null;
            MockDataReader reader = null;
            var prefix = String.Empty;

            prefix = DataGenerator.GenerateString();
            expected = ObjectCreator.CreateNew<TestFixture>();
            reader = new MockDataReaderHelper().BuildMockDataReader(new[] {expected}, prefix);
            var actual = SystemUnderTest.Build<TestFixture>(reader, prefix);

            Asserter.AssertEquality(expected, actual);
        }

        [Test]
        public void WillBuildItemWithValueFactories()
        {
            TestFixture expected = null;
            MockDataReader reader = null;
            Mock<IValueFactory> valueFactory = null;

            var valueFactoryModel = ObjectCreator.CreateNew<TestFixtureWithValueFactories>();

            expected = ObjectCreator.CreateNew<TestFixture>();
            reader = new MockDataReaderHelper().BuildMockDataReader(new [] {valueFactoryModel});

            valueFactory = Mocks.Get<IValueFactory>();
            Expression<Func<object>> fakeExpression = () => expected;

            valueFactory
                .Setup(x => x.GetValueFactory(It.IsAny<string>(), It.IsAny<ParameterInfo>()))
                .Returns(fakeExpression);
    
            var actual = SystemUnderTest.Build<TestFixtureWithValueFactories>(reader);

            valueFactory
                .Verify(x => x.GetValueFactory("TestFixture1", It.IsAny<ParameterInfo>()), 
                    Times.Once);

            valueFactory
                .Verify(x => x.GetValueFactory("TestFixture2", It.IsAny<ParameterInfo>()), 
                    Times.Once);

            Assert.IsFalse(actual.ValueFactories.IsNullOrEmpty());

            Assert.IsTrue(actual.ValueFactories.ContainsKey("TestFixture1"));
            Assert.IsNotNull(actual.ValueFactories["TestFixture1"]);
            Asserter.AssertEquality(expected, actual.TestFixture1);

            Assert.IsTrue(actual.ValueFactories.ContainsKey("TestFixture2"));
            Assert.IsNotNull(actual.ValueFactories["TestFixture2"]);
            Asserter.AssertEquality(expected, actual.TestFixture2);
        }

        public class TestFixture
        {
            [FieldMetadata("Id", SqlDbType.Int)]
            public int Id { get; set; }

            [FieldMetadata("ClassFoo", SqlDbType.NVarChar)]
            public string Foo { get; set; }

            [FieldMetadata("Baz", SqlDbType.NVarChar)]
            public string Baz { get; set; }
        }

        public class TestFixtureWithValueFactories : ValueFactoryModelBase
        {
            private TestFixture _TestFixture1;
            private TestFixture _TestFixture2;

            [FieldMetadata("Id", SqlDbType.Int)]
            public int Id { get; set; }

            [ValueFactory("TestFixture1")]
            public TestFixture TestFixture1
            {
                get
                {
                    if (_TestFixture1.IsNull())
                    {
                        _TestFixture1 = GetOrLoadLazyValue(_TestFixture1, "TestFixture1");
                    }

                    return _TestFixture1;
                }
                set
                {
                    _TestFixture1 = value;
                }
            }

            [ValueFactory("TestFixture2")]
            public TestFixture TestFixture2
            {
                get
                {
                    if (_TestFixture2.IsNull())
                    {
                        _TestFixture2 = GetOrLoadLazyValue(_TestFixture2, "TestFixture2");
                    }

                    return _TestFixture2;
                }
                set
                {
                    _TestFixture2 = value;
                }
            }
        }
    }
}
