using System.Data;

namespace Norml.Common.Data
{
    public interface IDataReaderBuilder
    {
        TItem Build<TItem>(IDataReader dataSource, string prefix = null, bool loadValueFactories = true)
            where TItem : class, new();
    }
}