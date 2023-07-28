using System;
using System.Collections.Generic;

namespace Plum.Tools
{
    public class MimeTool
    {
        private static IDictionary<string, string> _mappings = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase) {

        #region Big freaking list of mime types

        // combination of values from Windows 7 Registry and

        // from C:\Windows\System32\inetsrv\config\applicationHost.config

        // some added, including .7z and .dat

        {".3gp", "video/3gpp"},

        {".3gpp", "video/3gpp"},

        {".7z", "application/x-7z-compressed"},

        {".accdb", "application/msaccess"},

        {".acx", "application/internet-property-stream"},

        {".ade", "application/msaccess"},

        {".ai", "application/postscript"},

        {".avi", "video/x-msvideo"},

        {".bmp", "image/bmp"},

        {".cd", "text/plain"},

        {".chm", "application/octet-stream"},

        {".class", "application/x-java-applet"},

        {".cod", "image/cis-cod"},

        {".cpp", "text/plain"},

        {".cs", "text/plain"},

        {".csdproj", "text/plain"},

        {".csproj", "text/plain"},

        {".css", "text/css"},

        {".csv", "text/csv"},

        {".dat", "application/octet-stream"},

        {".datasource", "application/xml"},

        {".dbproj", "text/plain"},

        {".dll", "application/x-msdownload"},

        {".dll.config", "text/xml"},

        {".doc", "application/msword"},

        {".docm", "application/vnd.ms-word.document.macroEnabled.12"},

        {".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},

        {".dot", "application/msword"},

        {".dotm", "application/vnd.ms-word.template.macroEnabled.12"},

        {".dotx", "application/vnd.openxmlformats-officedocument.wordprocessingml.template"},

        {".dwf", "drawing/x-dwf"},

        {".dwp", "application/octet-stream"},

        {".exe", "application/octet-stream"},

        {".exe.config", "text/xml"},

        {".gif", "image/gif"},

        {".gz", "application/x-gzip"},

        {".h", "text/plain"},

        {".hlp", "application/winhlp"},

        {".hpp", "text/plain"},

        {".htc", "text/x-component"},

        {".htm", "text/html"},

        {".html", "text/html"},

        {".ico", "image/x-icon"},

        {".inc", "text/plain"},

        {".jar", "application/java-archive"},

        {".java", "application/octet-stream"},

        {".jpe", "image/jpeg"},

        {".jpeg", "image/jpeg"},

        {".jpg", "image/jpeg"},

        {".js", "application/x-javascript"},

        {".jsx", "text/jscript"},

        {".m2v", "video/mpeg"},

        {".m3u", "audio/x-mpegurl"},

        {".m3u8", "audio/x-mpegurl"},

        {".m4a", "audio/m4a"},

        {".m4b", "audio/m4b"},

        {".m4p", "audio/m4p"},

        {".manifest", "application/x-ms-manifest"},

        {".map", "text/plain"},

        {".master", "application/xml"},

        {".mdb", "application/x-msaccess"},

        {".mde", "application/msaccess"},

        {".me", "application/x-troff-me"},

        {".mov", "video/quicktime"},

        {".movie", "video/x-sgi-movie"},

        {".mp2", "video/mpeg"},

        {".mp2v", "video/mpeg"},

        {".mp3", "audio/mpeg"},

        {".mp4", "video/mp4"},

        {".mp4v", "video/mp4"},

        {".mpeg", "video/mpeg"},

        {".msi", "application/octet-stream"},

        {".pdf", "application/pdf"},

        {".png", "image/png"},

        {".ppt", "application/vnd.ms-powerpoint"},

        {".pptm", "application/vnd.ms-powerpoint.presentation.macroEnabled.12"},

        {".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation"},

        {".ps", "application/postscript"},

        {".psd", "application/octet-stream"},

        {".rgb", "image/x-rgb"},

        {".rgs", "text/plain"},

        {".rm", "application/vnd.rn-realmedia"},

        {".settings", "application/xml"},

        {".swf", "application/x-shockwave-flash"},

        {".tgz", "application/x-compressed"},

        {".tif", "image/tiff"},

        {".tiff", "image/tiff"},

        {".txt", "text/plain"},

        {".vcproj", "Application/xml"},

        {".vsd", "application/vnd.visio"},

        {".vsi", "application/ms-vsi"},

        {".vsix", "application/vsix"},

        {".wav", "audio/wav"},

        {".wave", "audio/wav"},

        {".xhtml", "application/xhtml+xml"},

        {".xlc", "application/vnd.ms-excel"},

        {".xld", "application/vnd.ms-excel"},

        {".xlk", "application/vnd.ms-excel"},

        {".xll", "application/vnd.ms-excel"},

        {".xlm", "application/vnd.ms-excel"},

        {".xls", "application/vnd.ms-excel"},

        {".xlsb", "application/vnd.ms-excel.sheet.binary.macroEnabled.12"},

        {".xlsm", "application/vnd.ms-excel.sheet.macroEnabled.12"},

        {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},

        {".xlt", "application/vnd.ms-excel"},

        {".xltm", "application/vnd.ms-excel.template.macroEnabled.12"},

        {".xltx", "application/vnd.openxmlformats-officedocument.spreadsheetml.template"},

        {".xlw", "application/vnd.ms-excel"},

        {".xml", "text/xml"},

        {".xsd", "text/xml"},

        {".xsf", "text/xml"},

        {".xsl", "text/xml"},

        {".xslt", "text/xml"},

        {".zip", "application/x-zip-compressed"},

        #endregion Big freaking list of mime types
        };

        /// <summary>
        /// 根据文件扩展名获取 Mime 类型
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static string Get(string extension)
        {
            if (extension == null)

            {
                throw new ArgumentNullException("extension");
            }

            if (!extension.StartsWith("."))

            {
                extension = "." + extension;
            }

            string mime;

            return _mappings.TryGetValue(extension, out mime) ? mime : "application/octet-stream";
        }
    }
}