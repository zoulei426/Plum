using Refit;
using System.Threading.Tasks;
using Volo.Abp.Account;
using Volo.Abp.Account.Web.Areas.Account.Controllers.Models;
using Volo.Abp.Identity;

namespace Plum.Desktop
{
    public interface IPlumApi
    {
        #region Account

        [Post("/api/account/login")]
        Task<AbpLoginResult> LoginAsync([Body] UserLoginInfo loginInfo);

        [Get("/api/account/logout")]
        Task LogoutAsync();

        [Post("/api/account/register")]
        Task<IdentityUserDto> RegisterAsync([Body] RegisterDto registerDto);

        [Get("/api/account/verifycode")]
        Task<bool> SendVerificationCodeAsync(string email);

        [Get("/api/identity/my-profile")]
        Task<ProfileDto> GetMyProfileAsync();

        [Put("/api/identity/my-profile")]
        Task<ProfileDto> UpdateMyProfileAsync([Body] UpdateProfileDto updateProfileDto);

        [Post("/api/identity/my-profile/change-password")]
        Task ChangePasswordAsync([Body] ChangePasswordInput input);

        #endregion Account
    }
}