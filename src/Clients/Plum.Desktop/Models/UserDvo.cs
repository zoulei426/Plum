using Plum.Object;
using Plum.Validation;
using PropertyChanged;
using System.ComponentModel;

namespace Plum.Models
{
    /// <summary>
    /// 个人信息
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class UserDvo : ValidityDvo
    {
        #region Properties

        [DisplayName("用户名")]
        public string UserName { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        [DisplayName("邮箱")]
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        [DisplayName("手机号")]
        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public string AvatarUrl { get; set; }

        #endregion Properties

        public UserDvo(IValidatorProvider validatorProvider) : base(validatorProvider)
        {
        }
    }
}