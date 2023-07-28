using System;

namespace Plum.Tasks
{
    public delegate void TaskStartedEventHandler(object sender, TaskStartedEventArgs e);

    [Serializable]
    public class TaskStartedEventArgs : EventArgs
    {
        #region Properties

        public Task Instance { get; private set; }

        #endregion Properties

        #region Ctor

        public TaskStartedEventArgs(Task task)
        {
            Instance = task;
        }

        #endregion Ctor
    }
}