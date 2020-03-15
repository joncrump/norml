using System.IO;

namespace Norml.Core.Helpers
{
    public interface IStreamWrapper
    {
        Stream Value { get; }

        void WriteLine(string value = null);
        void Write(string value = null);
    }
}
