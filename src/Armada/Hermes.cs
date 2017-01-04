using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Armada
{
    public static class Hermes
    {
        internal static IFileSystemOperations _fileSystem = new FileSystemOperations();

        public static string GetConfigFilePath(string configName)
        {
            var configPath = Environment.GetEnvironmentVariable("CONFIG_PATH");

            if (string.IsNullOrWhiteSpace(configPath))
                return null;

            foreach (var directoryPath in configPath.Split(Path.PathSeparator))
            {
                var probableFullConfigPath = Path.Combine(directoryPath, configName);

                if (_fileSystem.FileExists(probableFullConfigPath) ||
                    _fileSystem.DirectoryExists(probableFullConfigPath))
                    return probableFullConfigPath;
            }

            return null;
        }

        public static IHermesConfiguration GetConfig(string configName)
        {
            var filePath = GetConfigFilePath(configName);

            if (_fileSystem.FileExists(filePath) == false)
                return null;

            var fileContent = File.ReadAllText(filePath);
            var fileExtension = Path.GetExtension(filePath);

            switch (fileExtension)
            {
                case ".json":
                    return HermesConfiguration.ParseJson(fileContent);
                default:
                    throw new ArgumentException(string.Format("Unsupported file type: '{0}'.", fileExtension), nameof(configName));
            }
        }

        public static Dictionary<string, IHermesConfiguration> GetConfigs(string directoryName)
        {
            var directoryPath = GetConfigFilePath(directoryName);

            if (_fileSystem.DirectoryExists(directoryPath) == false)
                return null;
            
            var result = new Dictionary<string, IHermesConfiguration>();

            foreach(var configName in Directory.EnumerateFiles(directoryPath).Select(Path.GetFileName))
            {
                result[configName] = GetConfig(Path.Combine(directoryName, configName));
            }

            return result;
        }
    }
}
