namespace Norml.Core.Helpers
{
    public interface IMapper
    {
        TReturn Map<TReturn>(object value);
    }
}
