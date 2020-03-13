using System;
using System.Data;
using System.Linq.Expressions;
using Moq;
using NUnit.Framework;
using Norml.Common.Data.Attributes;
using Norml.Common.Extensions;
using Norml.Tests.Common;
using Norml.Tests.Common.Base;
using Norml.Tests.Common.Data;

namespace Norml.Common.Data.Tests.ReflectionBasedDataReaderBuilderTests
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
            TestClass expected = null;
            MockDataReader reader = null;

            expected = ObjectCreator.CreateNew<TestClass>();
            reader = new MockDataReaderHelper().BuildMockDataReader(new[] {expected});
        
            var actual = SystemUnderTest.Build<TestClass>(reader);

            Asserter.AssertEquality(expected, actual);
        }

        [Test]
        public void WillBuildItemWithoutValueFactoriesAndPrefix()
        {
            TestClass expected = null;
            MockDataReader reader = null;
            var prefix = String.Empty;

            prefix = DataGenerator.GenerateString();
            expected = ObjectCreator.CreateNew<TestClass>();
            reader = new MockDataReaderHelper().BuildMockDataReader(new[] {expected}, prefix);
            var actual = SystemUnderTest.Build<TestClass>(reader, prefix);

            Asserter.AssertEquality(expected, actual);
        }

        [Test]
        public void WillBuildItemWithValueFactories()
        {
            TestClass expected = null;
            MockDataReader reader = null;
            Mock<IValueFactory> valueFactory = null;

            var valueFactoryModel = ObjectCreator.CreateNew<TestClassWithValueFactories>();

            expected = ObjectCreator.CreateNew<TestClass>();
            reader = new MockDataReaderHelper().BuildMockDataReader(new [] {valueFactoryModel});

            valueFactory = Mocks.Get<IValueFactory>();
            Expression<Func<object>> fakeExpression = () => expected;

            valueFactory
                .Setup(x => x.GetValueFactory(It.IsAny<string>(), It.IsAny<ParameterInfo>()))
                .Returns(fakeExpression);
    
            var actual = SystemUnderTest.Build<TestClassWithValueFactories>(reader);

            valueFactory
                .Verify(x => x.GetValueFactory("TestClass1", It.IsAny<ParameterInfo>()), 
                    Times.Once);

            valueFactory
                .Verify(x => x.GetValueFactory("TestClass2", It.IsAny<ParameterInfo>()), 
                    Times.Once);

            Assert.IsFalse(actual.ValueFactories.IsNullOrEmpty());

            Assert.IsTrue(actual.ValueFactories.ContainsKey("TestClass1"));
            Assert.IsNotNull(actual.ValueFactories["TestClass1"]);
            Asserter.AssertEquality(expected, actual.TestClass1);

            Assert.IsTrue(actual.ValueFactories.ContainsKey("TestClass2"));
            Assert.IsNotNull(actual.ValueFactories["TestClass2"]);
            Asserter.AssertEquality(expected, actual.TestClass2);
        }

        public class TestClass
        {
            [FieldMetadata("Id", SqlDbType.Int)]
            public int Id { get; set; }

            [FieldMetadata("ClassFoo", SqlDbType.NVarChar)]
            public string Foo { get; set; }

            [FieldMetadata("Baz", SqlDbType.NVarChar)]
            public string Baz { get; set; }
        }

        public class TestClassWithValueFactories : ValueFactoryModelBase
        {
            private TestClass _testClass1;
            private TestClass _testClass2;

            [FieldMetadata("Id", SqlDbType.Int)]
            public int Id { get; set; }

            [ValueFactory("TestClass1")]
            public TestClass TestClass1
            {
                get
                {
                    if (_testClass1.IsNull())
                    {
                        _testClass1 = GetOrLoadLazyValue(_testClass1, "TestClass1");
                    }

                    return _testClass1;
                }
                set
                {
                    _testClass1 = value;
                }
            }

            [ValueFactory("TestClass2")]
            public TestClass TestClass2
            {
                get
                {
                    if (_testClass2.IsNull())
                    {
                        _testClass2 = GetOrLoadLazyValue(_testClass2, "TestClass2");
                    }

                    return _testClass2;
                }
                set
                {
                    _testClass2 = value;
                }
            }
        }
    }
}
