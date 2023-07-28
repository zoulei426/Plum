using System;

namespace Plum.Tasks
{
    public delegate void TaskEndedEventHandler(object sender, TaskEndedEventArgs e);

    [Serializable]
    public class TaskEndedEventArgs : EventArgs
    {
        #region Properties

        #endregion Properties

        #region Ctor

        public TaskEndedEventArgs()
        {
        }

        #endregion Ctor
    }
}