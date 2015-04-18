using System;
using System.IO;
using System.Reflection;
using Sandbox.Contracts;

namespace Sandbox.Environment.Configuration
{
    class EnvironmentPath
    {
        public static string GetExtensionsDirectory(PlatformType type)
        {
            string path = Path.Combine(AssemblyDirectory, "extensions", GetPlatformName(type));
            return path;
        }

        public static string GetUserDirectory(PlatformType type)
        {
            string path = Path.Combine(AssemblyDirectory, "user", GetPlatformName(type));
            return path;
        }

        public static string GetPackageDirectory(PlatformType type, string packageName)
        {
            return Path.Combine(GetUserDirectory(type), packageName);
        }

        public static string GetTemporaryDirectory(PlatformType type, string packageName)
        {
            return Path.Combine(GetPackageDirectory(type, packageName), "tmp");
        }

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        static string GetPlatformName(PlatformType type)
        {
            return type.ToString().ToLower();
        }
    }
}
