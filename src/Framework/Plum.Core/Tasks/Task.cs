using Plum.Object;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Plum.Tasks
{
    public class Task : BindableObject, ITask
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool ReportTime { get; set; }

        public TaskArgument Argument { get; set; }

        public bool AutoHandleException { get; set; }

        public Guid Id { get; set; }

        public bool IsBusy { get; set; }

        public bool IsStopPending { get; set; }

        public bool IsValidateMode { get; set; }

        public bool CanOpenResult { get; set; }

        #region Fields

        private string _Name;
        private string _Description;
        private TaskArgument _Argument;
        private bool _CanOpenResult;
        private int threadIDMain;
        private Stopwatch sw;

        #endregion Fields

        #region Events

        public event TaskGoEventHandler Go;

        public event TaskStartedEventHandler Started;

        public event TaskEndedEventHandler Ended;

        public event TaskCompletedEventHandler Completed;

        public event TaskStoppedEventHandler Stopped;

        public event TaskTerminatedEventHandler Terminated;

        public event TaskProgressChangedEventHandler ProgressChanged;

        public event TaskAlertEventHandler Alert;

        public event PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangedEventHandler ArgumentPropertyChanged;

        #endregion Events

        #region Methods

        #region Methods - Static

        public static ITask Create(
            Action<TaskGoEventArgs> go,
            Action<TaskAlertEventArgs> alert = null,
            Action<TaskProgressChangedEventArgs> progressChanged = null,
            Action<TaskStartedEventArgs> started = null,
            Action<TaskEndedEventArgs> ended = null,
            Action<TaskCompletedEventArgs> comleted = null,
            Action<TaskStoppedEventArgs> stopped = null,
            Action<TaskTerminatedEventArgs> terminated = null)
        {
            return Create<Task>(go, alert, progressChanged, started, ended, comleted, stopped, terminated);
        }

        public static ITask Create<T>(
            Action<TaskGoEventArgs> go,
            Action<TaskAlertEventArgs> alert = null,
            Action<TaskProgressChangedEventArgs> progressChanged = null,
            Action<TaskStartedEventArgs> started = null,
            Action<TaskEndedEventArgs> Ended = null,
            Action<TaskCompletedEventArgs> comleted = null,
            Action<TaskStoppedEventArgs> stopped = null,
            Action<TaskTerminatedEventArgs> terminated = null) where T : Task, new()
        {
            var task = new T() { Argument = new TaskArgument() };
            task.Go += (s, e) => go(e);
            task.Alert += (s, e) => { alert?.Invoke(e); };
            task.ProgressChanged += (s, e) => { progressChanged?.Invoke(e); };
            task.Started += (s, e) => { started?.Invoke(e); };
            task.Ended += (s, e) => { Ended?.Invoke(e); };
            task.Completed += (s, e) => { comleted?.Invoke(e); };
            task.Stopped += (s, e) => { stopped?.Invoke(e); };
            task.Terminated += (s, e) => { terminated?.Invoke(e); };

            return task;
        }

        #endregion Methods - Static

        #region Methods - Public

        public virtual void Start(bool validate = false)
        {
            lock (this)
                StartInner(validate);
        }

        private void StartInner(bool validate = false)
        {
            try
            {
                sw = Stopwatch.StartNew();
                threadIDMain = System.Threading.Thread.CurrentThread.ManagedThreadId;
                IsBusy = true;
                //IsStopPending = false;
                IsValidateMode = validate;
                CanOpenResult = false;

                ReportStarted();
                ReportGo();

                sw.Stop();
                ReportCompleted(sw);
            }
            catch (Exception ex)
            {
                IsStopPending = false;

                if (!(ex is TaskStopException))
                    ReportTerminated(ex);
                else
                    ReportStopped();
            }
            finally
            {
                ReportEnded();

                IsBusy = false;
                IsStopPending = false;
                IsValidateMode = false;
            }
        }

        public virtual void Stop()
        {
            IsStopPending = true;
        }

        public virtual void OpenResult()
        {
        }

        public virtual void Reset()
        {
            IsStopPending = false;
            CanOpenResult = false;
        }

        #endregion Methods - Public

        #region Methods - Override

        protected virtual void OnStarted()
        {
        }

        protected virtual void OnEnded()
        {
        }

        protected virtual void OnGo()
        {
        }

        protected virtual void OnValidate()
        {
        }

        protected virtual void OnStopped()
        {
        }

        protected virtual void OnCompleted(TaskCompletedEventArgs e)
        {
        }

        protected virtual void OnTerminate(Exception ex)
        {
        }

        protected virtual void OnProgressChanged(TaskProgressChangedEventArgs e)
        {
        }

        protected virtual void OnAlert(TaskAlertEventArgs e)
        {
        }

        protected virtual object OnGettingResult()
        {
            return Argument?.UserState;
        }

        protected virtual void OnArgumentPropertyChanged(PropertyChangedEventArgs e)
        {
        }

        protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }

        #endregion Methods - Override

        #region Methods - Protected

        public void ReportProgress(TaskProgressChangedEventArgs e)
        {
            e.Instance = this;
            e.Percent = e.Percent > 100 ? 100 : e.Percent;
            e.Percent = e.Percent < 0 ? 0 : e.Percent;

            TryStop();

            OnProgressChanged(e);
            ProgressChanged?.Invoke(this, e);
        }

        protected TimeSpan GetElapsedTime()
        {
            return sw.Elapsed;
        }

        #endregion Methods - Protected

        #region Methods - Report

        public void ReportAlert(TaskAlertEventArgs e)
        {
            TryStop();

            OnAlert(e);
            Alert?.Invoke(this, e);
        }

        #endregion Methods - Report

        #region Methods - Events

        private void Arg_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ReportArgumentPropertyChanged(e);
        }

        private void Task_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(sender, e);

            if (e.PropertyName == "Argument")
                ReportArgumentPropertyChanged(null);
        }

        #endregion Methods - Events

        #region Methods - Private

        private void TryStop()
        {
            if (IsStopPending && threadIDMain == System.Threading.Thread.CurrentThread.ManagedThreadId)
                //if (IsStopPending)
                throw new TaskStopException();
        }

        private void ReportArgumentPropertyChanged(PropertyChangedEventArgs e)
        {
            OnArgumentPropertyChanged(e);

            string name = e == null ? "Argument" : e.PropertyName;
            object value = e == null ? this.GetPropertyValue(name) : Argument.GetPropertyValue(name);

            ArgumentPropertyChanged?.Invoke(this, e);
        }

        private void ReportStarted()
        {
            OnStarted();

            Started?.Invoke(this, new TaskStartedEventArgs(this));
        }

        private void ReportEnded()
        {
            OnEnded();

            Ended?.Invoke(this, new TaskEndedEventArgs());
        }

        private void ReportGo()
        {
            if (IsValidateMode)
                OnValidate();
            else
                OnGo();

            if (!IsValidateMode && Go != null)
                Go(this, new TaskGoEventArgs(this));
        }

        private void ReportCompleted(Stopwatch sw)
        {
            var e = new TaskCompletedEventArgs(this) { Result = OnGettingResult(), Argument = Argument, Elapsed = sw.Elapsed };

            OnCompleted(e);
            Completed?.Invoke(this, e);
        }

        private void ReportStopped()
        {
            OnStopped();
            Stopped?.Invoke(this, new TaskStoppedEventArgs());
        }

        private void ReportTerminated(Exception ex)
        {
            ReportUnhandledException(ex);
            OnTerminate(ex);
            Terminated?.Invoke(this, new TaskTerminatedEventArgs() { Exception = ex });
        }

        private void ReportUnhandledException(Exception ex)
        {
            string msg = ex.ToString();

            var e = new TaskAlertEventArgs
            {
                Description = msg,
                Grade = eMessageGrade.Exception,
                UserState = ex,
            };

            OnAlert(e);
            Alert?.Invoke(this, e);

            if (!AutoHandleException)
                throw ex;
        }

        private void SetArgument(TaskArgument value)
        {
            UninstallArgument(_Argument);
            _Argument = value;
            InstallArgument(_Argument);
        }

        private void InstallArgument(TaskArgument arg)
        {
            if (arg == null)
                return;

            arg.PropertyChanged += Arg_PropertyChanged;
        }

        private void UninstallArgument(TaskArgument arg)
        {
            if (arg == null)
                return;

            arg.PropertyChanged -= Arg_PropertyChanged;
        }

        #endregion Methods - Private

        #endregion Methods
    }
}