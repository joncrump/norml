using System.Collections.Generic;
using System.Data;
using Norml.Core.Data.Attributes;

namespace Norml.Core.Data.Tests
{
    [Table("dbo.TestTable")]
    public class TestFixture
    {
        public TestFixture()
        {
            Values = new List<string>();
        }

        [FieldMetadata("TestFixtureId", SqlDbType.Int, "@id", true)]
        public int Id { get; set; }

        [FieldMetadata("SomeFoo", SqlDbType.NVarChar, "@fooParameter")]
        public string Foo { get; set; }
        
        
        [FieldMetadata("PioneerSquareBar", SqlDbType.NVarChar, "@itsFridayLetsGoToTheBar")]
        public string Bar { get; set; }

        public IEnumerable<string> Values { get; set; }
    }

    public class DummyClass
    {
        [FieldMetadata("Baz", SqlDbType.BigInt, "@baz")]
        public string Baz { get; set; }
    }

    [Table("dbo.SomeTable")]
    public class DummyClass1
    {
        public string Baz { get; set; }
    }
}
