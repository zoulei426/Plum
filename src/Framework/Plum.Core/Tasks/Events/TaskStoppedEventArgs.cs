using System;

namespace Plum.Tasks
{
    public delegate void TaskStoppedEventHandler(object sender, TaskStoppedEventArgs e);

    [Serializable]
    public class TaskStoppedEventArgs : EventArgs
    {
        #region Properties

        #endregion Properties

        #region Ctor

        public TaskStoppedEventArgs()
        {
        }

        #endregion Ctor
    }
}