using System;

namespace Plum.Tasks
{
    public delegate void TaskGoEventHandler(object sender, TaskGoEventArgs e);

    [Serializable]
    public class TaskGoEventArgs : EventArgs
    {
        #region Properties

        public Task Instance { get; private set; }

        #endregion Properties

        #region Ctor

        public TaskGoEventArgs(Task task)
        {
            Instance = task;
        }

        #endregion Ctor
    }
}