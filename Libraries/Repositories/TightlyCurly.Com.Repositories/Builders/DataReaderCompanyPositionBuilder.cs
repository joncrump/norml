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
    public class DataReaderCompanyPositionBuilder : ValueFactoryBuilderBase, 
        IBuilder<IDataReader, ICompanyPosition>
    {
        private readonly IEnumParser _enumParser;

        public DataReaderCompanyPositionBuilder(IValueFactory valueFactory, IEnumParser enumParser) 
            : base(valueFactory)
        {
            _enumParser = Guard.EnsureIsNotNull("enumParser", enumParser);
        }

        public ICompanyPosition Build(IDataReader dataSource)
        {
            Guard.EnsureIsNotNull("dataSource", dataSource);

            var model = new CompanyPositionDataModel
                {
                    Id = dataSource.Get<Guid>(Columns.CompanyPositionId),
                    EnteredDate = dataSource.Get<DateTime>(Columns.EnteredDate),
                    UpdatedDate = dataSource.Get<DateTime>(Columns.UpdatedDate),
                    CompanyId = dataSource.Get<Guid?>(Columns.CompanyId),
                    Title = dataSource.Get<string>(Columns.Title),
                    StartDate = dataSource.Get<DateTime?>(Columns.StartDate),
                    EndDate = dataSource.Get<DateTime?>(Columns.EndDate),
                    IsActive = dataSource.Get<bool?>(Columns.IsActive),
                    Description = dataSource.Get<string>(Columns.Description),
                    Notes = dataSource.Get<string>(Columns.Notes),
                    Department = dataSource.Get<string>(Columns.Department),
                    PositionCategory = _enumParser.Parse<PositionCategory>(
                        dataSource.Get<int>(Columns.PositionCategory).ToString())
                };

            AddValueFactory(model, LoaderKeys.GetCompanyByCompanyPosition, 
                new ParameterInfo(typeof(ICompanyPosition), model));
            AddValueFactory(model, LoaderKeys.GetPersonByCompanyPosition, 
                new ParameterInfo(typeof(ICompanyPosition), model));

            return model;
        }
    }
}
