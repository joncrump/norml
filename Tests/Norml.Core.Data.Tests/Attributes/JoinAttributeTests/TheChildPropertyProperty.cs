using System;

namespace Norml.Core.Data.Tests.Attributes.JoinAttributeTests
{
    [TestFixture]
    public class TheChildPropertyProperty : MsTestBase
    {
        [Test]
        public void WillReturnRightKeyIfChildPropertyPassedToConstructorIsNull()
        {
            var expected = String.Empty;

            TestRunner
                .DoCustomSetup(() =>
                {
                    expected = DataGenerator.GenerateString();
                })
                .ExecuteTest(() =>
                {
                    var attribute = new JoinAttribute(JoinType.Left, typeof(object),
                        DataGenerator.GenerateString(), expected);

                    Assert.AreEqual(expected, attribute.RightKey);
                    Assert.AreEqual(expected, attribute.ChildProperty);
                });
        }

        [Test]
        public void WillReturnRightKeyIfChildPropertyPassedToConstructorIsEmpty()
        {
            var expected = String.Empty;

            TestRunner
                .DoCustomSetup(() =>
                {
                    expected = DataGenerator.GenerateString();
                })
                .ExecuteTest(() =>
                {
                    var attribute = new JoinAttribute(JoinType.Left, typeof(object),
                        DataGenerator.GenerateString(), expected, childProperty: String.Empty);

                    Asserter.AssertEquality(expected, attribute.RightKey);
                    Asserter.AssertEquality(expected, attribute.ChildProperty);
                });
        }

        [Test]
        public void WillReturnChildPropertyIfChildPropertyPassedToConstructorHasValue()
        {
            var expected = String.Empty;

            TestRunner
                .DoCustomSetup(() =>
                {
                    expected = DataGenerator.GenerateString();
                })
                .ExecuteTest(() =>
                {
                    var attribute = new JoinAttribute(JoinType.Left, typeof(object),
                        DataGenerator.GenerateString(), DataGenerator.GenerateString(),
                        childProperty: expected);

                    Assert.AreNotEqual(expected, attribute.RightKey);
                    Asserter.AssertEquality(expected, attribute.ChildProperty);
                });
        }
    }
}
