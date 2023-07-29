using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plum.Windows.Apps.Account
{
    public interface IPlumAccountApi
    {
        [Post("/api/account/login")]
        Task<LoginResult> LoginAsync([Body] UserLoginInfo loginInfo);

        [Get("/api/account/logout")]
        Task LogoutAsync();

        [Post("/api/account/register")]
        Task<PlumUser> RegisterAsync([Body] RegisterDto registerDto);

        [Get("/api/account/verifycode")]
        Task<bool> SendVerificationCodeAsync(string email);

        [Get("/api/identity/my-profile")]
        Task<PlumUser> GetMyProfileAsync();

        //[Put("/api/identity/my-profile")]
        //Task<PlumUser> UpdateMyProfileAsync([Body] UpdateProfileDto updateProfileDto);

        //[Post("/api/identity/my-profile/change-password")]
        //Task ChangePasswordAsync([Body] ChangePasswordInput input);
    }
}