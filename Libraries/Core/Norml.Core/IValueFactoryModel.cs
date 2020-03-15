using System;
using System.Collections.Generic;

namespace Norml.Core
{
    public interface IValueFactoryModel
    {
        IDictionary<string, Func<object>> ValueFactories { get; set; }
    }
}
