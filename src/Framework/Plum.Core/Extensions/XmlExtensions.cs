using Plum.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Plum
{
    public class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }

    public static class XmlExtensions
    {
        public static void SaveToXml<T>(this T @object, string path)
        {
            DirectoryHelper.CreateIfNotExists(Path.GetDirectoryName(path));

            var xml = @object.ToXml();
            using (StreamWriter stream_writer = new StreamWriter(path))
            {
                stream_writer.Write(xml);
            }
        }

        public static T LoadFromXml<T>(this string path)
        {
            if (!File.Exists(path))
                return default;

            var x = new XmlSerializer(typeof(T));
            using (var reader = new StreamReader(path))
            {
                return (T)x.Deserialize(reader);
            }
        }

        public static object LoadFromXml(this string path, Type type)
        {
            if (!File.Exists(path))
                return default;

            var x = new XmlSerializer(type);
            using (var reader = new StreamReader(path))
            {
                return x.Deserialize(reader);
            }
        }

        public static string ToXml<T>(this T @object)
        {
            var xmlSerializer = new XmlSerializer(@object.GetType());

            using (var stringWriter = new Utf8StringWriter())
            {
                xmlSerializer.Serialize(stringWriter, @object);
                using (var streamReader = new StringReader(stringWriter.GetStringBuilder().ToString()))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }

        public static T FromXml<T>(this string xml)
        {
            var x = new XmlSerializer(typeof(T));
            var r = new StringReader(xml);

            return (T)x.Deserialize(r);
        }

        public static object FromXml(this string xml, Type type)
        {
            var x = new XmlSerializer(type);
            var r = new StringReader(xml);

            return x.Deserialize(r);
        }
    }
}