using System;

namespace Norml.Common.Data.Tests.EntityModelDatabaseRepositoryBaseTests
{
    public class TestModel : ITestModel
    {
        public Guid Id { get; set; }
        public DateTime EnteredDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
