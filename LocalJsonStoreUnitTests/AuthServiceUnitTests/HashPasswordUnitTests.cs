using LocalJsonStore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LocalJsonStoreUnitTests.AuthServiceUnitTests
{
    [TestClass]
    public class HashPasswordUnitTests
    {
        internal IAuthService AuthService { get; set; }

        public HashPasswordUnitTests()
        {
            AuthService = new AuthService();
        }

        [TestMethod]
        public void HashedPasswordsMatch()
        {
        }

        [TestMethod]
        public void HashedPasswordsDontMatch()
        {
        }

        [TestMethod]
        public void HashedPasswordsAppearUnique()
        {
        }
    }
}
