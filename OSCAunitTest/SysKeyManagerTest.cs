using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography;
using OSCA.Crypto;
using System.Security.AccessControl;
using OSCA;
using Org.BouncyCastle.X509;
using System.IO;
using Org.BouncyCastle.Pkcs;

namespace OSCAunitTest
{
    /// <summary>
    /// Summary description for SysKeyManagerTest
    /// </summary>
    [TestClass]
    public class SysKeyManagerTest
    {
        public SysKeyManagerTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void CreateTest()
        {
            CspParameters cspParam = SysKeyManager.Create(1024, "RSA", "Test Key Data");

            CspProviderFlags flags = cspParam.Flags;
            Assert.AreEqual("UseArchivableKey", flags.ToString());
            Assert.AreEqual("Test_Key_Data", cspParam.KeyContainerName);
            Assert.AreEqual<int>(24, cspParam.ProviderType);
            //CryptoKeySecurity sy = cspParam.CryptoKeySecurity;
        }

        [TestMethod]
        public void CreateTest1()
        {
            CspParameters cspParam = SysKeyManager.Create(1024, "DSA", "Test Key Data");

            CspProviderFlags flags = cspParam.Flags;
            Assert.AreEqual("UseArchivableKey", flags.ToString());
            Assert.AreEqual("Test_Key_Data", cspParam.KeyContainerName);
            Assert.AreEqual<int>(13, cspParam.ProviderType);
            //CryptoKeySecurity sy = cspParam.CryptoKeySecurity;
        }

        [TestMethod]
        public void ExportToP12Test()
        {
            CaTestHarness.InitialiseCA(true);
            ICA ca = CaTestHarness.LoadCA();
            string password = "foobar";

            //byte[] p12 = ca.Backup(password);

            //File.WriteAllBytes(CaTestHarness.testHarnessLocation + @"\CA\Backup.pfx", p12);

            //Read in the private key and certificate
            FileStream p12stream = new FileStream(CaTestHarness.testHarnessLocation + @"\CA\Backup.pfx", FileMode.Open);
            Pkcs12Store p12in = new Pkcs12Store(p12stream, password.ToCharArray());

            Assert.AreEqual("", p12in.GetCertificateAlias(ca.Certificate));

            //Assert.IsNotNull(p12in.GetKey().Key);
            //Assert.AreSame(ca.Certificate, p12in.GetCertificate(ca.CAName.ToString()).Certificate);

        }


    }
}
