﻿using System;

namespace Norml.Core.Data.Tests.EntityModelDatabaseRepositoryBaseTests
{
    public class TestModel : ITestModel
    {
        public Guid Id { get; set; }
        public DateTime EnteredDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
