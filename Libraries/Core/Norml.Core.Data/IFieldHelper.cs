using System.Collections.Generic;
using System.Data;

namespace Norml.Core.Data
{
    public interface IFieldHelper
    {
        //IDictionary<string, IDictionary<string, FieldParameterInfo>> 
            TableObjectMapping BuildFields<TValue>(
            IEnumerable<string> desiredFields = null, string tableName = null, TValue model = default(TValue), 
            bool ignoreIdentity = false, string alias = null, string instancePropertyName = null, MappingKind mappingKind = MappingKind.Attribute) where TValue : class;

        IEnumerable<IDbDataParameter> 
            ExtractParameters(TableObjectMapping tableFields,
            //IDictionary<string, IDictionary<string, FieldParameterInfo>> fields,
            bool ignoreIdentity);
    }
}
