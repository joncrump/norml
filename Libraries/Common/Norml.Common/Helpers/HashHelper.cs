namespace Norml.Common.Helpers
{
    public class HashHelper : IHashHelper
    {
        public string GenerateHash(object value)
        {
            Guard.ThrowIfNull("value", value);

            if (value is IHashable)
            {
                return ((IHashable) value).Hash;
            }

            return value.GetHashCode().ToString();
        }
    }
}
