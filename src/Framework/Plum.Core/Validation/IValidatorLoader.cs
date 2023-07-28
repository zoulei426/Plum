using System.Collections.Generic;
using System.Reflection;
using static FluentValidation.AssemblyScanner;

namespace Plum.Validation
{
    public interface IValidatorLoader
    {
        List<AssemblyScanResult> ScanResults { get; set; }

        IEnumerable<AssemblyScanResult> ScanValidators(Assembly assembly);
    }
}