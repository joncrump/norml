using System;
using Norml.Core.Data.Attributes;
using Norml.Core.Tests.Common.Base;
using NUnit.Framework;

namespace Norml.Core.Data.Tests.Attributes.JoinAttributeTests
{
    [TestFixture]
    public class TheParentPropertyProperty : TestBase
    {
        [Test]
        public void WillReturnLeftKeyIfParentPropertyPassedToConstructorIsNull()
        {
            var expected = DataGenerator.GenerateString();
            var attribute = new JoinAttribute(JoinType.Left, typeof(object),
                expected, DataGenerator.GenerateString());

            Assert.AreEqual(expected, attribute.LeftKey);
            Assert.AreEqual(expected, attribute.ParentProperty);
        }

        [Test]
        public void WillReturnLeftKeyIfParentPropertyPassedToConstructorIsEmpty()
        {
            var expected = DataGenerator.GenerateString();
            var attribute = new JoinAttribute(JoinType.Left, typeof(object),
                expected, DataGenerator.GenerateString(), parentProperty:String.Empty);

            Asserter.AssertEquality(expected, attribute.LeftKey);
            Asserter.AssertEquality(expected, attribute.ParentProperty);
        }

        [Test]
        public void WillReturnParentPropertyIfParentPropertyPassedToConstructorHasValue()
        {
            var expected  = DataGenerator.GenerateString();

            var attribute = new JoinAttribute(JoinType.Left, typeof(object),
                DataGenerator.GenerateString(), DataGenerator.GenerateString(),
                parentProperty: expected);

            Assert.AreNotEqual(expected, attribute.LeftKey);
            Asserter.AssertEquality(expected, attribute.ParentProperty);
        }
    }
}