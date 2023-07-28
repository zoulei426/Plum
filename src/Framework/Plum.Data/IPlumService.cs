using System.Net.Http;

namespace Plum
{
    public interface IPlumService
    {
        HttpClient CenterClient { get; }
        HttpClient DomainClient { get; }

        IPlumService Load();
    }
}