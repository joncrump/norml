namespace Norml.Core.Data.Mappings
{
    public interface IObjectMapperFactory
    {
        IDataMapper GetMapper(MappingKind mappingKind);
    }
}
