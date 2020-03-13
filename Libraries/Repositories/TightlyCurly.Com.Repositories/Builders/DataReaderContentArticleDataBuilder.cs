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
    public class DataReaderContentArticleDataBuilder : ValueFactoryBuilderBase, 
        IBuilder<IDataReader, IContentArticleData>
    {
        public DataReaderContentArticleDataBuilder(IValueFactory valueFactory) 
            : base(valueFactory)
        {
        }

        public IContentArticleData Build(IDataReader dataSource)
        {
            Guard.EnsureIsNotNull("dataSource", dataSource);

            var model = new ContentArticleDataDataModel
                {
                    Id = dataSource.Get<Guid>(Columns.ContentArticleDataId),
                    EnteredDate = dataSource.Get<DateTime>(Columns.EnteredDate),
                    UpdatedDate = dataSource.Get<DateTime>(Columns.UpdatedDate),
                    Description = dataSource.Get<string>(Columns.Description),
                    Text = dataSource.Get<string>(Columns.Text),
                    LocaleId = dataSource.Get<Guid>(Columns.LocaleId)
                };

            AddValueFactory(model, LoaderKeys.GetNotesByContentArticle, 
                new ParameterInfo(typeof(IContentArticleData), model));
            AddValueFactory(model, LoaderKeys.GetRevisionsByContentArticleData, 
                new ParameterInfo(typeof(IContentArticleData), model));
            AddValueFactory(model, LoaderKeys.GetContentItemByContentArticleData, 
                new ParameterInfo(typeof(IContentArticleData), model));
            AddValueFactory(model, LoaderKeys.GetMetaKeywordsByContentArticleData, 
                new ParameterInfo(typeof(IContentArticleData), model));
            AddValueFactory(model, LoaderKeys.GetDataByContentArticleData, 
                new ParameterInfo(typeof(IContentArticleData), model));
            AddValueFactory(model, LoaderKeys.GetLocaleByContentArticleData, 
                new ParameterInfo(typeof(IContentArticleData), model));

            return model;
        }
    }
}
