using System;

namespace Plum.Tasks
{
    public interface ITask
    {
        #region Properties

        TaskArgument Argument { get; set; }
        bool IsBusy { get; }
        bool IsValidateMode { get; }
        Guid Id { get; }

        string Name { get; set; }
        string Description { get; set; }

        #endregion Properties

        #region Events

        event TaskGoEventHandler Go;

        event TaskStartedEventHandler Started;

        event TaskEndedEventHandler Ended;

        event TaskCompletedEventHandler Completed;

        event TaskStoppedEventHandler Stopped;

        event TaskTerminatedEventHandler Terminated;

        event TaskProgressChangedEventHandler ProgressChanged;

        event TaskAlertEventHandler Alert;

        #endregion Events

        #region Methods

        void Start(bool validate = false);

        void Stop();

        void Reset();

        void ReportProgress(TaskProgressChangedEventArgs e);

        void ReportAlert(TaskAlertEventArgs e);

        #endregion Methods
    }
}