using System;
using System.Data;
using TightlyCurly.Com.Common;
using TightlyCurly.Com.Common.Data;
using TightlyCurly.Com.Common.Models;
using TightlyCurly.Com.Repositories.Constants;
using TightlyCurly.Com.Repositories.Models;
using TightlyCurly.Com.Common.Extensions;

namespace TightlyCurly.Com.Repositories.Builders
{
    public class DataReaderAddressBuilder : ValueFactoryBuilderBase, IBuilder<IDataReader, IAddress>
    {
        public DataReaderAddressBuilder(IValueFactory valueFactory) : base(valueFactory)
        {
        }

        public IAddress Build(IDataReader dataSource)
        {
            Guard.EnsureIsNotNull("dataSource", dataSource);

            var model = new AddressDataModel
                {
                    Id = dataSource.Get<Guid>(Columns.AddressId),
                    EnteredDate = dataSource.Get<DateTime>(Columns.EnteredDate),
                    UpdatedDate = dataSource.Get<DateTime>(Columns.UpdatedDate),
                    Line1 = dataSource.Get<string>(Columns.Line1),
                    Line2 = dataSource.Get<string>(Columns.Line2),
                    City = dataSource.Get<string>(Columns.City),
                    StateProvinceId = dataSource.Get<Guid>(Columns.StateProvinceId),
                    PostalCode = dataSource.Get<string>(Columns.PostalCode)
                };

            AddValueFactory(model, LoaderKeys.GetStateProvinceByAddress, 
                new ParameterInfo(typeof(IAddress), model));
            AddValueFactory(model, LoaderKeys.GetCountryByAddress, 
                new ParameterInfo(typeof(IAddress), model));

            return model;
        }
    }
}
