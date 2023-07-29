using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plum.Windows.Apps.Account
{
    public class UserLoginInfo
    {
        [Required]
        [StringLength(255)]
        public string UserNameOrEmailAddress { get; set; }

        [Required]
        [StringLength(32)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}