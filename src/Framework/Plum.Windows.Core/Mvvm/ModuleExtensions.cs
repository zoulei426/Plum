using Plum.Windows.Attributes;
using Prism.Ioc;
using System;
using System.Linq;
using System.Reflection;

namespace Plum.Windows.Mvvm
{
    /// <summary>
    /// 注册导航页
    /// </summary>
    public static class ModuleExtensions
    {
        public static void RegisterNavigations(this IContainerRegistry containerRegistry, Assembly assembly)
        {
            var types = assembly.GetTypes();

            //var query = from type in types
            //            where !type.IsAbstract
            //                && !type.IsGenericTypeDefinition
            //                && type.GetCustomAttribute<NavigableAttribute>() != null
            //            select type;
            var query = from type in types
                        where !type.IsAbstract && !type.IsGenericTypeDefinition
                        let attr = type.GetCustomAttribute<NavigableAttribute>()
                        where attr != null
                        select new NavigableResult(type, attr.Name.IsNullOrEmpty() ? type.FullName : attr.Name);
            foreach (var result in query)
            {
                containerRegistry.RegisterForNavigation(result.Type, result.Name);
            }
        }
    }

    public class NavigableResult
    {
        public Type Type { get; set; }

        public string Name { get; set; }

        public NavigableResult(Type type, string name)
        {
            Type = type;
            Name = name;
        }
    }
}