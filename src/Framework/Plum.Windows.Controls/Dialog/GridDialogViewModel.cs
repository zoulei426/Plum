using Plum.Tools;
using Plum.Windows.Attributes;
using Plum.Windows.Commands;
using Plum.Windows.Mvvm;
using Plum.Windows.Params;
using Npoi.Mapper;
using Prism.Ioc;
using Prism.Services.Dialogs;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Plum.Windows.Controls.Dialog
{
    [AddINotifyPropertyChangedInterface]
    [View(typeof(GridDialog))]
    public class GridDialogViewModel : DialogViewModel
    {
        public object ItemsSource { get; set; }

        public object SelectedItem { get; set; }

        public override event Action<IDialogResult> RequestClose;

        private string ObejctName;

        public ICommand ExportCommand { get; set; }

        public GridDialogViewModel(IContainerExtension container) : base(container)
        {
        }

        protected override void RegisterCommands()
        {
            ExportCommand = new RelayCommand(ExecuteExport, CanExport);
        }

        private bool CanExport()
        {
            return ItemsSource is not null;
        }

        private void ExecuteExport()
        {
            //ShowSaveFileDialog(
            //    $"{ObejctName}项目质检报告_{DateTime.Now:yyyy_MM_dd}",
            //    "Excel文件(*.xls,*.xlsx)|*.xls;*.xlsx",
            //    async (dialogResult) =>
            //    {
            //        if (dialogResult.Result == ButtonResult.OK)
            //        {
            //            Notifier.Info($"开始导出质检报告");

            //            var saveFile = (dialogResult.Parameters as FileDialogParameters).FileName;
            //            saveFile = PathTool.GetNewFileName(saveFile);

            //            await Task.Run(() =>
            //            {
            //                var mapper = new Mapper();
            //                IEnumerable<dynamic> objs = (IEnumerable<dynamic>)ItemsSource;
            //                mapper.Put(objs);

            //                mapper.Save(saveFile, true);
            //            });

            //            Notifier.Success($"质检报告导出成功");
            //        }
            //    });
        }

        public override void OnLoaded()
        {
            base.OnLoaded();
        }

        public override void OnUnloaded()
        {
            base.OnUnloaded();
            //ItemsSource?.Clear();
            ItemsSource = null;
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            base.OnDialogOpened(parameters);

            Dispatcher.Invoke(() =>
            {
                IsBusy = true;

                ObejctName = parameters.GetValue<string>("ObjectName");
                var type = parameters.GetValue<Type>("ObjectType");
                var result = parameters.GetValue<string>("Object").FromJson(type);

                ItemsSource = result;

                IsBusy = false;
            });
        }
    }
}