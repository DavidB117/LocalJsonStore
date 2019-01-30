using LocalJsonStore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace LocalJsonStoreUnitTests.LocalJsonStoreUnitTests.Constructors
{
    [TestClass]
    public class OneArgConstructor
    {
        [TestMethod]
        public void DefaultDataDirectory()
        {
            var msg = "TEST_FAILED." + MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name + ": ";
            var defDirName = Process.GetCurrentProcess().ProcessName + "_Data";
            var defDirPath = Directory.GetCurrentDirectory() + "\\" + defDirName + "\\";
            var ljs = new LocalJsonStore<TestUser>();
            var actualDirName = new DirectoryInfo(ljs.DataDirectory).Name;
            var actualDirPath = ljs.DataDirectory;
            Assert.IsTrue(defDirName == actualDirName, msg + "the expected default directory [" + defDirName + "] is different than the actual [" + actualDirName + "]");
            Assert.IsTrue(defDirPath == actualDirPath, msg + "the expected default directory path [" + defDirPath + "] is different then the actual [" + actualDirPath + "]");
        }

        [TestMethod]
        public void CustomDataDirectory()
        {
            var msg = "TEST_FAILED." + MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name + ": ";
            var cusDirName = "CustomDir_Data";
            var cusDirPath = Directory.GetCurrentDirectory() + "\\" + cusDirName + "\\";
            var ljs = new LocalJsonStore<TestUser>(cusDirName);
            var actualDirName = new DirectoryInfo(ljs.DataDirectory).Name;
            var actualDirPath = ljs.DataDirectory;
            Assert.IsTrue(cusDirName == actualDirName, msg + "the expected directory [" + cusDirName + "] is different than the actual [" + actualDirName + "]");
            Assert.IsTrue(cusDirPath == actualDirPath, msg + "the expected directory path [" + cusDirPath + "] is different then the actual [" + actualDirPath + "]");
        }
    }
}
