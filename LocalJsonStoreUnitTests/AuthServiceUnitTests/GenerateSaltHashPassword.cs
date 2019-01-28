using LocalJsonStore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LocalJsonStoreUnitTests.AuthServiceUnitTests
{
    [TestClass]
    public class GenerateSaltHashPassword
    {
        public IAuthService AuthenticationService { get; set; }

        public GenerateSaltHashPassword()
        {
            AuthenticationService = new AuthService();
        }

        [TestMethod]
        public void HashPasswordWithSalt()
        {
            var p1 = "Password1234";
            var p1Salt = AuthenticationService.GenerateSalt();
            var p1Hash = AuthenticationService.HashPassword(p1 + p1Salt);

            var p2 = "Password1234";
            var p2Salt = AuthenticationService.GenerateSalt();
            var p2Hash = AuthenticationService.HashPassword(p2 + p2Salt);

            var p3 = p2;
            var p3Salt = p2Salt;
            var p3Hash = AuthenticationService.HashPassword(p3Salt + p3);

            var p4 = "Password1233";
            var p4Salt = AuthenticationService.GenerateSalt();
            var p4Hash = AuthenticationService.HashPassword(p4 + p4Salt);

            Assert.AreEqual(p1Hash, p2Hash);
            Assert.AreNotEqual(p1Hash, p3Hash);
            Assert.AreNotEqual(p2Hash, p3Hash);
            Assert.AreNotEqual(p2Hash, p4Hash);
        }

        [TestMethod]
        public void HashPasswordWithSaltAsSeparateParameters()
        {
            var p1 = "Password1234";
            var p1Salt = AuthenticationService.GenerateSalt();
            var p1Hash = AuthenticationService.HashPassword(p1, p1Salt);

            var p2 = "Password1234";
            var p2Salt = AuthenticationService.GenerateSalt();
            var p2Hash = AuthenticationService.HashPassword(p2, p2Salt);

            var p3 = p2;
            var p3Salt = p2Salt;
            var p3Hash = AuthenticationService.HashPassword(p3, p3Salt);

            var p4 = "Password1233";
            var p4Salt = AuthenticationService.GenerateSalt();
            var p4Hash = AuthenticationService.HashPassword(p4, p4Salt);

            Assert.AreEqual(p1Hash, p2Hash);
            Assert.AreEqual(p2Hash, p3Hash);
            Assert.AreNotEqual(p1Hash, p4Hash);
            Assert.AreNotEqual(p2Hash, p4Hash);
            Assert.AreNotEqual(p3Hash, p4Hash);
        }
    }
}
