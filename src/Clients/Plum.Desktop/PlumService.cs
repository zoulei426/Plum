using Plum.Config;
using Plum.Models;
using Plum.Windows.Consts;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Plum.Windows
{
    public class PlumService : IPlumService
    {
        private readonly IConfigurator configurator;

        public HttpClient CenterClient { get; private set; }
        public HttpClient DomainClient { get; private set; }

        public PlumService(IConfigurator configurator)
        {
            this.configurator = configurator;
            Check.NotNull(configurator);
        }

        public IPlumService Load()
        {
            var clientSetting = configurator.GetValue<ClientSettingDvo>();
            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

            //client.DefaultRequestHeaders.Add("Accept",
            //    "application/vnd.github.v3+json");
            CenterClient = new HttpClient();
            CenterClient.BaseAddress = clientSetting.CenterClient.Url.ToUri();
            CenterClient.DefaultRequestHeaders.UserAgent.Add(
                new ProductInfoHeaderValue(new ProductHeaderValue("Plum.Desktop", version?.ToString())));
            CenterClient.DefaultRequestHeaders.AcceptLanguage.Add(
                new StringWithQualityHeaderValue(Headers.ACCEPT_LANGUAGE));

            DomainClient = new HttpClient();
            DomainClient.BaseAddress = clientSetting.DomainClient.Url.ToUri();
            DomainClient.DefaultRequestHeaders.UserAgent.Add(
                new ProductInfoHeaderValue(new ProductHeaderValue("Plum.Desktop", version?.ToString())));
            DomainClient.DefaultRequestHeaders.AcceptLanguage.Add(
                new StringWithQualityHeaderValue(Headers.ACCEPT_LANGUAGE));

            return this;
        }
    }
}