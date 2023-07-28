/*
 *  ┌─────────────────────────────────────────────────────────────┐
 *  │┌───┬───┬───┬───┬───┬───┬───┬───┬───┬───┬───┬───┬───┬───┬───┐│
 *  ││Esc│!1 │@2 │#3 │$4 │%5 │^6 │&7 │*8 │(9 │)0 │_- │+= │|\ │`~ ││
 *  │├───┴─┬─┴─┬─┴─┬─┴─┬─┴─┬─┴─┬─┴─┬─┴─┬─┴─┬─┴─┬─┴─┬─┴─┬─┴─┬─┴───┤│
 *  ││ Tab │ Q │ W │ E │ R │ T │ Y │ U │ I │ O │ P │{[ │}] │ BS  ││
 *  │├─────┴┬──┴┬──┴┬──┴┬──┴┬──┴┬──┴┬──┴┬──┴┬──┴┬──┴┬──┴┬──┴─────┤│
 *  ││ Ctrl │ A │ S │ D │ F │ G │ H │ J │ K │ L │: ;│" '│ Enter  ││
 *  │├──────┴─┬─┴─┬─┴─┬─┴─┬─┴─┬─┴─┬─┴─┬─┴─┬─┴─┬─┴─┬─┴─┬─┴────┬───┤│
 *  ││ Shift  │ Z │ X │ C │ V │ B │ N │ M │< ,│> .│? /│Shift │Fn ││
 *  │└─────┬──┴┬──┴──┬┴───┴───┴───┴───┴───┴──┬┴───┴┬──┴┬─────┴───┘│
 *  │      │Fn │ Alt │         Space         │ Alt │Win│   HHKB   │
 *  │      └───┴─────┴───────────────────────┴─────┴───┘          │
 *  └─────────────────────────────────────────────────────────────┘
 */

using Plum.Config;
using Plum.Log;
using Plum.Notify;
using Plum.Validation;
using Plum.Windows;
using Plum.Windows.Config;
using Plum.Windows.Controls.Dialog;
using Plum.Windows.Extensions;
using Plum.Windows.Mvvm;
using Plum.Windows.Notify;
using FluentValidation;
using MaterialDesignThemes.Wpf;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Refit;
using Serilog;
using Serilog.Events;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Plum.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        #region Methods

        #region Methods - Override

        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            ProcessController.CheckSingleton();

            base.OnStartup(e);

            Serilog.Log.Logger = new LoggerConfiguration()
             .MinimumLevel.Information()//最小的记录等级
             .MinimumLevel.Override("Microsoft", LogEventLevel.Information)//对其他日志进行重写,除此之外,目前框架只有微软自带的日志组件
             .WriteTo.File(Path.Combine(SystemPath.Logs, "log.txt"),
                      rollingInterval: RollingInterval.Day)
             .CreateLogger();

            //UI线程未捕获异常处理事件
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            //非UI线程未捕获异常处理事件
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            //Task线程内未捕获异常处理事件
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            //Exit += App_Exit;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance(new Configurator().Load());
            containerRegistry.RegisterInstance(new PlumService(Container.Resolve<IConfigurator>()).Load());
            containerRegistry.RegisterInstance(RestService.For<Desktop.IPlumApi>(Container.Resolve<IPlumService>().DomainClient));
            containerRegistry.RegisterSingleton<Log.ILogger, SeriLogger>();
            containerRegistry.RegisterSingleton<INotifier, Notifier>();
            containerRegistry.RegisterInstance<ISnackbarMessageQueue>(new SnackbarMessageQueue(TimeSpan.FromSeconds(2)));
            containerRegistry.RegisterSingleton<IPropertyGridDialog, PropertyGridDialog>();
            containerRegistry.RegisterSingleton<IConfirmDialog, ConfirmDialog>();
            containerRegistry.RegisterSingleton<IValidatorProvider, ValidatorProvider>();

            ValidatorOptions.Global.DisplayNameResolver = (type, member, lambda) =>
            {
                if (member is not null)
                {
                    return member.GetDisplayName();
                }
                return null;
            };

            var currentAssembly = Assembly.GetAssembly(this.GetType());

            containerRegistry.RegisterInstance(
                typeof(IValidatorLoader),
                ValidatorLoader.GetInstance().Load(currentAssembly));
        }

        protected override void ConfigureViewModelLocator()
        {
            ViewModelLocationProvider.SetDefaultViewModelFactory(
                new ViewModelResolver(() => Container).UseDefaultConfigure().ResolveViewModelForView);
        }

        /// <summary>
        /// 创建窗体
        /// </summary>
        /// <returns></returns>
        protected override Window CreateShell()
        {
            InitializeCultureInfo();
            return Container.Resolve<LoginWindow>();
        }

        protected override void Initialize()
        {
            base.Initialize();
            Plum.Properties.Settings.Default.PropertyChanged += (sender, eventArgs) => Plum.Properties.Settings.Default.Save();
        }

        protected override void OnInitialized()
        {
            ApiExceptionResolverExtensions.SetUnityContainer(Container);
            base.OnInitialized();
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            var modulePath = @".\Modules";
            Directory.CreateDirectory(modulePath);
            return new DirectoryModuleCatalog { ModulePath = modulePath };
        }

        protected override void OnExit(ExitEventArgs e)
        {
            //Container.Resolve<IEventAggregator>().GetEvent<ApplicationExiting>().Publish();
            Serilog.Log.CloseAndFlush();

            base.OnExit(e);
        }

        #endregion Methods - Override

        #region Methods - Private

        private void InitializeCultureInfo()
        {
        }

        /// <summary>
        /// UI线程未捕获异常处理事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Exception ex = e.Exception;
            Container.Resolve<INotifier>().Error(ex.Message);
            //MessageBox.Show($"程序运行出错，原因：{ex.Message}-{ex.InnerException?.Message}",
            //    "系统提示", MessageBoxButton.OK, MessageBoxImage.Error);

            Container.Resolve<Log.ILogger>().Error(ex.Message, ex);
            e.Handled = true;
        }

        /// <summary>
        /// 非UI线程未捕获异常处理事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                MessageBox.Show($"程序组件出错，原因：{ex.Message}",
                    "系统提示", MessageBoxButton.OK, MessageBoxImage.Error);
                Container.Resolve<Log.ILogger>().Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// Task线程内未捕获异常处理事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            Exception ex = e.Exception;
            MessageBox.Show($"执行任务出错，原因：{ex.Message}",
                "系统提示", MessageBoxButton.OK, MessageBoxImage.Error);
            Container.Resolve<Log.ILogger>().Error(ex.Message, ex);
            //设置该异常已察觉
            e.SetObserved();
        }

        //private void App_Exit(object sender, ExitEventArgs e)
        //{
        //}

        #endregion Methods - Private

        #endregion Methods
    }
}