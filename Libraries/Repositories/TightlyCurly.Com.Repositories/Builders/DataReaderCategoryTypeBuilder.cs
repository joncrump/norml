using System;
using System.Data;
using TightlyCurly.Com.Common;
using TightlyCurly.Com.Common.Data;
using TightlyCurly.Com.Common.Extensions;
using TightlyCurly.Com.Common.Models;
using TightlyCurly.Com.Repositories.Constants;
using TightlyCurly.Com.Repositories.Models;

namespace TightlyCurly.Com.Repositories.Builders
{
    public class DataReaderCategoryTypeBuilder : ValueFactoryBuilderBase, IBuilder<IDataReader, ICategoryType>
    {
        public DataReaderCategoryTypeBuilder(IValueFactory valueFactory) : base(valueFactory)
        {
        }

        public ICategoryType Build(IDataReader dataSource)
        {
            Guard.EnsureIsNotNull("dataSource", dataSource);

            var model = new CategoryTypeDataModel
                {
                    Id = dataSource.Get<Guid>(Columns.CategoryTypeId),
                    Name = dataSource.Get<string>(Columns.Name),
                    EnteredDate = dataSource.Get<DateTime>(Columns.EnteredDate),
                    UpdatedDate = dataSource.Get<DateTime>(Columns.UpdatedDate)
                };

            AddValueFactory(model, LoaderKeys.GetCategoryNamesByCategoryType, 
                new ParameterInfo(typeof(ICategoryType), model));

            return model;
        }
    }
}
