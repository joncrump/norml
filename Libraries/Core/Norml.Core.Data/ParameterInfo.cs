﻿using System;

namespace Norml.Core.Data
{
    public class ParameterInfo
    {
        public ParameterInfo()
        {
        }

        public ParameterInfo(Type type, object value)
        {
            Type = type;
            Value = value;
        }

        public Type Type { get; set; }
        public object Value { get; set; }
    }
}
