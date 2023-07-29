using Plum.Events;
using Plum.Object;
using Plum.Windows.Commands;
using Plum.Windows.Mvvm;
using Prism.Commands;
using Prism.Ioc;
using PropertyChanged;
using Refit;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Plum.Panels
{
    /// <summary>
    /// 注册页
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class SignUpPanelViewModel : ViewModelBase, IViewLoadedAndUnloadedAware
    {
        #region Properties

        /// <summary>
        /// 是否锁定
        /// </summary>
        public bool IsLocked { get; set; }

        public int RemainingTimeBasedSecond { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string VerificationCode { get; set; }

        #endregion Properties

        #region Fields

        private const int IntervalBasedSecond = 60;

        private SignUpArgs signUpArgs;

        private Desktop.IPlumApi api;

        #endregion Fields

        #region Commands

        /// <summary>
        /// 注册
        /// </summary>
        public ICommand SignUpCommand { get; set; }

        /// <summary>
        /// 发送验证码
        /// </summary>
        public ICommand SendVerificationCodeCommand { get; set; }

        #endregion Commands

        #region Ctor

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="container"></param>
        public SignUpPanelViewModel(IContainerExtension container) : base(container)
        {
            ConfigureSignUpArgs();
        }

        #endregion Ctor

        #region Methods

        public void OnLoaded()
        {
            api = Container.Resolve<Desktop.IPlumApi>();
        }

        public void OnUnloaded()
        {
        }

        #region Methods - Override

        protected override void RegisterCommands()
        {
            SignUpCommand = new RelayCommand(ExecuteSignUp, CanSignUp);

            SendVerificationCodeCommand = new DelegateCommand(
              async () =>
              {
                  try
                  {
                      //var result = await api.SendVerificationCodeAsync(Email);
                      var result = true;
                      if (result)
                      {
                          await StartVerificationCodeTimer();
                      }
                  }
                  catch (Exception ex)
                  {
                      Console.WriteLine(ex);
                  }
              },
              () => true);
        }

        #endregion Methods - Override

        #region Methods - Private

        private bool CanSignUp()
        {
            if (Email.IsNullOrEmpty() || UserName.IsNullOrEmpty() || Password.IsNullOrEmpty())
                return false;

            return true;
        }

        private async void ExecuteSignUp()
        {
            EventAggregator.GetEvent<MainWindowLoadingEvent>().Publish(true);

            try
            {
                var user = await api.RegisterAsync(new Volo.Abp.Account.RegisterDto
                {
                    UserName = UserName,
                    EmailAddress = Email,
                    Password = Password,
                    AppName = "Plum"
                });

                if (user is not null)
                {
                    //Password = string.Empty;
                    Notifier.Success("注册成功", true);
                    ConfigureSignUpArgs(configure: args =>
                    {
                        args.UserName = UserName;
                        args.Password = Password;
                    });
                    EventAggregator.GetEvent<SignUpSuccessEvent>().Publish(signUpArgs);
                }
            }
            catch (ApiException ex)
            {
                if (ex.Content.IsNullOrEmpty())
                {
                    Notifier.Error(ex.Message, true);
                }
                Notifier.Error(ex.Content.FromJson<ErrorResponse>().ToString(), true);
            }
            catch (HttpRequestException httpRequestException)
            {
                Notifier.Error(httpRequestException.ToDetailString(), true);
            }
            catch (Exception ex)
            {
                Notifier.Error("注册失败" + ex, true);
            }

            EventAggregator.GetEvent<MainWindowLoadingEvent>().Publish(false);
        }

        private void ConfigureSignUpArgs(Action<SignUpArgs> configure = null, bool force = false)
        {
            if (signUpArgs == null || force) signUpArgs = new SignUpArgs();

            configure?.Invoke(signUpArgs);
        }

        private async Task StartVerificationCodeTimer()
        {
            IsLocked = true;
            for (RemainingTimeBasedSecond = IntervalBasedSecond; RemainingTimeBasedSecond > 0; RemainingTimeBasedSecond--)
            {
                await Task.Delay(1000);
            }
            IsLocked = false;
        }

        #endregion Methods - Private

        #endregion Methods
    }
}