using System;

namespace Norml.Core.Data.Mappings
{
    public class ObjectMapperFactory : IObjectMapperFactory
    {
        private readonly IResolver _resolver;

        public ObjectMapperFactory(IResolver resolver)
        {
            _resolver = resolver.ThrowIfNull(nameof(resolver));
        }

        public IDataMapper GetMapper(MappingKind mappingKind)
        {
            switch (mappingKind)
            {
                case MappingKind.Attribute:
                    return _resolver.Resolve<IDataMapper>(MappingKind.Attribute.ToString());
                default:
                    throw new InvalidOperationException($"Mapping Kind {mappingKind} is not supported");
            }
        }
    }
}
