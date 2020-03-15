using System;

namespace Norml.Core
{
    public interface IModel 
    {
        Guid Id { get; set; }
        DateTime EnteredDate { get; set; }
        DateTime UpdatedDate { get; set; }
    }
}