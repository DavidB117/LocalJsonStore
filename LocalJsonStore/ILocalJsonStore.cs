using System.Collections.Generic;

namespace LocalJsonStore
{
    public interface ILocalJsonStore<T>
    {
        bool BackUpLocalJsonStore(string targetPath = null);

        bool DoesFileExist(string fileName);
        bool DoesFileExist(string subDirectoryName, string fileName);

        List<string> GetAllFileNames();
        List<string> GetAllFileNames(string subDirectoryName);

        bool CreateFile(string fileName, T data);
        bool CreateFile(string subDirectoryName, string fileName, T data);
        T ReadFile(string fileName);
        T ReadFile(string subDirectoryName, string fileName);
        bool UpdateFile(string fileName, T data);
        bool UpdateFile(string subDirectoryName, string fileName, T data);
        bool DeleteFile(string fileName);
        bool DeleteFile(string subDirectoryName, string fileName);

        bool DoesSubDirectoryExist(string subDirectoryName);
        List<string> GetAllSubDirectories();
        List<string> GetAllSubDirectoryPaths();
        string GetSubDirectoryPath(string subDirectoryName);
        bool CreateSubDirectory(string subDirectoryName);
        bool DeleteSubDirectory(string subDirectoryName);
    }
}
