using System;

namespace Norml.Core.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CountMetadataAttribute : Attribute
    {
        public CountMetadataAttribute(string fieldName, string fieldAlias)
        {
            FieldName = fieldName;
            FieldAlias = fieldAlias;
        }

        public string FieldName { get; private set; }
        public string FieldAlias { get; private set; }
    }
}