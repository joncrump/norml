using System;
using System.Collections;
using System.Collections.Generic;

namespace Norml.Common
{
    public interface IValueFactoryModel
    {
        IDictionary<string, Func<object>> ValueFactories { get; set; }
    }
}
