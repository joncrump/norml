using System;
using Norml.Core.Data.Mappings;
using Norml.Core.Tests.Common.Base;
using NUnit.Framework;

namespace Norml.Core.Data.Tests.Mappings.ObjectMappingFactoryTests
{
    [TestFixture]
    public class TheGetMappingForTypeMethod : MockTestBase<ObjectMapperFactory>
    {
        [Test]
        public void WillThrowArgumentNullExceptionIfObjectTypeIsNull()
        {
            throw new NotImplementedException();
            //Asserter.AssertException<ArgumentNullException>(() =>
            //        SystemUnderTest.GetMappingForType(null, It.IsAny<MappingKind>()))
            //    .AndVerifyHasParameter("objectType");
        }

        /*
         * public IMapping GetMappingForType(Type objectType, MappingKind mappingKind)
        {
            throw new NotImplementedException();
        }
         */
    }
}
