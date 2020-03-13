using System;
using System.Data;
using TightlyCurly.Com.Common;
using TightlyCurly.Com.Common.Data;
using TightlyCurly.Com.Common.Extensions;
using TightlyCurly.Com.Common.Helpers;
using TightlyCurly.Com.Common.Models;
using TightlyCurly.Com.Repositories.Constants;
using TightlyCurly.Com.Repositories.Models;

namespace TightlyCurly.Com.Repositories.Builders
{
    public class DataReaderCompanyRelationshipNodeBuilder : ValueFactoryBuilderBase, IBuilder<IDataReader, ICompanyRelationshipNode>
    {
        private readonly IEnumParser _enumParser;

        public DataReaderCompanyRelationshipNodeBuilder(IValueFactory valueFactory, 
            IEnumParser enumParser) : base(valueFactory)
        {
            _enumParser = Guard.EnsureIsNotNull("enumParser", enumParser);
        }

        public ICompanyRelationshipNode Build(IDataReader dataSource)
        {
            Guard.EnsureIsNotNull("dataSource", dataSource);

            var model = new CompanyRelationshipNodeDataModel
                {
                    Id = dataSource.Get<Guid>(Columns.RelationshipNodeId),
                    EnteredDate = dataSource.Get<DateTime>(Columns.EnteredDate),
                    UpdatedDate = dataSource.Get<DateTime>(Columns.UpdatedDate),
                    StartDate = dataSource.Get<DateTime?>(Columns.StartDate),
                    EndDate = dataSource.Get<DateTime?>(Columns.EndDate),
                    Name = dataSource.Get<string>(Columns.Name),
                    Notes = dataSource.Get<string>(Columns.Notes),
                    RelationshipType = _enumParser.Parse<RelationshipType>(
                        dataSource.Get<int>(Columns.RelationshipType).ToString())
                };

            AddValueFactory(model, LoaderKeys.GetParentsByRelationshipNode, 
                new ParameterInfo(typeof(ICompanyRelationshipNode), model));
            AddValueFactory(model, LoaderKeys.GetChildrenByRelationshipNode, 
                new ParameterInfo(typeof(ICompanyRelationshipNode), model));

            return model;
        }
    }
}
