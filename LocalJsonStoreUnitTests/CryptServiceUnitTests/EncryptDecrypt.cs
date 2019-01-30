using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LocalJsonStore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LocalJsonStoreUnitTests.CryptServiceUnitTests
{
    [TestClass]
    public class EncryptDecrypt
    {
        public ICryptService Cs { get; set; }

        public EncryptDecrypt()
        {
            Cs = new CryptService();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "args not null")]
        public void ParameterExceptions1()
        {
            Cs.Encrypt(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "args not empty")]
        public void ParameterExceptions2()
        {
            Cs.Encrypt("", "");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "args not string.empty")]
        public void ParameterExceptions3()
        {
            Cs.Encrypt(string.Empty, string.Empty);
        }

        [TestMethod]
        public void EncryptUnique()
        {
            var data = "hopefully all the encrypted strings are unique for the same data";
            var key = "key";

            var l = new List<string>();
            for (var i = 0; i < 1000; i++)
            {
                l.Add(Cs.Encrypt(data, key));
            }

            Assert.IsTrue(l.Distinct().Count() == l.Count);
        }

        [TestMethod]
        public void EncryptedStringIsBase64()
        {
            var b64Chars = new List<char>()
            {
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q','r','s','t','u','v','w','x','y','z',
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q','R','S','T','U','V','W','X','Y','Z',
                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
                '+', '/', '='
            };
            var data = "adshlf jadslfjlsjdhf fd  sdkl slas   sakljfsdkjf ha  daj dsl lashd";
            var key = "key";

            var l = new List<string>();
            for (var i = 0; i < 1000; i++)
            {
                l.Add(Cs.Encrypt(data, key));
            }

            foreach (var str in l)
            {
                foreach (var c in str)
                {
                    Assert.IsTrue(b64Chars.Contains(c));
                }
            }
        }

        [TestMethod]
        public void EncryptAndDecrypt()
        {
            var msg = "TEST_FAILED." + MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name + ": ";

            var key = "0123456789012345";
            var data1 = "this is a secret";
            var data2 = "this is another secret";
            var data3 = data2;

            var encryptedData1 = Cs.Encrypt(data1, key);
            var encryptedData2 = Cs.Encrypt(data2, key);
            var encryptedData3 = Cs.Encrypt(data3, key);

            var unencryptedData1 = Cs.Decrypt(encryptedData1, key);
            var unencryptedData2 = Cs.Decrypt(encryptedData2, key);
            var unencryptedData3 = Cs.Decrypt(encryptedData3, key);

            // make sure encrypted data is different from original
            Assert.AreNotEqual(encryptedData1, data1, msg + "encrypted data matches original data");
            Assert.AreNotEqual(encryptedData1, data2, msg + "encrypted data matches original data");
            Assert.AreNotEqual(encryptedData2, data3, msg + "encrypted data matches original data");

            // make sure encrypted strings are all unique
            Assert.AreNotEqual(encryptedData1, encryptedData2, msg + "encrypted data matches");
            Assert.AreNotEqual(encryptedData1, encryptedData3, msg + "encrypted data matches");
            Assert.AreNotEqual(encryptedData2, encryptedData3, msg + "encrypted data matches");

            // make sure the unencrypted data matches the original data
            Assert.AreEqual(unencryptedData1, data1, msg + "unencrypted data does not match original data");
            Assert.AreEqual(unencryptedData2, data2, msg + "unencrypted data does not match original data");
            Assert.AreEqual(unencryptedData3, data3, msg + "unencrypted data does not match original data");
        }
    }
}
