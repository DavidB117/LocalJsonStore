﻿using LocalJsonStore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace LocalJsonStoreUnitTests.LocalJsonStoreUnitTests.Constructors
{
    public class TestUser
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    [TestClass]
    public class DefaultConstructor
    {
        public ILocalJsonStore<TestUser> Ljs { get; set; }

        public DefaultConstructor()
        {
            Ljs = new LocalJsonStore<TestUser>();
        }

        [TestMethod]
        public void CurrentDirectoryInitialized()
        {
            var msg = "TEST_FAILED." + MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name + ": ";
            Assert.IsTrue(Ljs.CurrentDirectory == Directory.GetCurrentDirectory() + "\\", msg + "CurrentDirectory [" + Ljs.CurrentDirectory + "][" + Directory.GetCurrentDirectory() + "] not initialized with default constructor");
        }

        [TestMethod]
        public void DataDirectoryInitialized()
        {
            var msg = "TEST_FAILED." + MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name + ": ";
            var ljsDir = Ljs.DataDirectory;
            var curDir = Directory.GetCurrentDirectory() + "\\" + Process.GetCurrentProcess().ProcessName + "_Data" + "\\";
            Assert.IsTrue(ljsDir == curDir, msg + "DataDirectory [" + ljsDir + "][" + curDir + "] not initialized with default constructor");
        }

        [TestMethod]
        public void SubDirectoriesInitialized()
        {
            var msg = "TEST_FAILED." + MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name + ": ";
            var ljsSubDir = Ljs.SubDirectories;
            var emptyList = new List<string>();
            Assert.IsTrue(ljsSubDir.Count == emptyList.Count, msg + "SubDirectories [" + ljsSubDir.Count + "][" + emptyList.Count + "] not initialized with default constructor");
        }
    }
}
