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
    public class DataReaderContentArticleBuilder : ValueFactoryBuilderBase, 
        IBuilder<IDataReader, IContentArticle>
    {
        public DataReaderContentArticleBuilder(IValueFactory valueFactory) : base(valueFactory)
        {
        }

        public IContentArticle Build(IDataReader dataSource)
        {
            Guard.EnsureIsNotNull("dataSource", dataSource);

            var model = new ContentArticleDataModel
                {
                    Id = dataSource.Get<Guid>(Columns.ContentArticleId),
                    MetaDescription = dataSource.Get<string>(Columns.MetaDescription),
                    EnteredDate = dataSource.Get<DateTime>(Columns.EnteredDate),
                    UpdatedDate = dataSource.Get<DateTime>(Columns.UpdatedDate),
                    IsActive = dataSource.Get<bool>(Columns.IsActive),
                    ContentItemId = dataSource.Get<Guid>(Columns.ContentItemId),
                    Description = dataSource.Get<string>(Columns.Description),
                    Text = dataSource.Get<string>(Columns.Text),
                    LocaleId = dataSource.Get<Guid>(Columns.LocaleId),
                };

            AddValueFactory(model, LoaderKeys.GetNotesByContentArticle, 
                new ParameterInfo(typeof(IContentArticle), model));
            AddValueFactory(model, LoaderKeys.GetRevisionsByContentArticle, 
                new ParameterInfo(typeof(IContentArticle), model));
            AddValueFactory(model, LoaderKeys.GetContentItemByContentArticle, 
                new ParameterInfo(typeof(IContentArticle), model));
            AddValueFactory(model, LoaderKeys.GetMetaKeywordsByContentArticle, 
                new ParameterInfo(typeof(IContentArticle), model));
            AddValueFactory(model, LoaderKeys.GetDataByContentArticle, 
                new ParameterInfo(typeof(IContentArticle), model));
            AddValueFactory(model, LoaderKeys.GetLocaleByContentArticle, 
                new ParameterInfo(typeof(IContentArticle), model));

            return model;
        }
    }
}
