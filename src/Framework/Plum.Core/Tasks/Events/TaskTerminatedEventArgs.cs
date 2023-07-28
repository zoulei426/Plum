﻿using System;

namespace Plum.Tasks
{
    public delegate void TaskTerminatedEventHandler(object sender, TaskTerminatedEventArgs e);

    [Serializable]
    public class TaskTerminatedEventArgs : EventArgs
    {
        #region Properties

        public Exception Exception { get; set; }

        #endregion Properties

        #region Ctor

        public TaskTerminatedEventArgs()
        {
        }

        #endregion Ctor
    }
}