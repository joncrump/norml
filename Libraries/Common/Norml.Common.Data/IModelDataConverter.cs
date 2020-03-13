using System.Collections.Generic;

namespace Norml.Common.Data
{
    public interface IModelDataConverter
    {
        IDatatableObjectMapping ConvertToDataTable<TModel>(IEnumerable<TModel> models);
    }
}
