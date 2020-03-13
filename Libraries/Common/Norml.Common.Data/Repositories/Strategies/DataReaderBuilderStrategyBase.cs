namespace Norml.Common.Data.Repositories.Strategies
{
    public abstract class DataReaderBuilderStrategyBase
    {
        protected IDataReaderBuilder DataReaderBuilder { get; private set; }

        protected DataReaderBuilderStrategyBase(IDataReaderBuilder dataReaderBuilder)
        {
            DataReaderBuilder = Guard.ThrowIfNull("dataReaderBuilder", dataReaderBuilder);
        }
    }
}
