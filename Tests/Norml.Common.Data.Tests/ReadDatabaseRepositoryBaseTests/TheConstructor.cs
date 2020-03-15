﻿using NUnit.Framework;
using Norml.Tests.Common.Base;

namespace Norml.Common.Data.Tests.ReadDatabaseRepositoryBaseTests
{
    [TestFixture]
    public class TheConstructor : TestBase
    {
        [Test]
        public void WillPassConstructorTests()
        {
            DoConstructorTests<TestableReadDatabaseRepository>();
        }
    }
}
