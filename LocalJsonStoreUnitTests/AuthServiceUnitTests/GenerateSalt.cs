using LocalJsonStore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace LocalJsonStoreUnitTests.AuthServiceUnitTests
{
    [TestClass]
    public class GenerateSalt
    {
        private const int NUMBER_OF_SALTS_TO_GENERATE = 100;

        public IAuthService AuthenticationService { get; set; }

        public GenerateSalt()
        {
            AuthenticationService = new AuthService();
        }

        [TestMethod]
        public void ReturnsValue()
        {
            var m = MethodBase.GetCurrentMethod();
            var msg = "TEST_FAILED." + m.ReflectedType.Name + "." + m.Name + ": ";

            var s = AuthenticationService.GenerateSalt();
            Assert.IsTrue(!string.IsNullOrWhiteSpace(s), msg + "salt does not return a value [" + s + "]");
        }

        [TestMethod]
        public void SaltLengthWithinDefaultRange()
        {
            var m = MethodBase.GetCurrentMethod();
            var msg = "TEST_FAILED." + m.ReflectedType.Name + "." + m.Name + ": ";

            for (var i = 0; i < NUMBER_OF_SALTS_TO_GENERATE; i++)
            {
                var s = AuthenticationService.GenerateSalt();
                Assert.IsTrue(s.Length >= AuthenticationService.DEFAULT_SALT_LENGTH_MIN, msg + "salt length [" + s.Length + "] is less than the default minimum [" + AuthenticationService.DEFAULT_SALT_LENGTH_MIN + "] it should be");
                Assert.IsTrue(s.Length <= AuthenticationService.DEFAULT_SALT_LENGTH_MAX, msg + "salt length [" + s.Length + "] is greater than the default maximum [" + AuthenticationService.DEFAULT_SALT_LENGTH_MAX + "] it should be");
            }
        }

        [TestMethod]
        public void SaltLengthWithinRandomRanges()
        {
            var m = MethodBase.GetCurrentMethod();
            var msg = "TEST_FAILED." + m.ReflectedType.Name + "." + m.Name + ": ";

            var r = new Random();
            for (var i = 0; i < NUMBER_OF_SALTS_TO_GENERATE; i++)
            {
                var min = r.Next(1, 10);
                var max = r.Next(11, 20);
                var s = AuthenticationService.GenerateSalt(min, max);
                Assert.IsTrue(s.Length >= min, msg + "salt length [" + s.Length + "] is less than the minimum [" + min + "] it should be");
                Assert.IsTrue(s.Length <= max, msg + "salt length [" + s.Length + "] is greater than the maximum [" + max + "] it should be");
            }
        }

        [TestMethod]
        public void SaltOnlyContainsDefaultCharacters()
        {
            var m = MethodBase.GetCurrentMethod();
            var msg = "TEST_FAILED." + m.ReflectedType.Name + "." + m.Name + ": ";

            for (var i = 0; i < NUMBER_OF_SALTS_TO_GENERATE; i++)
            {
                var s = AuthenticationService.GenerateSalt();
                foreach (var c in s)
                {
                    Assert.IsTrue(AuthenticationService.DEFAULT_CHARACTERS.Contains(c.ToString()), msg + "salt using default character set [" + AuthenticationService.DEFAULT_CHARACTERS + "]contains non-default character " + c.ToString());
                }
            }
        }

        [TestMethod]
        public void SaltOnlyContainsPseudoRandomCharacters()
        {
            var m = MethodBase.GetCurrentMethod();
            var msg = "TEST_FAILED." + m.ReflectedType.Name + "." + m.Name + ": ";

            var pseudoRandomChars = new List<string>()
            {
                "POSDFKalgjui14790",
                "poiuytrewqasdfghbn75432MNCBZXTQWYDF",
                "ABCDEFGHIJK1290678hdjskalzxcvb"
            };

            foreach (var rc in pseudoRandomChars)
            {
                var s = AuthenticationService.GenerateSalt(AuthenticationService.DEFAULT_SALT_LENGTH_MIN, AuthenticationService.DEFAULT_SALT_LENGTH_MAX, rc);
                foreach (var c in s)
                {
                    Assert.IsTrue(rc.Contains(c.ToString()), msg + "salt in random character set [" + rc + "] does not contain " + c.ToString());
                }
            }
        }
    }
}
