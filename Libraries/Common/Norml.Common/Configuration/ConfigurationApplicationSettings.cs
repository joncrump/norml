using System;
using System.Configuration;

namespace Norml.Common.Configuration
{
    public class ConfigurationApplicationSettings : IApplicationSettings
    {
        public Guid ApplicationId
        {
            get
            {
                return new Guid(ConfigurationManager.AppSettings["applicationId"]);
            }
        }
    }
}
