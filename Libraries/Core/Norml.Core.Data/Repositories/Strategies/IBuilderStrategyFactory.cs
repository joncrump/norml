namespace Norml.Core.Data.Repositories.Strategies
{
    public interface IBuilderStrategyFactory
    {
        IBuilderStrategy GetStrategy(BuildMode buildMode);
    }
}
