using System;
using System.Data;
using TightlyCurly.Com.Common.Data;
using TightlyCurly.Com.Common.Models;

namespace TightlyCurly.Com.Repositories.Builders
{
    public class DataReaderContentArticleRevisionBuilder : ValueFactoryBuilderBase, 
        IBuilder<IDataReader, IContentArticleRevision>
    {
        public DataReaderContentArticleRevisionBuilder(IValueFactory valueFactory) 
            : base(valueFactory)
        {
        }

        public IContentArticleRevision Build(IDataReader dataSource)
        {
            throw new NotImplementedException();
        }
    }
}
