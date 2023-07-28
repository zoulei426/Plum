using Plum.Windows.Consts;
using Refit;
using System.Threading.Tasks;

namespace Plum
{
    [Headers(
        Headers.USER_AGENT,
        Headers.ACCEPT_LANGUAGE)]
    public interface IPlumApi
    {
        [Get("/api/account/is-granted")]
        Task<bool> IsGranted(string policyName);

        #region DataDictionary

        //[Get("/api/data-dictionary/data-dictionary/by-code")]
        //Task<DataDictionaryDto> GetDataDictionaryByCodeAsync(string code);

        #endregion DataDictionary
    }
}