using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Codecool.SecureApi.Tests.Extensions
{
    public static class AssemblyExtensions
    {
        public static Stream GetEmbeddedResourceStream(this Assembly assembly, string relativeResourcePath)
        {
            if (string.IsNullOrEmpty(relativeResourcePath))
            {
                throw new ArgumentNullException("relativeResourcePath");
            }

            var resourcePath =
                $"{Regex.Replace(assembly.ManifestModule.Name, @"\.(exe|dll)$", string.Empty, RegexOptions.IgnoreCase)}.{relativeResourcePath}";

            var stream = assembly.GetManifestResourceStream(resourcePath);
            if (stream == null)
                throw new ArgumentException($"The specified embedded resource {relativeResourcePath} is not found.");
            return stream;
        }

        public static string GetEmbeddedResourceContent(this Assembly assembly, string relativeResourcePath)
        {
            using var stream = new StreamReader(assembly.GetEmbeddedResourceStream(relativeResourcePath));
            return stream.ReadToEnd();
        }
    }
}
