using Plum.Events;
using Plum.Windows;
using Plum.Windows.Authorizations;
using Plum.Windows.Commands;
using Plum.Windows.DataDictionaries;
using Plum.Windows.Mvvm;
using IdentityModel.Client;
using Prism.Ioc;
using PropertyChanged;
using Refit;
using System;
using System.Configuration;
using System.Windows.Input;

namespace Plum.Panels
{
    /// <summary>
    /// 登录页
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class SignInPanelViewModel : ViewModelBase, IViewLoadedAndUnloadedAware
    {
        #region Properties

        /// <summary>
        /// 账户
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 记住密码
        /// </summary>
        public bool RememberMe { get; set; }

        /// <summary>
        /// 自动登录
        /// </summary>
        public bool AutoSignIn { get; set; }

        #endregion Properties

        #region Fields

        private SignUpArgs signUpArgs;

        #endregion Fields

        #region Commands

        /// <summary>
        /// 登录
        /// </summary>
        public ICommand SignInCommand { get; set; }

        #endregion Commands

        #region Ctor

        public SignInPanelViewModel(IContainerExtension container) : base(container)
        {
            EventAggregator.GetEvent<SignUpSuccessEvent>()
                .Subscribe(args => signUpArgs = args);
        }

        #endregion Ctor

        #region Methods

        #region Methods - Override

        protected override void RegisterCommands()
        {
            SignInCommand = new RelayCommand(ExecuteSignIn, () => CanSignIn());
        }

        #endregion Methods - Override

        #region Methods - Public

        public void OnLoaded()
        {
            // 从注册页获取登录信息
            if (signUpArgs != null)
            {
                RememberMe = false;
                AutoSignIn = false;
                UserName = signUpArgs.UserName;
                Password = signUpArgs.Password;

                //SignInCommand.Execute(null);
                signUpArgs = null;
                return;
            }

            RememberMe = Configurator.GetValue<bool>(nameof(RememberMe));
            AutoSignIn = Configurator.GetValue<bool>(nameof(AutoSignIn));
            UserName = Configurator.GetValue<string>(nameof(UserName));
            Password = Configurator.GetValue<string>(nameof(Password)).DecryptDes();

            if (AutoSignIn)
            {
                SignInCommand.Execute(null);
            }
        }

        public void OnUnloaded()
        {
        }

        #endregion Methods - Public

        #region Methods - Private

        private bool CanSignIn()
        {
            return !UserName.IsNullOrEmpty() && !Password.IsNullOrEmpty();
        }

        private async void ExecuteSignIn()
        {
            try
            {
                EventAggregator.GetEvent<MainWindowLoadingEvent>().Publish(true);

                var httpClient = Container.Resolve<IPlumService>().DomainClient;

                var disco = await httpClient.GetDiscoveryDocumentAsync(
                    new DiscoveryDocumentRequest
                    {
                        Policy = new DiscoveryPolicy { RequireHttps = false }
                    });
                if (disco.IsError)
                {
                    //Notifier.Error(disco.Error, true);
                    Dispatcher.Invoke(() =>
                    {
                        MessageQueue.Enqueue(disco.Error);
                    });
                    EventAggregator.GetEvent<MainWindowLoadingEvent>().Publish(false);
                    return;
                }

                var tokenResponse = await httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest
                {
                    Address = disco.TokenEndpoint,
                    ClientId = ConfigurationManager.AppSettings["ClientId"],
                    ClientSecret = ConfigurationManager.AppSettings["ClientSecret"],
                    Scope = ConfigurationManager.AppSettings["Scope"],
                    UserName = UserName,
                    Password = Password
                });

                if (tokenResponse.IsError)
                {
                    //Notifier.Error(tokenResponse.Error, true);
                    //MessageQueue.Enqueue("用户名或密码错误");
                    MessageQueue.Enqueue(tokenResponse.Error);
                    Configurator.SetValue(nameof(AutoSignIn), false);
                    EventAggregator.GetEvent<MainWindowLoadingEvent>().Publish(false);
                    return;
                }

                httpClient.SetBearerToken(tokenResponse.AccessToken);

                Container.RegisterInstance(RestService.For<Desktop.IPlumApi>(httpClient));

                var systemApi = RestService.For<Plum.IPlumApi>(httpClient);
                AuthorizeManager.Initialize(systemApi);
                DataDictionaryManager.Initialize(systemApi);

                SignInSuccessed();
                Notifier.Success("登录成功");

                EventAggregator.GetEvent<MainWindowLoadingEvent>().Publish(false);
            }
            catch (Exception ex)
            {
                Configurator.SetValue(nameof(AutoSignIn), false);
                EventAggregator.GetEvent<MainWindowLoadingEvent>().Publish(false);

                //Notifier.Error("登录失败" + ex, true);
                MessageQueue.Enqueue(ex.Message);
            }
        }

        private void SignInSuccessed()
        {
            // Saves data.
            Configurator.SetValue(nameof(RememberMe), RememberMe);
            Configurator.SetValue(nameof(AutoSignIn), AutoSignIn);
            Configurator.SetValue(nameof(UserName), RememberMe ? UserName : string.Empty);
            Configurator.SetValue(nameof(Password), RememberMe ? Password.EncryptDes() : string.Empty);

            // Launches main window and closes itself.
            WindowTool.Switch<LoginWindow, MainWindow>();
        }

        #endregion Methods - Private

        #endregion Methods
    }
}