using System;

namespace Norml.Common.Data.Mappings
{
    public interface IDataMapper
    {
        TypeMapping GetMappingFor<TValue>();
        TypeMapping GetMappingForType(Type objectType);
    }
}