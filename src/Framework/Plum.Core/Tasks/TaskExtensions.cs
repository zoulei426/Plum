using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Plum.Tasks
{
    public static class TaskExtensions
    {
        #region Methods

        public static void StartAsync(this ITask source, bool validate = false)
        {
            ThreadWorkstaion.Start(new Action(() => { source.Start(validate); }));
        }

        public static void StartAsync(this ITask source, ApartmentState state, bool validate = false)
        {
            ThreadWorkstaion.Start(new Action(() => { source.Start(validate); }), state);
        }

        public static void ReportProgress(this ITask source, int percent, object userState = null)
        {
            var e = new TaskProgressChangedEventArgs(percent, userState);
            source.ReportProgress(e);
        }

        public static bool ReportSuccess(this ITask source, string description, string catalog = null, TaskAlertMetadata meta = null)
        {
            var e = new TaskAlertEventArgs
            {
                Description = description,
                Grade = eMessageGrade.Success,
                UserState = catalog.IsNullOrEmpty() ? null : new TaskAlertUserStateCatalog() { Catalog = catalog },
                Metadata = meta,
            };

            source.ReportAlert(e);

            return e.IsCancel;
        }

        public static bool ReportInfomation(this ITask source, string description, string catalog = null, TaskAlertMetadata meta = null)
        {
            var e = new TaskAlertEventArgs
            {
                Description = description,
                Grade = eMessageGrade.Infomation,
                UserState = catalog.IsNullOrEmpty() ? null : new TaskAlertUserStateCatalog() { Catalog = catalog },
                Metadata = meta,
            };

            source.ReportAlert(e);

            return e.IsCancel;
        }

        public static bool ReportError(this ITask source, string description, string catalog = null, TaskAlertMetadata meta = null)
        {
            var e = new TaskAlertEventArgs
            {
                Description = description,
                Grade = eMessageGrade.Error,
                UserState = catalog.IsNullOrEmpty() ? null : new TaskAlertUserStateCatalog() { Catalog = catalog },
                Metadata = meta,
            };

            source.ReportAlert(e);

            return e.IsCancel;
        }

        public static bool ReportWarn(this ITask source, string description, string catalog = null, TaskAlertMetadata meta = null)
        {
            var e = new TaskAlertEventArgs
            {
                Description = description,
                Grade = eMessageGrade.Warn,
                UserState = catalog.IsNullOrEmpty() ? null : new TaskAlertUserStateCatalog() { Catalog = catalog },
                Metadata = meta,
            };

            source.ReportAlert(e);

            return e.IsCancel;
        }

        //public static bool ReportException(this ITask source, Exception ex)
        //{
        //    string msg = string.Format(LanguageAttribute.GetLanguage("lang670303"), ex);
        //    return source.ReportException(ex, msg);
        //}

        public static bool ReportException(this ITask source, Exception ex, string description)
        {
            var e = new TaskAlertEventArgs
            {
                Description = description,
                Grade = eMessageGrade.Exception,
                UserState = ex,
            };

            source.ReportAlert(e);

            return e.IsCancel;
        }

        public static bool ReportAlert(this ITask source, object userState, string description)
        {
            var e = new TaskAlertEventArgs(userState, description);

            source.ReportAlert(e);

            return e.IsCancel;
        }

        public static bool ReportAlert(this ITask source, eMessageGrade grade, object userState, string description)
        {
            var e = new TaskAlertEventArgs(grade, userState, description);
            source.ReportAlert(e);

            return e.IsCancel;
        }

        #endregion Methods
    }
}