using LocalJsonStore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Reflection;

namespace LocalJsonStoreUnitTests.AuthServiceUnitTests
{
    [TestClass]
    public class HashPasswordUnitTests
    {
        public IAuthService AuthenticationService { get; set; }

        public HashPasswordUnitTests()
        {
            AuthenticationService = new AuthService();
        }

        [TestMethod]
        public void HashedPasswordsMatch()
        {
            var m = MethodBase.GetCurrentMethod();
            var msg = "TEST_FAILED." + m.ReflectedType.Name + "." + m.Name + ": ";

            var password1 = "Password1234";
            var password2 = "Password1234";
            var password1Hash = AuthenticationService.HashPassword(password1);
            var password2Hash = AuthenticationService.HashPassword(password2);
            Assert.AreEqual(password1Hash, password2Hash, msg + "hashed passwords do not match");
        }

        [TestMethod]
        public void HashedPasswordsDontMatch()
        {
            var m = MethodBase.GetCurrentMethod();
            var msg = "TEST_FAILED." + m.ReflectedType.Name + "." + m.Name + ": ";

            var password1 = "Password1234";
            var password2 = "Password1233";
            var password1Hash = AuthenticationService.HashPassword(password1);
            var password2Hash = AuthenticationService.HashPassword(password2);
            Assert.AreNotEqual(password1Hash, password2Hash, msg + "hashed passwords do match");
        }

        [TestMethod]
        public void HashedPasswordsAppearUnique()
        {
            var m = MethodBase.GetCurrentMethod();
            var msg = "TEST_FAILED." + m.ReflectedType.Name + "." + m.Name + ": ";

            var passwords = new List<string>()
            {
                "a",
                "aa",
                "aaa",
                "aaaa",
                "A",
                "AA",
                "AAA",
                "AAAA",
                "b",
                "bb",
                "B",
                "BB",
                "ABA",
                "ABAa",
                "BABa",
                "ABAA",
                "password",
                "password123",
                "passworD",
                "hello",
                "Hello"
            };

            for (var i = 0; i < passwords.Count; i++)
            {
                var passHash1 = AuthenticationService.HashPassword(passwords[i]);
                for (var j = i + 1; j < passwords.Count; j++)
                {
                    var passHash2 = AuthenticationService.HashPassword(passwords[j]);
                    Assert.AreNotEqual(passHash1, passHash2, msg + "hashed passwords " + passwords[i] + "[" + passHash1 + "] and " + passwords[j] + "[" + passHash2 + "] should be unique");
                }
            }
        }
    }
}
