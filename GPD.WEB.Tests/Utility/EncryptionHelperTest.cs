using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GPD.Utility;

namespace GPD.WEB.Tests.Utility
{
    [TestClass]
    public class EncryptionHelperTest
    {
        [TestMethod]
        public void Encrypt()
        {
            string input = "Pass@1234";
            string result = "bEj2zc83seAjPYIFYJ4m9Q/cIOSOED97UxDYAtkELh8=";
            string cipherText = EncryptionHelper.Encrypt(input);
            Assert.AreEqual(cipherText, result);
        }

        [TestMethod]
        public void Decrypt()
        {
            string input = "bEj2zc83seAjPYIFYJ4m9Q/cIOSOED97UxDYAtkELh8=";
            string result = "Pass@1234";
            string clearText = EncryptionHelper.Decrypt(input);
            Assert.AreEqual(clearText, result);
        }
    }
}
