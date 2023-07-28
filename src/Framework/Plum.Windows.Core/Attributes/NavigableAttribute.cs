using System;

namespace Plum.Windows.Attributes
{
    public class NavigableAttribute : Attribute
    {
        public string Name { get; set; }

        public NavigableAttribute()
        {
        }

        public NavigableAttribute(string name)
        {
            Name = name;
        }
    }
}