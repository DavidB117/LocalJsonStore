using LocalJsonStore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace LocalJsonStoreUnitTests.AuthServiceUnitTests
{
    [TestClass]
    public class GenerateSaltUnitTests
    {
        #region Constants
        // copied over from the AuthService.cs file
        private const int DEFAULT_SALT_LENGTH_MIN = 6;
        private const int DEFAULT_SALT_LENGTH_MAX = 10;
        private const string CHARACTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        // specific to this unit test file
        private const int NUMBER_OF_SALTS_TO_GENERATE = 100;
        private const string TEST_FAILED = "TEST_FAILED.";
        #endregion

        #region Properties
        public IAuthService AuthService { get; set; }
        #endregion

        #region Constructor
        public GenerateSaltUnitTests()
        {
            AuthService = new AuthService();
        }
        #endregion

        #region Methods
        [TestMethod]
        public void ReturnsValue()
        {
            var m = MethodBase.GetCurrentMethod();
            var msg = TEST_FAILED + m.ReflectedType.Name + "." + m.Name + ": ";

            var s = AuthService.GenerateSalt();
            Assert.IsTrue(!string.IsNullOrWhiteSpace(s), msg + "salt does not return a value");
        }

        [TestMethod]
        public void SaltLengthWithinDefaultRange()
        {
            var m = MethodBase.GetCurrentMethod();
            var msg = TEST_FAILED + m.ReflectedType.Name + "." + m.Name + ": ";

            for (var i = 0; i < NUMBER_OF_SALTS_TO_GENERATE; i++)
            {
                var s = AuthService.GenerateSalt();
                Assert.IsTrue(s.Length >= DEFAULT_SALT_LENGTH_MIN, msg + "salt length is less than the minimum it should be");
                Assert.IsTrue(s.Length <= DEFAULT_SALT_LENGTH_MAX, msg + "salt length is greater than the maximum it should be");
            }
        }

        [TestMethod]
        public void SaltLengthWithinRandomRanges()
        {
            var m = MethodBase.GetCurrentMethod();
            var msg = TEST_FAILED + m.ReflectedType.Name + "." + m.Name + ": ";

            var r = new Random();
            for (var i = 0; i < NUMBER_OF_SALTS_TO_GENERATE; i++)
            {
                var min = r.Next();
                var max = r.Next();
                if (min > max)
                {
                    var temp = min;
                    min = max;
                    max = temp;
                }

                var s = AuthService.GenerateSalt(min, max);
                Assert.IsTrue(s.Length >= min, msg + "salt length is less than the minimum it should be");
                Assert.IsTrue(s.Length <= max, msg + "salt length is greater than the maximum it should be");
            }
        }

        [TestMethod]
        public void SaltOnlyContainsDefaultCharacters()
        {
            var m = MethodBase.GetCurrentMethod();
            var msg = TEST_FAILED + m.ReflectedType.Name + "." + m.Name + ": ";

            for (var i = 0; i < NUMBER_OF_SALTS_TO_GENERATE; i++)
            {
                var s = AuthService.GenerateSalt();
                foreach (var c in s)
                {
                    Assert.IsTrue(CHARACTERS.Contains(c.ToString()), msg + "salt using default character set contains non-default characters");
                }
            }
        }

        [TestMethod]
        public void SaltOnlyContainsRandomCharacters()
        {
            var m = MethodBase.GetCurrentMethod();
            var msg = TEST_FAILED + m.ReflectedType.Name + "." + m.Name + ": ";

            var randomChars = new List<string>()
            {
                "POSDFKalgjui14790",
                "poiuytrewqasdfghbn75432MNCBZXTQWYDF",
                "ABCDEFGHIJK1290678hdjskalzxcvb"
            };

            foreach (var rc in randomChars)
            {
                var s = AuthService.GenerateSalt(DEFAULT_SALT_LENGTH_MIN, DEFAULT_SALT_LENGTH_MAX, rc);
                foreach (var c in s)
                {
                    Assert.IsTrue(rc.Contains(c.ToString()), msg + "salt in random character set [" + rc + "] does not contain " + c.ToString());
                }
            }
        }
        #endregion
    }
}
