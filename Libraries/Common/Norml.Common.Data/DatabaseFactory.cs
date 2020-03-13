﻿namespace Norml.Common.Data
{
    public class DatabaseFactory : IDatabaseFactory
    {
        private readonly IResolver _resolver;

        public DatabaseFactory(IResolver resolver)
        {
            _resolver = Guard.ThrowIfNull("resolver", resolver);
        }

        public IDatabaseWrapper GetDatabase(string databaseName)
        {
            Guard.ThrowIfNullOrEmpty("databaseName", databaseName);

            return _resolver.Resolve<IDatabaseWrapper>(databaseName);
        }
    }
}
