namespace Norml.Tests.Common.Helpers.Strategies
{
    public interface IAsserterStrategyFactory
    {
        IAssertHelper Asserter { get; set; }
        IAsserterStrategy GetStrategy<TValue>();
    }
}
