using System;

namespace Norml.Core.Data.Tests.EntityModelDatabaseRepositoryBaseTests
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
