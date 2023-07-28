using FluentValidation;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static FluentValidation.AssemblyScanner;

namespace Plum.Validation
{
    public class ValidatorLoader : IValidatorLoader
    {
        public List<AssemblyScanResult> ScanResults { get; set; }

        private static ValidatorLoader loader = new();

        private ValidatorLoader()
        {
            ScanResults = new List<AssemblyScanResult>();
        }

        public static ValidatorLoader GetInstance()
        {
            return loader;
        }

        public IValidatorLoader Load(Assembly assembly)
        {
            var types = assembly.GetTypes();
            var openGenericType = typeof(IValidator<>);

            var query = from type in types
                        where !type.IsAbstract && !type.IsGenericTypeDefinition
                        let interfaces = type.GetInterfaces()
                        let genericInterfaces = interfaces.Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == openGenericType)
                        let matchingInterface = genericInterfaces.FirstOrDefault()
                        where matchingInterface != null
                        select new AssemblyScanResult(matchingInterface, type);
            ScanResults.AddRange(query.ToList());
            //foreach (var type in types)
            //{
            //    if (type.IsAssignableFrom<IValidator>())
            //    {
            //        ScanResults.Add(new AssemblyScanResult(type, type));
            //    }
            //}

            //ScanResults = (List<AssemblyScanResult>)FindValidatorsInAssembly(assembly).GetEnumerator();
            return this;
        }

        public IEnumerable<AssemblyScanResult> ScanValidators(Assembly assembly)
        {
            return (IEnumerable<AssemblyScanResult>)FindValidatorsInAssembly(assembly).GetEnumerator();
        }
    }
}