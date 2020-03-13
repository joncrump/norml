using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Norml.Common.Data
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public IDictionary<string, string> ConnectionStrings
        {
            get 
            {
                return ConfigurationManager.ConnectionStrings
                    .Cast<ConnectionStringSettings>()
                    .ToDictionary(c => c.Name, c => c.ConnectionString);
            }
        }
    }
}
