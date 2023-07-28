using PropertyChanged;
using System;

namespace Plum.Users
{
    [AddINotifyPropertyChangedInterface]
    public class PlumUser
    {
        #region Base properties

        public virtual Guid? TenantId { get; set; }

        public virtual string UserName { get; set; }

        public virtual string Name { get; set; }

        public virtual string Surname { get; set; }

        public virtual string Email { get; set; }

        public virtual bool EmailConfirmed { get; set; }

        public virtual string PhoneNumber { get; set; }

        public virtual bool PhoneNumberConfirmed { get; set; }

        #endregion Base properties

        public string AvatarUrl { get; set; }

        public PlumUser()
        {
        }
    }
}