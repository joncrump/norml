using System;

namespace Norml.Common.Data.Mappings
{
    public interface IObjectMapperFactory
    {
        IDataMapper GetMapper(MappingKind mappingKind);
    }
}
