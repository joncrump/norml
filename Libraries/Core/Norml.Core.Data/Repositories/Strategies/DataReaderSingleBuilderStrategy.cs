using System.Collections.Generic;
using System.Data;

namespace Norml.Common.Data.Repositories.Strategies
{
    public class DataReaderSingleBuilderStrategy : DataReaderBuilderStrategyBase, IBuilderStrategy
    {
        public DataReaderSingleBuilderStrategy(IDataReaderBuilder dataReaderBuilder) : base(dataReaderBuilder)
        {
        }

        public IEnumerable<TValue> BuildItems<TValue>(dynamic parameters, IDataReader dataSource)
            where TValue : class, new()
        {
            Guard.ThrowIfNull<string>("parameters", parameters);
            Guard.ThrowIfNull("dataSource", dataSource);

            var items = new List<TValue>();

            while (dataSource.Read())
            {
                items.Add(DataReaderBuilder.Build<TValue>(dataSource));
            }

            return items;
        }
    }
}
