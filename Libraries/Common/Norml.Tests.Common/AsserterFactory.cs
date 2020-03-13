using System;
using Norml.Tests.Common.Helpers;

namespace Norml.Tests.Common
{
    public static class AsserterFactory
    {
        public static IAssertAdapter GetAssertAdapter(UnitTestFrameworkType frameworkType)
        {
            switch (frameworkType)
            {
                case UnitTestFrameworkType.Nunit:
                    return new NUnitAssertAdapter();
                default:
                    throw new NotSupportedException($"The framework type {frameworkType} is not supported.");
            }
        }
    }
}