using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Norml.Tests.Common.Base;
using Norml.Tests.Common.Helpers;

namespace Norml.Tests.Common.Data
{
    public abstract class MockDataItemTestBase<TItemUnderTest> : MockTestBase<TItemUnderTest>
        where TItemUnderTest : class
    {
        protected IDataReaderHelper DataReaderHelper;

        protected MockDataItemTestBase(IAssertAdapter assertAdapter, 
            IDataReaderHelper dataReaderHelper) : base(assertAdapter)
        {
            DataReaderHelper = dataReaderHelper;
        }

        protected MockDataItemTestBase(IDataGenerator dataGenerator, IAssertAdapter assertAdapter, 
            IDataReaderHelper dataReaderHelper) : base(dataGenerator, assertAdapter)
        {
            DataReaderHelper = dataReaderHelper;
        }

        protected MockDataItemTestBase(IDataGenerator dataGenerator, IObjectCreator objectCreator, IAssertAdapter assertAdapter, 
            IDataReaderHelper dataReaderHelper) : base(dataGenerator, objectCreator, assertAdapter)
        {
            DataReaderHelper = dataReaderHelper;
        }

        protected MockDataItemTestBase(IObjectCreator objectCreator, IAssertAdapter assertAdapter, IDataReaderHelper dataReaderHelper) : base(objectCreator, assertAdapter)
        {
            DataReaderHelper = dataReaderHelper;
        }
    }
}
