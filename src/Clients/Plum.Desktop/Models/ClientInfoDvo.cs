using Plum.Object;
using PropertyChanged;

namespace Plum.Models
{
    [AddINotifyPropertyChangedInterface]
    public class ClientInfoDvo : DataViewObject
    {
        public string Url { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string Scope { get; set; }

        public ClientInfoDvo()
        {
            ClientId = "Plum_App";
            ClientSecret = "5AC48988-3805-4327-9210-916D284C7353";
            Scope = "Plum";
        }
    }
}