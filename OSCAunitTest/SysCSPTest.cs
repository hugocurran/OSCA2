using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSCA.Crypto;

namespace OSCAunitTest
{
    [TestClass]
    public class SysCSPTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            Dictionary<string, int> providers = SysCSP.ReadAllProviders();
            Assert.IsTrue(providers.Count > 0);
        }
    }
}
