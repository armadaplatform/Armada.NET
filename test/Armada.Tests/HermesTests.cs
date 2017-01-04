using System;
using System.IO;
using Xunit;

namespace Armada.Tests
{
    public class HermesTests
    {
        public HermesTests()
        {
            Environment.SetEnvironmentVariable("CONFIG_PATH", null);
        }

        [Fact]
        public void return_null_if_CONFIG_PATH_is_not_set()
        {
            var actual = Hermes.GetConfigFilePath("config.json");

            Assert.Null(actual);
        }

        [Fact]
        public void return_null_if_none_directory_in_CONFIG_PATH_contains_file()
        {
            Environment.SetEnvironmentVariable("CONFIG_PATH", "SOME_DIRECTORY");
            Hermes._fileSystem = new FakeFileSystemOperations { DoesFileExist = false };

            var actual = Hermes.GetConfigFilePath("config.json");

            Assert.Null(actual);
        }

        [Fact]
        public void return_full_path_if_file_is_found_anywhere_in_CONFIG_PATH()
        {            
            Environment.SetEnvironmentVariable("CONFIG_PATH", "SOME_DIRECTORY");
            Hermes._fileSystem = new FakeFileSystemOperations { DoesFileExist = true };
            var expected = "SOME_DIRECTORY" + Path.DirectorySeparatorChar + "config.json";

            var actual = Hermes.GetConfigFilePath("config.json");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void return_full_path_of_first_file_if_found_in_multiple_places()
        {            
            Environment.SetEnvironmentVariable("CONFIG_PATH", string.Join(Path.PathSeparator.ToString(), new []{"SOME_DIRECTORY", "ANOTHER_DIRECTORY"}));
            Hermes._fileSystem = new FakeFileSystemOperations { DoesFileExist = true };
            var expected = "SOME_DIRECTORY" + Path.DirectorySeparatorChar + "config.json";

            var actual = Hermes.GetConfigFilePath("config.json");

            Assert.Equal(expected, actual);
        }

        private class FakeFileSystemOperations : IFileSystemOperations
        {
            public bool DoesFileExist { get; set; } = false;
            public bool DoesDirectoryExist { get; set; } = false;

            public bool DirectoryExists(string path)
            {
                return DoesDirectoryExist;
            }

            public bool FileExists(string path)
            {
                return DoesFileExist;
            }
        }
    }
}
