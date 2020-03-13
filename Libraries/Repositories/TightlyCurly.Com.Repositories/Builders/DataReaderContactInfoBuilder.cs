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
    public class DataReaderContactInfoBuilder : ValueFactoryBuilderBase, 
        IBuilder<IDataReader, IContactInfo>
    {
        public DataReaderContactInfoBuilder(IValueFactory valueFactory) : base(valueFactory)
        {
        }

        public IContactInfo Build(IDataReader dataSource)
        {
            Guard.EnsureIsNotNull("dataSource", dataSource);

            var model = new ContactInfoDataModel
                {
                    Id = dataSource.Get<Guid>(Columns.ContactInfoId),
                    EnteredDate = dataSource.Get<DateTime>(Columns.EnteredDate),
                    UpdatedDate = dataSource.Get<DateTime>(Columns.UpdatedDate),
                    Notes = dataSource.Get<string>(Columns.Notes),
                    Description = dataSource.Get<string>(Columns.Description),
                    Title = dataSource.Get<string>(Columns.Title)
                };

            AddValueFactory(model, LoaderKeys.GetPeopleByContactInfo, 
                new ParameterInfo(typeof(IContactInfo), model));

            AddValueFactory(model, LoaderKeys.GetAddressesByContactInfo, 
                new ParameterInfo(typeof(IContactInfo), model));
        
            AddValueFactory(model, LoaderKeys.GetPhoneNumbersByContactInfo, 
                new ParameterInfo(typeof(IContactInfo), model));

            AddValueFactory(model, LoaderKeys.GetEmailsByContactInfo, 
                new ParameterInfo(typeof(IContactInfo), model));

            AddValueFactory(model, LoaderKeys.GetSocialMediaByContactInfo, 
                new ParameterInfo(typeof(IContactInfo), model));

            AddValueFactory(model, LoaderKeys.GetCompanyPositionsByContactInfo, 
                new ParameterInfo(typeof(IContactInfo), model));

            return model;
        }
    }
}
