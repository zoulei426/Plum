using System.Xml.Serialization;

namespace Plum.Common
{
    /// <summary>
    /// XML数据字典
    /// </summary>
    public class XDictionary
    {
        [XmlAttribute("Code")]
        public string Code { get; set; } = string.Empty;

        [XmlAttribute("Name")]
        public string Name { get; set; } = string.Empty;
    }
}