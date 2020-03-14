using System;
using System.Data;

namespace Norml.Common.Data.Helpers
{
    public interface IDataBuilderHelper
    {
        SqlDbType InferDatabaseType(Type type);
        string GetParameterName(string name);
    }
}
