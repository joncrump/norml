using System;

namespace Norml.Core.Helpers
{
    public class EnvironmentHelper : IEnvironmentHelper
    {
        public string GetMachineName()
        {
            return Environment.MachineName;
        }

        public string GetUsername()
        {
            return Environment.UserName;
        }
    }
}
