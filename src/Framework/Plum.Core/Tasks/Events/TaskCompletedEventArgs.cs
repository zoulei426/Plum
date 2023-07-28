﻿using System;

namespace Plum.Tasks
{
    public delegate void TaskCompletedEventHandler(object sender, TaskCompletedEventArgs e);

    [Serializable]
    public class TaskCompletedEventArgs : EventArgs
    {
        #region Properties

        public Task Instance { get; private set; }
        public object Result { get; set; }
        public TaskArgument Argument { get; set; }
        public TimeSpan Elapsed { get; set; }

        #endregion Properties

        #region Ctor

        public TaskCompletedEventArgs(Task task)
        {
            Instance = task;
        }

        #endregion Ctor
    }
}