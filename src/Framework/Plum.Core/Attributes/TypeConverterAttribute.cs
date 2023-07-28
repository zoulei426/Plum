using System;
using System.Collections.Generic;
using System.Reflection;

namespace Plum.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed class TypeConverterAttribute : Attribute
    {
        #region Fields

        public Type Target { get; private set; }

        #endregion Fields

        #region Properties

        #endregion Properties

        #region Ctor

        public TypeConverterAttribute(Type target)
        {
            this.Target = target;
        }

        #endregion Ctor

        #region Methods

        public static Dictionary<Type, MethodInfo> Create(Type type)
        {
            Dictionary<Type, MethodInfo> ht = new Dictionary<Type, MethodInfo>();

            MethodInfo[] listMethods = type.GetMethods();

            foreach (MethodInfo mi in listMethods)
            {
                TypeConverterAttribute[] attrs = mi.GetAttributes<TypeConverterAttribute>();

                foreach (TypeConverterAttribute attr in attrs)
                    ht[attr.Target] = mi;
            }

            return ht;
        }

        #endregion Methods
    }
}