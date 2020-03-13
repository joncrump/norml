namespace Norml.Common.Helpers
{
    public interface IMapper
    {
        TReturn Map<TReturn>(object value);
    }
}
