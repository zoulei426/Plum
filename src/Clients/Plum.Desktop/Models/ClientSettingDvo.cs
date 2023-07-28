using Plum.Object;
using PropertyChanged;

namespace Plum.Models
{
    [AddINotifyPropertyChangedInterface]
    public class ClientSettingDvo : DataViewObject
    {
        public ClientInfoDvo CenterClient { get; set; }

        public ClientInfoDvo DomainClient { get; set; }

        public ClientSettingDvo()
        {
            CenterClient = new ClientInfoDvo();
            CenterClient.Url = "http://202.61.89.138:803";
            DomainClient = new ClientInfoDvo();
            DomainClient.Url = "http://127.0.0.1:44386";
        }
    }
}