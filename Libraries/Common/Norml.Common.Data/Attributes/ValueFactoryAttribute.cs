using System;

namespace Norml.Common.Data.Attributes
{
    public class ValueFactoryAttribute : Attribute
    {
        public ValueFactoryAttribute(string valueFactoryName)
        {
            ValueFactoryName = Guard.ThrowIfNullOrEmpty("valueFactoryName", valueFactoryName);
        }

        public string ValueFactoryName { get; private set; }
    }
}