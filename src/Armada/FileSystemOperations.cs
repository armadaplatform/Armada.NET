using System;
using System.IO;

namespace Armada
{
    public interface IFileSystemOperations
    {
        bool FileExists(string path);
        bool DirectoryExists(string path);
    }

    public class FileSystemOperations : IFileSystemOperations
    {
        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public bool FileExists(string path)
        {
            return File.Exists(path);
        }
    }
}