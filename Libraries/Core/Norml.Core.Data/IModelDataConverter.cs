using System.Collections.Generic;

namespace Norml.Core.Data
{
    public interface IModelDataConverter
    {
        IDatatableObjectMapping ConvertToDataTable<TModel>(IEnumerable<TModel> models);
    }
}
