using System;
using System.Data;

namespace Norml.Core.Data.Helpers
{
    public interface IDataBuilderHelper
    {
        SqlDbType InferDatabaseType(Type type);
        string GetParameterName(string name);
    }
}
