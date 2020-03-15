using System.Data;

namespace Norml.Core.Data
{
    public interface IDataReaderBuilder
    {
        TItem Build<TItem>(IDataReader dataSource, string prefix = null, bool loadValueFactories = true)
            where TItem : class, new();
    }
}