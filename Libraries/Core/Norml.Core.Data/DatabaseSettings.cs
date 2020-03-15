using System.Collections.Generic;

namespace Norml.Core.Data
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
