using System.Collections.Generic;
using System.ComponentModel;

namespace Plum.Validation
{
    public interface IValidityInfo : IDataErrorInfo
    {
        bool IsValid { get; }

        string ValidInfo { get; }

        Dictionary<string, List<string>> Errors { get; }
    }
}