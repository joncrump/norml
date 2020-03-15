namespace Norml.Core.Data.Tests.Helpers.DataBuilderHelperTests
{
    [TestFixture]
    public class TheGetParameterNameMethod : MockTestBase<DataBuilderHelper>
    {
        [Test]
        public void WillReturnParameterBasedOnName()
        {
            var parameterName = "SomeProperty";
            var expected = "@someProperty";

            var actual = SystemUnderTest.GetParameterName(parameterName);

            Asserter.AssertEquality(expected, actual);
        }
    }
}
