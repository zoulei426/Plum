using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Account.Web.Areas.Account.Controllers.Models;
using Volo.Abp.Identity;

namespace Plum.Library.Contracts
{
    public interface IPlumApi
    {
        [Post("/api/account/login")]
        Task<AbpLoginResult> Login([Body] UserLoginInfo loginInfo);

        [Get("/api/identity/users/by-username/{userName}")]
        Task<IdentityUserDto> GetUserByUserName(string userName);

        [Get("/api/identity/users/by-email/{email}")]
        Task<IdentityUserDto> GetUserByEamil(string email);
    }
}