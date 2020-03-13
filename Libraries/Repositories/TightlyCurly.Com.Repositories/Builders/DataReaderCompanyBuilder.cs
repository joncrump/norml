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
    public class DataReaderCompanyBuilder : ValueFactoryBuilderBase, IBuilder<IDataReader, ICompany>
    {
        public DataReaderCompanyBuilder(IValueFactory valueFactory)
            : base(valueFactory)
        {
        }

        public ICompany Build(IDataReader dataSource)
        {
            Guard.EnsureIsNotNull("dataSource", dataSource);

            var model = new CompanyDataModel
                {
                    Id = dataSource.Get<Guid>(Columns.CompanyId),
                    EnteredDate = dataSource.Get<DateTime>(Columns.EnteredDate),
                    UpdatedDate = dataSource.Get<DateTime>(Columns.UpdatedDate),
                    Name = dataSource.Get<string>(Columns.Name),
                    ParentCompanyId = dataSource.Get<Guid?>(Columns.ParentCompanyId),
                };

            AddValueFactory(model, LoaderKeys.GetCountriesByCompany,
                new ParameterInfo(typeof(ICompany), model));
            AddValueFactory(model, LoaderKeys.GetContactInfosByCompany,
                new ParameterInfo(typeof(ICompany), model));
            AddValueFactory(model, LoaderKeys.GetParentCompanyByCompany,
                new ParameterInfo(typeof(ICompany), model));
            AddValueFactory(model, LoaderKeys.GetChildCompaniesByCompany,
                new ParameterInfo(typeof(ICompany), model));
            AddValueFactory(model, LoaderKeys.GetRelationshipsByCompany,
                new ParameterInfo(typeof(ICompany), model));
            AddValueFactory(model, LoaderKeys.GetCompanyTypesByCompany, 
                new ParameterInfo(typeof(ICompany), model));

            return model;
        }
    }
}
