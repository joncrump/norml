﻿namespace Norml.Core.Actions
{
    public delegate void ProcessEventHandler(object sender, ProcessEventArgs eventArgs);

    public interface IActionHandler
    {
        event ProcessEventHandler ProcessEventOccurred;
        void Execute(object[] paramters = null);
    }
}
