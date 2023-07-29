using PropertyChanged;
using System;

namespace Plum.Windows.Apps.Account
{
    [AddINotifyPropertyChangedInterface]
    public class PlumUser
    {
        #region properties

        public virtual Guid? TenantId { get; set; }

        public virtual string UserName { get; set; }

        public virtual string Name { get; set; }

        public virtual string Surname { get; set; }

        public virtual string Email { get; set; }

        public virtual bool EmailConfirmed { get; set; }

        public virtual string PhoneNumber { get; set; }

        public virtual bool PhoneNumberConfirmed { get; set; }
        public string AvatarUrl { get; set; }

        #endregion properties

        public PlumUser()
        {
        }

        public static PlumUser Admin()
        {
            return new PlumUser
            {
                UserName = "admin",
                Name = "管理员"
            };
        }
    }
}