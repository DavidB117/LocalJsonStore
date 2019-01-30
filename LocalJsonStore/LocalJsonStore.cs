using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace LocalJsonStore
{
    public class LocalJsonStore<T> : ILocalJsonStore<T>
    {
        #region Constants
        private const string FILE_TYPE = ".json";
        private const string DOUBLE_BACK_SLASH = "\\";
        private const string DEFAULT_DATA_FOLDER_NAME_SUFIX = "_Data";
        private const string DEFAULT_BACKUP_FOLDER_NAME_SUFIX = "_DataBackup";
        #endregion

        #region PrivateProperties
        private readonly Dictionary<string, string> _dataDirectorySubDirectories; // key = directory name, value = directory path
        private string _defaultDataFolderName { get; }
        private string _defaultBackupFolderName { get; }
        #endregion

        #region PublicProperties
        public string CurrentDirectory { get; }
        public string DataDirectory { get; }
        private List<string> _subDirectories;
        public List<string> SubDirectories
        {
            get
            {
                return _subDirectories;
            }
            set
            {
                _subDirectories = value ?? throw new ArgumentNullException();

                // remove all _dataDirectorySubDirectories thats keys are not in SubDirectories list
                for (var i = _dataDirectorySubDirectories.Keys.Count - 1; i >= 0; i--)
                {
                    if (!SubDirectories.Contains(_dataDirectorySubDirectories[_dataDirectorySubDirectories.Keys.ElementAt(i)]))
                    {
                        DeleteSubDirectory(_dataDirectorySubDirectories.Keys.ElementAt(i));
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
            _dataDirectorySubDirectories = new Dictionary<string, string>();
            _defaultDataFolderName = Process.GetCurrentProcess().ProcessName + DEFAULT_DATA_FOLDER_NAME_SUFIX;
            _defaultBackupFolderName = Process.GetCurrentProcess().ProcessName + DEFAULT_BACKUP_FOLDER_NAME_SUFIX;

            CurrentDirectory = Directory.GetCurrentDirectory() + DOUBLE_BACK_SLASH;
            DataDirectory = CurrentDirectory + ((!string.IsNullOrWhiteSpace(dataDirectory)) ? dataDirectory : _defaultDataFolderName) + DOUBLE_BACK_SLASH;
            Directory.CreateDirectory(DataDirectory);
            SubDirectories = subDirectories ?? new List<string>();
        }
        #endregion

        #region GeneralMethods
        public bool BackUpLocalJsonStore(string targetPath = null, bool overwrite = true)
        {
            if (string.IsNullOrWhiteSpace(targetPath))
            {
                targetPath = CurrentDirectory + DOUBLE_BACK_SLASH + _defaultBackupFolderName + DOUBLE_BACK_SLASH;
            }
            else
            {
                targetPath += DOUBLE_BACK_SLASH + _defaultBackupFolderName + DOUBLE_BACK_SLASH;
            }

            if (!Directory.Exists(targetPath)) Directory.CreateDirectory(targetPath);

            foreach (var sourceFilePath in Directory.GetFiles(DataDirectory))
            {
                File.Copy(sourceFilePath, Path.Combine(targetPath, Path.GetFileName(sourceFilePath)), overwrite);
            }

            foreach (var sourceDirectoryPath in Directory.GetDirectories(DataDirectory))
            {
                File.Copy(sourceDirectoryPath, Path.Combine(targetPath, Path.GetDirectoryName(sourceDirectoryPath)), overwrite);
            }

            return true;
        }

        public override string ToString()
        {
            const string TAB = "\t";
            const string CHEVRON = "> ";
            const string FILE = " (file)";
            const string DIRECTORY = " (directory)";
            var str = string.Empty;
            str += Path.GetDirectoryName(DataDirectory) + DIRECTORY + Environment.NewLine;
            foreach (var x in GetAllFileNames()) str += TAB + x + FILE + Environment.NewLine;
            foreach (var x in GetAllSubDirectories())
            {
                str += TAB + CHEVRON + x + DIRECTORY + Environment.NewLine;
                foreach (var y in GetAllFileNames(x))
                {
                    str += TAB + TAB + y + FILE + Environment.NewLine;
                }
            }
            return str;
        }
        #endregion

        #region FileMethods
        public bool DoesFileExist(string fileName)
        {
            return File.Exists(DataDirectory + fileName + FILE_TYPE);
        }

        public bool DoesFileExist(string subDirectoryName, string fileName)
        {
            return File.Exists(GetSubDirectoryPath(subDirectoryName) + fileName + FILE_TYPE);
        }
        
        public List<string> GetAllFileNames()
        {
            return _getAllFileNames(Directory.GetFiles(DataDirectory).ToList());
        }

        public List<string> GetAllFileNames(string subDirectoryName)
        {
            return _getAllFileNames(Directory.GetFiles(DataDirectory + subDirectoryName + DOUBLE_BACK_SLASH).ToList());
        }
        
        public bool CreateFile(string fileName, T data)
        {
            return _createFile(DataDirectory + fileName + FILE_TYPE, data);
        }

        public bool CreateFile(string subDirectoryName, string fileName, T data)
        {
            return _createFile(GetSubDirectoryPath(subDirectoryName) + fileName + FILE_TYPE, data);
        }

        public T ReadFile(string fileName)
        {
            return _readFile(DataDirectory + fileName + FILE_TYPE);
        }

        public T ReadFile(string subDirectoryName, string fileName)
        {
            return _readFile(GetSubDirectoryPath(subDirectoryName) + fileName + FILE_TYPE);
        }

        public bool UpdateFile(string fileName, T data)
        {
            return _updateFile(DataDirectory + fileName + FILE_TYPE, data);
        }

        public bool UpdateFile(string subDirectoryName, string fileName, T data)
        {
            return _updateFile(GetSubDirectoryPath(subDirectoryName) + fileName + FILE_TYPE, data);
        }

        public bool DeleteFile(string fileName)
        {
            return _deleteFile(DataDirectory + fileName + FILE_TYPE);
        }

        public bool DeleteFile(string subDirectoryName, string fileName)
        {
            return _deleteFile(GetSubDirectoryPath(subDirectoryName) + fileName + FILE_TYPE);
        }


        
        private List<string> _getAllFileNames(List<string> filePaths)
        {
            var fileNames = new List<string>();
            foreach (var filePath in filePaths)
            {
                fileNames.Add(Path.GetFileNameWithoutExtension(filePath));
            }
            return fileNames;
        }
        private bool _createFile(string fullPath, T data)
        {
            if (!File.Exists(fullPath))
            {
                using (var sw = File.CreateText(fullPath))
                {
                    sw.WriteLine(JsonConvert.SerializeObject(data, Formatting.Indented));
                    return true;
                }
            }
            return false;
        }
        private T _readFile(string fullPath)
        {
            T myObj = default(T); // default of object being serialized
            if (File.Exists(fullPath))
                myObj = JsonConvert.DeserializeObject<T>(File.ReadAllText(fullPath));
            else
                throw new FileNotFoundException();
            return myObj;
        }
        private bool _updateFile(string fullPath, T data)
        {
            if (File.Exists(fullPath))
            {
                File.WriteAllText(fullPath, string.Empty);
                using (var sw = File.CreateText(fullPath))
                {
                    sw.WriteLine(JsonConvert.SerializeObject(data, Formatting.Indented));
                    return true;
                }
            }
            return false;
        }
        private bool _deleteFile(string fullPath)
        {
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return true;
            }
            return false;
        }
        #endregion

        #region SubDirectoryMethods
        public bool DoesSubDirectoryExist(string subDirectoryName)
        {
            return _dataDirectorySubDirectories.ContainsKey(subDirectoryName);
        }

        public List<string> GetAllSubDirectories()
        {
            return _dataDirectorySubDirectories.Keys.ToList();
        }

        public List<string> GetAllSubDirectoryPaths()
        {
            return _dataDirectorySubDirectories.Values.ToList();
        }

        public string GetSubDirectoryPath(string subDirectoryName)
        {
            var path = string.Empty;
            if (DoesSubDirectoryExist(subDirectoryName))
                path = _dataDirectorySubDirectories[subDirectoryName];
            else
                throw new DirectoryNotFoundException();
            return path;
        }

        public bool CreateSubDirectory(string subDirectoryName)
        {
            if (!DoesSubDirectoryExist(subDirectoryName))
            {
                var path = DataDirectory + subDirectoryName + DOUBLE_BACK_SLASH;
                Directory.CreateDirectory(path);
                _dataDirectorySubDirectories.Add(subDirectoryName, path);
                SubDirectories.Add(subDirectoryName);
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
                SubDirectories.Remove(subDirectoryName);
                return true;
            }
            else
            {
                throw new DirectoryNotFoundException(MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name + ": " +
                    subDirectoryName + " directory was not found");
            }
        }
        #endregion
    }
}
