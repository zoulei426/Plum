using Plum.Config;
using Plum.Log;
using Plum.Notify;
using Plum.Object;
using Plum.Windows.Attributes;
using Plum.Windows.Consts;
using Plum.Windows.Params;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.Localization;
using Microsoft.Win32;
using Prism.Events;
using Prism.Ioc;
using Prism.Regions;
using Prism.Services.Dialogs;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Plum.Windows.Mvvm
{
    /// <summary>
    /// 界面ViewModel基类
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class ViewModelBase : BindableObject
    {
        #region Fields

        private Dictionary<string, IRegionNavigationJournal> _RegionJournal;

        private readonly IStringLocalizer _Localizer;

        #endregion Fields

        #region Properties

        public bool IsBusy { get; set; }

        /// <summary>
        /// Gets or sets the dispatcher.
        /// </summary>
        public Dispatcher Dispatcher { get; set; }

        protected IStringLocalizer Localizer => _Localizer;

        /// <summary>
        /// 事件汇总器，用于发布或订阅事件
        /// </summary>
        protected readonly IEventAggregator EventAggregator;

        /// <summary>
        /// 区域管理器
        /// </summary>
        protected readonly IRegionManager RegionManager;

        /// <summary>
        /// 模态框服务
        /// </summary>
        protected readonly IDialogService DialogService;

        /// <summary>
        /// 依赖注入容器
        /// </summary>
        protected readonly IContainerExtension Container;

        /// <summary>
        /// 配置文件
        /// </summary>
        protected readonly IConfigurator Configurator;

        /// <summary>
        /// 日志器
        /// </summary>
        protected readonly ILogger Logger;

        /// <summary>
        /// 通知器
        /// </summary>
        protected readonly INotifier Notifier;

        //public readonly ISnackbarMessageQueue MessageQueue;
        public ISnackbarMessageQueue MessageQueue { get; set; }

        #endregion Properties

        #region Ctor

        /// <summary>
        /// 基类ViewModel构造函数
        /// </summary>
        /// <param name="container">注入容器</param>
        public ViewModelBase(IContainerExtension container)
        {
            Container = container;
            //var fac = container.Resolve<IStringLocalizerFactory>();
            //_Localizer = fac.Create("", "");
            DialogService = container.Resolve<IDialogService>();
            RegionManager = container.Resolve<IRegionManager>();
            EventAggregator = container.Resolve<IEventAggregator>();
            Configurator = container.Resolve<IConfigurator>();
            Logger = container.Resolve<ILogger>();
            Notifier = container.Resolve<INotifier>();
            MessageQueue = container.Resolve<ISnackbarMessageQueue>();

            _RegionJournal = new Dictionary<string, IRegionNavigationJournal>();

            RegisterCommands();
            SubscribeEvents();
        }

        #endregion Ctor

        #region Methods

        /// <summary>
        /// Invokes the specified action.
        /// </summary>
        /// <param name="action">The action.</param>
        protected virtual void Invoke(Action action) => Dispatcher.Invoke(action);

        /// <summary>
        /// 注册命令
        /// </summary>
        protected virtual void RegisterCommands()
        {
        }

        /// <summary>
        /// 订阅消息
        /// </summary>
        protected virtual void SubscribeEvents()
        {
        }

        #region 导航

        /// <summary>
        /// 导航到指定Page
        /// </summary>
        /// <param name="regionName">区域名称</param>
        /// <param name="target">目标Page名称</param>
        /// <param name="navigationCallback">导航回调函数</param>
        protected void Navigate(string regionName, string target, Action<NavigationResult> navigationCallback = null, NavigationParameters navigationParameters = null)
        {
            IRegion region = RegionManager.Regions[regionName];
            if (region == null) return;
            region.RemoveAll();

            if (navigationParameters is null)
            {
                region.RequestNavigate(
                    target,
                    args =>
                    {
                        _RegionJournal[region.Name] = args.Context.NavigationService.Journal;
                    });
            }
            else
            {
                region.RequestNavigate(
                    target,
                    args =>
                    {
                        _RegionJournal[region.Name] = args.Context.NavigationService.Journal;
                    },
                    navigationParameters);
            }
        }

        protected bool CanNavigateBackward(params string[] regions)
        {
            if (regions is null ||
                regions.Length == 0 ||
                regions.Any(x => !_RegionJournal.ContainsKey(x)) ||
                regions.Any(x => _RegionJournal[x] is null))
                return false;

            return regions.All(x => _RegionJournal[x].CanGoBack);
        }

        /// <summary>
        /// 向后导航
        /// </summary>
        protected void NavigateBackward(params string[] regions)
        {
            if (!CanNavigateBackward(regions))
                return;

            regions.ForEach(x => _RegionJournal[x].GoBack());
        }

        protected bool CanNavigateForward(params string[] regions)
        {
            if (regions is null ||
               regions.Length == 0 ||
               regions.Any(x => !_RegionJournal.ContainsKey(x)) ||
               regions.Any(x => _RegionJournal[x] is null))
                return false;

            return regions.All(x => _RegionJournal[x].CanGoForward);
        }

        /// <summary>
        /// 向前导航
        /// </summary>
        protected void NavigateForward(params string[] regions)
        {
            if (!CanNavigateForward(regions))
                return;

            regions.ForEach(x => _RegionJournal[x].GoForward());
        }

        #endregion 导航

        #region 模态框

        //protected virtual async Task ShowDialog<TView>()
        //{
        //    var view = Container.Resolve<TView>();
        //    if (view is null)
        //        return;

        //    await DialogHost.Show(view, SystemDialogHost.RootDialog);
        //}

        protected virtual async Task ShowDialog<TView>(
            string title = null,
            IDialogParameters parameters = null,
            Action<IDialogResult> callback = null,
            object dialogIdentifier = null)
        {
            var view = Container.Resolve<TView>();
            if (view is null)
                return;

            if (dialogIdentifier is null)
                dialogIdentifier = SystemDialogHost.RootDialog;

            await DialogHost.Show(view, dialogIdentifier, (sender, args) =>
            {
                var dialogContent = args.Session.Content as FrameworkElement;
                if (dialogContent is not null)
                {
                    var dialogAware = dialogContent.DataContext as IDialogAware;
                    if (dialogAware is not null)
                    {
                        dialogAware.RequestClose += callback;

                        var @params = new DialogParameters
                        {
                            { "Title", title }
                        };
                        @params.AddRange(parameters);

                        dialogAware.OnDialogOpened(@params);
                    }
                }
            }, (sender, args) =>
            {
                // var buttonResult = args.Parameter is bool parameter
                //? parameter ? ButtonResult.OK : ButtonResult.Cancel
                //: ButtonResult.Ignore;

                var dialogContent = args.Session.Content as FrameworkElement;
                if (dialogContent is not null)
                {
                    var dialogAware = dialogContent.DataContext as IDialogAware;
                    if (dialogAware is not null)
                    {
                        dialogAware.RequestClose -= callback;
                    }
                }
            });
        }

        protected virtual async Task ShowObjectDialog<T>(string title, T @object, Action<IDialogResult> callback = null)
            where T : BindableObject
        {
            await ShowDialog<IPropertyGridDialog>(title, new PropertyGridDialogParameters(@object), callback);
        }

        protected virtual void ShowOpenFileDialog(string filter, Action<IDialogResult> callback, bool multiselect = false)
        {
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            // 选择附件
            var ofd = new OpenFileDialog
            {
                Title = "选择文件",
                Filter = filter,
                FileName = "选择文件",
                FilterIndex = 1,
                ValidateNames = false,
                CheckFileExists = false,
                CheckPathExists = true,
                Multiselect = multiselect,
                RestoreDirectory = true,
                InitialDirectory = desktopPath
            };
            bool? result = null;
            try
            {
                result = ofd.ShowDialog();
            }
            catch (Exception ex)
            {
                Notifier.Error(ex.ToString());
            }
            callback.Invoke(new DialogResult(
                result.HasValue
                ? result.Value
                    ? ButtonResult.OK
                    : ButtonResult.Cancel
                : ButtonResult.Cancel,
                multiselect
                    ? new FileDialogParameters(ofd.FileNames.ToList())
                    : new FileDialogParameters(ofd.FileName)
                ));
        }

        protected virtual void ShowSaveFileDialog(string fileName, string filter, Action<IDialogResult> callback)
        {
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            var sfd = new SaveFileDialog
            {
                FileName = fileName, // 默认文件名
                RestoreDirectory = true, //保存对话框是否记忆上次打开的目录
                Filter = filter
            };
            bool? result = sfd.ShowDialog();

            callback.Invoke(new DialogResult(
                result.HasValue
                ? result.Value
                    ? ButtonResult.OK
                    : ButtonResult.Cancel
                : ButtonResult.Cancel,
                new FileDialogParameters(sfd.FileName)
                ));
        }

        #endregion 模态框

        /// <summary>
        /// 弹框提示
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="callback">回调函数</param>
        protected void Alert(string message, Action<IDialogResult> callback = null)
        {
            DialogService.ShowDialog("AlertDialog", new DialogParameters($"message={message}"), callback);
        }

        /// <summary>
        /// 确认框提示
        /// </summary>
        /// <param name="message">确认框消息</param>
        /// <param name="callback">回调函数</param>
        protected async Task ShowConfirmDialogAsync(string message = null, Action<IDialogResult> callback = null, object dialogIdentifier = null)
        {
            //await ShowDialog<IConfirmDialog>(string.Empty, null, callback, dialogIdentifier);
            var view = Container.Resolve<IConfirmDialog>();
            if (view is null)
                return;

            if (dialogIdentifier is null)
                dialogIdentifier = SystemDialogHost.RootDialog;

            await DialogHost.Show(view, dialogIdentifier, (sender, args) =>
            {
                //var dialogContent = args.Session.Content as FrameworkElement;
                //if (dialogContent is not null)
                //{
                //    var dialogAware = dialogContent.DataContext as IDialogAware;
                //    if (dialogAware is not null)
                //    {
                //        //dialogAware.RequestClose += callback;

                //        var @params = new DialogParameters
                //        {
                //            { "Title", "提示" }
                //        };

                //        dialogAware.OnDialogOpened(@params);
                //    }
                //}
            }, (sender, args) =>
            {
                var buttonResult = args.Parameter is bool parameter
                    ? parameter ? ButtonResult.OK : ButtonResult.Cancel
                    : ButtonResult.Ignore;

                callback.Invoke(new DialogResult(buttonResult));
            });
        }

        public virtual FrameworkElement CreateView()
        {
            var ui = ResolveView(GetType());
            ui.DataContext = this;

            return ui;
        }

        public virtual FrameworkElement CreateView(Type typeModel, object instance)
        {
            var ui = ResolveView(typeModel);
            ui.DataContext = instance;

            return ui;
        }

        public static FrameworkElement ResolveView(Type typeModel, params object[] args)
        {
            var attr = typeModel.GetAttribute<ViewAttribute>();
            if (attr == null || attr.Type == null)
                throw new NotImplementedException();

            var m = Activator.CreateInstance(attr.Type, args) as FrameworkElement;
            return m;
        }

        //public static ViewModelBase ResolveModel<T>(params object[] args) where T : FrameworkElement
        //{
        //    return ResolveModel(typeof(T), args);
        //}

        //public static ViewModelBase ResolveModel(Type typeView, params object[] args)
        //{
        //    var attr = typeView.GetAttribute<ViewModelAttribute>();
        //    if (attr == null || attr.Type == null)
        //        throw new NotImplementedException();

        //    var m = Activator.CreateInstance(attr.Type, args) as ViewModelBase;
        //    return m;
        //}
        protected virtual void TaskAlert(object sender, Tasks.TaskAlertEventArgs e)
        {
            switch (e.Grade)
            {
                case eMessageGrade.Infomation:
                    Notifier.Info(e.Description);
                    Logger.Infomation(e.Description);
                    break;

                case eMessageGrade.Warn:
                    Notifier.Warning(e.Description);
                    Logger.Warning(e.Description);
                    break;

                case eMessageGrade.Error:
                    Notifier.Error(e.Description);
                    Logger.Error(e.Description);
                    break;

                case eMessageGrade.Success:
                    Notifier.Success(e.Description);
                    Logger.Infomation(e.Description);
                    break;
            }
        }

        #endregion Methods
    }
}