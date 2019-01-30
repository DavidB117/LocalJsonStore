using LocalJsonStore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace LocalJsonStoreUnitTests.LocalJsonStoreUnitTests.Constructors
{
    [TestClass]
    public class TwoArgConstructor
    {
        [TestMethod]
        public void DefaultSubDirectories()
        {
            var msg = "TEST_FAILED." + MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name + ": ";

            var ljs = new LocalJsonStore<TestUser>();

            Assert.IsTrue(ljs.SubDirectories != null, msg + "sub directories not init");
            Assert.IsTrue(ljs.SubDirectories.Count == new List<string>().Count, msg + "sub directories count not zero");
        }

        [TestMethod]
        public void CustomSubDirectories()
        {
            var msg = "TEST_FAILED." + MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name + ": ";

            var subDir = new List<string>()
            {
                "Directory1",
                "Users",
                "Products",
                "Brands",
                "Directory2"
            };

            var ljs = new LocalJsonStore<TestUser>(string.Empty, subDir);

            Assert.AreEqual(subDir.Count, ljs.SubDirectories.Count, msg + "the sub directory list count and the local json store sub directory list count are not equal");
            Assert.AreEqual(subDir, ljs.SubDirectories, msg + "the sub directory list and the local json store sub directory list are not equal");
            for (var i = 0; i < subDir.Count && i < ljs.SubDirectories.Count; i++)
            {
                Assert.AreEqual(subDir[i], ljs.SubDirectories[i], msg + "the sub dir ele [" + subDir[i] + "] is not equal to the ljs.SubDirectories ele [" + ljs.SubDirectories[i] + "]");
            }
        }

        [TestMethod]
        public void SetSubDirectories()
        {
            var msg = "TEST_FAILED." + MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name + ": ";

            var ljs = new LocalJsonStore<TestUser>(string.Empty, new List<string>()
            {
                "Directory1",
                "Users",
                "Products",
                "Brands",
                "Directory2"
            });

            var newSubDir = new List<string>()
            {
                "Directory12",
                "Users",
                "MyProducts",
                "MyBrands",
                "Directory2"
            };

            ljs.SubDirectories = newSubDir;

            Assert.AreEqual(newSubDir.Count, ljs.SubDirectories.Count, msg + "the sub directory list count and the local json store sub directory list count are not equal");
            Assert.AreEqual(newSubDir, ljs.SubDirectories, msg + "the sub directory list and the local json store sub directory list are not equal");
            for (var i = 0; i < newSubDir.Count && i < ljs.SubDirectories.Count; i++)
            {
                Assert.AreEqual(newSubDir[i], ljs.SubDirectories[i], msg + "the sub dir ele [" + newSubDir[i] + "] is not equal to the ljs.SubDirectories ele [" + ljs.SubDirectories[i] + "]");
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetSubDirectoriesToNull1()
        {
#pragma warning disable IDE0017 // Simplify object initialization
            var x = new LocalJsonStore<TestUser>();
#pragma warning restore IDE0017 // Simplify object initialization
            x.SubDirectories = null;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetSubDirectoriesToNull2()
        {
            var x = new LocalJsonStore<TestUser>
            {
                SubDirectories = null
            };
        }
    }
}
