using System;
using NUnit.Framework;
using Norml.Tests.Common.Base;

namespace Norml.Common.Data.Tests.EntityModelDatabaseRepositoryBaseTests
{
    [TestFixture]
    public class TheDeleteMethod : MockTestBase<TestableEntityDatabaseRepository>
    {
        [Test]
        public void WillThrowArgumentNullExceptionIfModelIsNull()
        {
            Asserter
                .AssertException<ArgumentNullException>(
                    () => SystemUnderTest.Delete(null))
                .AndVerifyHasParameter("model");
        }
    }
}
