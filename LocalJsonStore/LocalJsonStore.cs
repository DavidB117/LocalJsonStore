using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalJsonStore
{
    public class LocalJsonStore<T> : ILocalJsonStore<T>
    {
        #region Constants
        private const string FILE_TYPE = ".json";
        private const string DOUBLE_BACK_SLASH = "\\";
        private const string DEFAULT_DATA_FOLDER_NAME = "Data";
        #endregion

        #region Properties
        private readonly Dictionary<string, string> _dataDirectorySubDirectories; // key = directory name, value = directory path
        public IAuthService AuthenticationService { get; }
        public string CurrentDirectory { get; }
        public string DataDirectory { get; }
        public List<string> SubDirectories
        {
            get { return SubDirectories; }
            set
            {
                SubDirectories = value ?? throw new ArgumentNullException();

                // remove all _dataDirectorySubDirectories thats keys are not in SubDirectories list
                foreach (var x in _dataDirectorySubDirectories)
                {
                    if (!SubDirectories.Contains(x.Key))
                    {
                        _dataDirectorySubDirectories.Remove(x.Key);
                        DeleteSubDirectory(x.Key);
                    }
                }

                // add all new directories in SubDirectories to our _dataDirectorySubDirectories structure
                foreach (var dir in SubDirectories)
                {
                    if (!_dataDirectorySubDirectories.ContainsKey(dir))
                    {
                        var dirPath = DataDirectory + dir + DOUBLE_BACK_SLASH;
                        _dataDirectorySubDirectories.Add(dir, dirPath);
                    }
                }

                // create directories that do not exist
                foreach (var dir in _dataDirectorySubDirectories)
                {
                    Directory.CreateDirectory(dir.Value);
                }
            }
        }
        #endregion

        #region Constructor
        public LocalJsonStore(string dataDirectory = null, List<string> subDirectories = null)
        {
            AuthenticationService = new AuthService();
            CurrentDirectory = Directory.GetCurrentDirectory();
            DataDirectory = CurrentDirectory + DOUBLE_BACK_SLASH + ((!string.IsNullOrWhiteSpace(dataDirectory)) ? dataDirectory : DEFAULT_DATA_FOLDER_NAME) + DOUBLE_BACK_SLASH;

            // create a dictionary of directory names, and the path associated with that directory name
            _dataDirectorySubDirectories = new Dictionary<string, string>();
            foreach (var dir in SubDirectories = subDirectories ?? new List<string>())
            {
                var dirPath = DataDirectory + dir + DOUBLE_BACK_SLASH;
                _dataDirectorySubDirectories.Add(dir, dirPath);
            }

            // create all directories if they do not exist
            Directory.CreateDirectory(DataDirectory);
            foreach (var dir in _dataDirectorySubDirectories)
            {
                Directory.CreateDirectory(dir.Value);
            }
        }
        #endregion

        #region GeneralMethods
        public bool BackUpLocalJsonStore(string pathToBackupDirectory)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region FileMethods
        public bool DoesFileExist(string fileName)
        {
            return File.Exists(DataDirectory + fileName + FILE_TYPE);
        }

        public bool DoesFileExist(string subDirectoryName, string fileName)
        {
            return File.Exists(GetSubDirectoryPath(subDirectoryName) + DOUBLE_BACK_SLASH + fileName + FILE_TYPE);
        }

        public List<string> GetAllFileNames()
        {
            var fileNames = new List<string>();
            var filePaths = Directory.GetFiles(DataDirectory);
            foreach (var filePath in filePaths)
            {
                fileNames.Add(Path.GetFileNameWithoutExtension(filePath));
            }
            return fileNames;
        }

        public List<string> GetAllFileNames(string subDirectoryName)
        {
            var fileNames = new List<string>();
            var filePaths = Directory.GetFiles(DataDirectory + subDirectoryName + DOUBLE_BACK_SLASH);
            foreach (var filePath in filePaths)
            {
                fileNames.Add(Path.GetFileNameWithoutExtension(filePath));
            }
            return fileNames;
        }









        public bool CreateFile(string fileName, T data)
        {
            throw new NotImplementedException();
        }

        public bool CreateFile(string subDirectoryName, string fileName, T data)
        {
            throw new NotImplementedException();
        }

        public T ReadFile(string fileName)
        {
            throw new NotImplementedException();
        }

        public T ReadFile(string subDirectoryName, string fileName)
        {
            throw new NotImplementedException();
        }

        public bool UpdateFile(string fileName, T data)
        {
            throw new NotImplementedException();
        }

        public bool UpdateFile(string subDirectoryName, string fileName, T data)
        {
            throw new NotImplementedException();
        }

        public bool DeleteFile(string fileName)
        {
            throw new NotImplementedException();
        }

        public bool DeleteFile(string subDirectoryName, string fileName)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region DirectoryMethods
        public bool DoesSubDirectoryExist(string subDirectoryName)
        {
            return _dataDirectorySubDirectories.ContainsKey(subDirectoryName);
        }

        public List<string> GetAllSubDirectories()
        {
            throw new NotImplementedException();
        }

        public string GetSubDirectoryPath(string subDirectoryName)
        {
            var path = string.Empty;
            if (DoesSubDirectoryExist(subDirectoryName)) path = _dataDirectorySubDirectories[subDirectoryName];
            else throw new DirectoryNotFoundException();
            return path;
        }

        public bool CreateSubDirectory(string subDirectoryName)
        {
            if (!DoesSubDirectoryExist(subDirectoryName))
            {
                var path = DataDirectory + subDirectoryName + DOUBLE_BACK_SLASH;
                Directory.CreateDirectory(path);
                _dataDirectorySubDirectories.Add(subDirectoryName, path);
                return true;
            }
            return false;
        }

        public bool DeleteSubDirectory(string subDirectoryName)
        {
            if (DoesSubDirectoryExist(subDirectoryName))
            {
                Directory.Delete(GetSubDirectoryPath(subDirectoryName));
                _dataDirectorySubDirectories.Remove(subDirectoryName);
                return true;
            }
            else
            {
                throw new DirectoryNotFoundException();
            }
        }
        #endregion
    }
}
