using System;

namespace Norml.Core.Data.Mappings
{
    public interface IDataMapper
    {
        TypeMapping GetMappingFor<TValue>();
        TypeMapping GetMappingForType(Type objectType);
    }
}