﻿using System;
using Moq;
using Norml.Core.Data.Attributes;
using Norml.Core.Exceptions;
using Norml.Core.Tests.Common.Base;
using NUnit.Framework;

namespace Norml.Core.Data.Tests.Attributes.JoinAttributeTests
{
    [TestFixture]
    public class TheConstructor : TestBase
    {
        [Test]
        public void WillThrowArgumentInvalidExceptionIfJoinTypeIsNone()
        {
            Asserter
                .AssertException<ArgumentInvalidException>(
                    () => new JoinAttribute(JoinType.None, It.IsAny<Type>(), It.IsAny<string>(),
                        It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                        It.IsAny<string>(), It.IsAny<JoinType>(), It.IsAny<string>(),
                        It.IsAny<string>()))
                .AndVerifyHasParameter("relationshipType");
        }

        [Test]
        public void WillThrowArgumentNullExceptionIfJoinedTypeIsNull()
        {
            Asserter
                .AssertException<ArgumentNullException>(
                    () => new JoinAttribute(JoinType.Inner, It.IsAny<Type>(), It.IsAny<string>(),
                        It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                        It.IsAny<string>(), It.IsAny<JoinType>(), It.IsAny<string>(),
                        It.IsAny<string>()))
                .AndVerifyHasParameter("joinedType");
        }

        [Test]
        public void WillThrowArgumentNullExceptionIfLeftKeyIsNull()
        {
            Asserter
                .AssertException<ArgumentNullException>(
                    () => new JoinAttribute(JoinType.Inner, typeof(object), null,
                        It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                        It.IsAny<string>(), It.IsAny<JoinType>(), It.IsAny<string>(),
                        It.IsAny<string>()))
                .AndVerifyHasParameter("leftKey");
        }

        [Test]
        public void WillThrowArgumentEmptyExceptionIfLeftKeyIsEmpty()
        {
            Asserter
                .AssertException<ArgumentEmptyException>(
                    () => new JoinAttribute(JoinType.Inner, typeof(object), String.Empty,
                        It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                        It.IsAny<string>(), It.IsAny<JoinType>(), It.IsAny<string>(),
                        It.IsAny<string>()))
                .AndVerifyHasParameter("leftKey");
        }

        [Test]
        public void WillThrowArgumentNullExceptionIfRightKeyIsNull()
        {
            Asserter
                .AssertException<ArgumentNullException>(
                    () => new JoinAttribute(JoinType.Inner, typeof(object), DataGenerator.GenerateString(),
                        null, It.IsAny<string>(), It.IsAny<string>(),
                        It.IsAny<string>(), It.IsAny<JoinType>(), It.IsAny<string>(),
                        It.IsAny<string>()))
                .AndVerifyHasParameter("rightKey");
        }

        [Test]
        public void WillThrowArgumentEmptyExceptionIfRightKeyIsEmpty()
        {
            Asserter
                .AssertException<ArgumentEmptyException>(
                    () => new JoinAttribute(JoinType.Inner, typeof(object), 
                        DataGenerator.GenerateEmailAddress(),
                        String.Empty, It.IsAny<string>(), It.IsAny<string>(),
                        It.IsAny<string>(), It.IsAny<JoinType>(), It.IsAny<string>(),
                        It.IsAny<string>()))
                .AndVerifyHasParameter("rightKey");
        }

        [Test]
        public void WillPopulateProperties()
        {
            var expectedJoinType = JoinType.None;
            Type expectedType = null;
            var expectedLeftKey = String.Empty;
            var expectedRightKey = String.Empty;
            var expectedJoinTable = String.Empty;
            var expectedJoinTableLeftKey = String.Empty;
            var expectedJoinTableRightKey = String.Empty;
            var expectedJoinTableJoinType = JoinType.None;
            var expectedParentProperty = String.Empty;
            var expectedChildProperty = String.Empty;

            expectedJoinType = JoinType.Left;
            expectedType = typeof(Object);
            expectedLeftKey = DataGenerator.GenerateString();
            expectedRightKey = DataGenerator.GenerateString();
            expectedJoinTable = DataGenerator.GenerateString();
            expectedJoinTableLeftKey = DataGenerator.GenerateString();
            expectedJoinTableRightKey = DataGenerator.GenerateString();
            expectedJoinTableJoinType = JoinType.Right;
            expectedParentProperty = DataGenerator.GenerateString();
            expectedChildProperty = DataGenerator.GenerateString();

            var join = new JoinAttribute(expectedJoinType, expectedType, expectedLeftKey,
                expectedRightKey, expectedJoinTable, expectedJoinTableLeftKey,
                expectedJoinTableRightKey, expectedJoinTableJoinType,
                expectedParentProperty, expectedChildProperty);

            Asserter.AssertEquality(expectedJoinType, join.JoinType);
            Asserter.AssertEquality(expectedType.ToString(), join.JoinedType.ToString());
            Asserter.AssertEquality(expectedLeftKey, expectedLeftKey);
            Asserter.AssertEquality(expectedRightKey, join.RightKey);
            Asserter.AssertEquality(expectedJoinTable, join.JoinTable);
            Asserter.AssertEquality(expectedJoinTableLeftKey, join.JoinTableLeftKey);
            Asserter.AssertEquality(expectedJoinTableRightKey, join.JoinTableRightKey);
            Asserter.AssertEquality(expectedJoinTableJoinType, join.JoinTableJoinType);
            Asserter.AssertEquality(expectedParentProperty, join.ParentProperty);
            Asserter.AssertEquality(expectedChildProperty, join.ChildProperty);
        }
    }
}