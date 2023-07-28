using System;

namespace Plum.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DataDictionaryAttribute : Attribute
    {
        public string Code { get; set; }

        public string SuperiorPropertyName { get; set; }

        public DataDictionaryAttribute(string code)
        {
            Code = code;
        }

        public DataDictionaryAttribute(string code, string propertyName) : this(code)
        {
            SuperiorPropertyName = propertyName;
        }
    }
}