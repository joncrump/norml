using System;

namespace Norml.Tests.Common
{
    public class MappedType
    {
        public MappedType(Type interfaceType, Type concreteType)
        {
            InterfaceType = interfaceType;
            ConcreteType = concreteType;
        }

        public Type InterfaceType { get; private set; }
        public Type ConcreteType { get; private set; }
    }
}
