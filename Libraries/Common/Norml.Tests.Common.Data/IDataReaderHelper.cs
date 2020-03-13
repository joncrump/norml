using System.Collections.Generic;

namespace Norml.Tests.Common.Data
{
    public interface IDataReaderHelper
    {
        MockDataReader BuildMockDataReader<TModel>(IEnumerable<TModel> instances, 
            string prefix = null) where TModel : class, new();
    }
}
