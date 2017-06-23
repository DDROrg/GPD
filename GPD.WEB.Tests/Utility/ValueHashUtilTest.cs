using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GPD.WEB.Tests.Utility
{
    using GPD.Utility.CommonUtils;
    [TestClass]
    public class ValueHashUtilTest
    {
        [TestMethod]
        public void CreateHashAndValidate()
        {
            string input = "Pass@1234";
            string hash = ValueHashUtil.CreateHash(input);
            bool isValid = ValueHashUtil.ValidateHash(input, hash);
            Assert.AreEqual(isValid, true);
        }
    }
}
