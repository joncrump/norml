﻿using System;

namespace Norml.Common.Helpers
{
    public interface ILogger
    {
        void Trace(string message, Exception exception = null);
        void Debug(string message, Exception exception = null);
        void Info(string message, Exception exception = null);
        void Warn(string message, Exception exception = null);
        void Error(string message, Exception exception = null);
        void Fatal(string message, Exception exception = null);
    }
}
