namespace Norml.Common.Data.Repositories.Strategies
{
    public class DataReaderBuilderStrategyFactory : IBuilderStrategyFactory
    {
        private readonly IResolver _resolver;

        public DataReaderBuilderStrategyFactory(IResolver resolver)
        {
            _resolver = Guard.ThrowIfNull("resolver", resolver);
        }

        public IBuilderStrategy GetStrategy(BuildMode buildMode)
        {
            var strategy = _resolver.Resolve<IBuilderStrategy>(buildMode.ToString());

            return strategy;
        }
    }
}
