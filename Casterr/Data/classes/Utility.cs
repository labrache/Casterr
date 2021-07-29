using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Casterr.Data.classes
{
    public static class Utility
    {
        public static List<String> imageExtList = new List<string> { ".jpg", ".jpeg", ".jfif", ".pjpeg", ".pjp", ".png", ".apng", ".gif", ".svg", ".webp" };
        public static String humanReadableSize(double len)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return String.Format("{0:0.##} {1}", len, sizes[order]);
        }
        public static string GetFileExtensionFromUrl(string url)
        {
            url = url.Split('?')[0];
            url = url.Split('/').Last();
            return url.Contains('.') ? url.Substring(url.LastIndexOf('.')).ToLower() : "";
        }
    }
}
