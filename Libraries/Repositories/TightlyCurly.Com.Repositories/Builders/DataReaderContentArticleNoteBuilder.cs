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
    public class DataReaderContentArticleNoteBuilder : ValueFactoryBuilderBase, 
        IBuilder<IDataReader, IContentArticleNote>
    {
        public DataReaderContentArticleNoteBuilder(IValueFactory valueFactory) : base(valueFactory)
        {
        }

        public IContentArticleNote Build(IDataReader dataSource)
        {
            Guard.EnsureIsNotNull("dataSource", dataSource);

            var model = new ContentArticleNoteDataModel
                {
                    Id = dataSource.Get<Guid>(Columns.ContentArticleNoteId),
                    EnteredDate = dataSource.Get<DateTime>(Columns.EnteredDate),
                    UpdatedDate = dataSource.Get<DateTime>(Columns.UpdatedDate),
                    Note = dataSource.Get<string>(Columns.Note)
                };

            AddValueFactory(model, LoaderKeys.GetEnteredByByContentArticleNote, 
                new ParameterInfo(typeof(IContentArticleNote), model));

            return model;
        }
    }
}
