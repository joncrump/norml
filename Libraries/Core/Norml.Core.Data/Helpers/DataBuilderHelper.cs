using System;
using System.Data;
using System.Linq;
using System.Xml;

namespace Norml.Common.Data.Helpers
{
    public class DataBuilderHelper : IDataBuilderHelper
    {
        public SqlDbType InferDatabaseType(Type type)
        {
            if (type == typeof(long))
            {
                return SqlDbType.BigInt;
            }

            if (type == typeof(bool))
            {
                return SqlDbType.Bit;
            }

            if (type == typeof(DateTime))
            {
                return SqlDbType.DateTime;
            }

            if (type == typeof(decimal))
            {
                return SqlDbType.Decimal;
            }

            if (type == typeof(float))
            {
                return SqlDbType.Float;
            }

            if (type == typeof(object))
            {
                return SqlDbType.Image;
            }

            if (type == typeof(int))
            {
                return SqlDbType.Int;
            }

            if (type == typeof(char))
            {
                return SqlDbType.NChar;
            }

            if (type == typeof(string))
            {
                return SqlDbType.NVarChar;
            }

            if (type == typeof(Guid))
            {
                return SqlDbType.UniqueIdentifier;
            }

            if (type == typeof(short))
            {
                return SqlDbType.SmallInt;
            }

            if (type == typeof(byte))
            {
                return SqlDbType.TinyInt;
            }

            if (type == typeof(DateTimeOffset))
            {
                return SqlDbType.DateTimeOffset;
            }

            throw new NotSupportedException($"The type {type} is not supported.");
        }

        public string GetParameterName(string name)
        {
            var firstLetter = char.ToLower(name[0]);

            if (name.Length == 1)
            {
                return $"@{firstLetter}";
            }

            var theRest = new[] {firstLetter}.Concat(name.Substring(1, name.Length - 1).ToCharArray()).ToArray();

            return $"@{new string(theRest)}";
        }
    }
}
