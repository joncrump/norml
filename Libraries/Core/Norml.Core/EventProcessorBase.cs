﻿using System;
using Norml.Common.Actions;

namespace Norml.Common
{
    public abstract class EventProcessorBase
    {
        public event ProcessEventHandler ProcessEventOccurred;

        protected void OnProcessEvent(EventLevel eventLevel, string message)
        {
            if (ProcessEventOccurred != null)
            {
                ProcessEventOccurred(this, new ProcessEventArgs(DateTime.Now, message, eventLevel));
            }
        }

        protected void HandleProcessEventOccurred(object sender, ProcessEventArgs eventArgs)
        {
            if (ProcessEventOccurred != null)
            {
                ProcessEventOccurred(sender, eventArgs);
            }
        }
    }
}
