using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSCA;
using OSCA.Offline;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Asn1.X509;
using System.IO;
using Org.BouncyCastle.Pkcs;
using OSCA.Profile;

namespace OSCAunitTest
{
    /// <summary>
    /// Tests for CreatCA.  Note this is functionally the same as the CA test harness.
    /// </summary>
    [TestClass]
    public class CaFactoryTest
    {
        public CaFactoryTest()
        {    // TODO: Add constructor logic here 
        }

        private CAConfig BcSubCaConfig = new CAConfig()
        {
            name = "Test subCA (BC)",
            profile = CA_Profile.SubCA,
            pkAlgo = "RSA",
            pkSize = 1024,
            sigAlgo = "SHA1withRSA",
            keyUsage = (X509KeyUsage.KeyCertSign | X509KeyUsage.CrlSign),
            version = X509ver.V3,
            life = 2,
            units = "Years",
            FIPS140 = false,
            caType = CA_Type.simpleCA,
            location = CaTestHarness.testHarnessLocation + @"\subCA",
            password = "foobar",
            crlInterval = 90,
            profileFile = CaTestHarness.testHarnessLocation + @"\subCA.xml",
            DN = new X509Name("CN=Test subCA BC")
        };

        public static CAConfig SysSubCaConfig = new CAConfig()
        {
            name = "Test subCA (Sys)",
            profile = CA_Profile.SubCA,
            pkAlgo = "RSA",
            pkSize = 1024,
            sigAlgo = "SHA1withRSA",
            keyUsage = (X509KeyUsage.KeyCertSign | X509KeyUsage.CrlSign),
            version = X509ver.V3,
            life = 2,
            units = "Years",
            FIPS140 = true,
            caType = CA_Type.fipsCA,
            location = CaTestHarness.testHarnessLocation + @"\subCA",
            password = "foobar",
            crlInterval = 90,
            profileFile = CaTestHarness.testHarnessLocation + @"\subCA.xml",
            DN = new X509Name("CN=Test subCA Sys")
        };

        //private string subCaLocation = CaTestHarness.testHarnessLocation

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
        [ClassCleanup()]
        public static void MyClassCleanup() 
        {
            // CaTestHarness.Cleanup();
        
        }
        
        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void MyTestInitialize() 
        {
            try
            {
                CaTestHarness.Cleanup();
            }
            catch (DirectoryNotFoundException) { }
            try
            {
                Directory.Delete(CaTestHarness.testHarnessLocation + @"\subCA", true);
            }
            catch (DirectoryNotFoundException) { }
        }
        //
        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup() 
        {
            
        
        }
        //
        #endregion

        /// <summary>
        /// Test creation of a Simple (BC crypto) root CA; same as the CATestHarness
        /// </summary>
        [TestMethod]
        public void CreateRootCATest()
        {
            CaTestHarness.InitialiseCA(false);

            ICA ca = CaTestHarness.LoadCA();

            Assert.AreEqual("CN=Test Harness BC", ca.CAName);
            Assert.IsFalse(ca.FIPS140Mode);
        }

        /// <summary>
        /// Test creation of a FIPS (Sys crypto) root CA; same as the CATestHarness
        /// </summary>
        [TestMethod]
        public void CreateRootCATest1()
        {
            CaTestHarness.InitialiseCA(true);

            ICA ca = CaTestHarness.LoadCA();

            Assert.AreEqual("CN=Test Harness Sys", ca.CAName);
            Assert.IsTrue(ca.FIPS140Mode);
        }

        /// <summary>
        /// Test creation of a Simple (BC crypto) sub CA
        /// </summary>
        [TestMethod]
        public void CreateSubCATest()
        {
            // Setup
            CaTestHarness.InitialiseCA(false);
            ICA ca = CaTestHarness.LoadCA();

            // Test
            string caLocation;
            Directory.CreateDirectory(CaTestHarness.testHarnessLocation + @"\subCA");
            caLocation = CaFactory.CreateSubCA(BcSubCaConfig, ca);
            Assert.AreEqual(CaTestHarness.testHarnessLocation + @"\subCA\CAConfig.xml", caLocation);

            // Test 2
            ICA subCa = OSCA.OSCA.LoadCA(caLocation, "foobar");
            Assert.AreEqual("CN=Test subCA BC", subCa.CAName);
            Assert.IsFalse(subCa.FIPS140Mode);
        }

        /// <summary>
        /// Test creation of a FIPS (Sys crypto) sub CA
        /// </summary>
        [TestMethod]
        public void CreateSubCATest1()
        {
            // Setup
            CaTestHarness.InitialiseCA(true);
            ICA ca = CaTestHarness.LoadCA();

            // Test
            string caLocation;
            Directory.CreateDirectory(CaTestHarness.testHarnessLocation + @"\subCA");
            caLocation = CaFactory.CreateSubCA(SysSubCaConfig, ca);
            Assert.AreEqual(CaTestHarness.testHarnessLocation + @"\subCA\CAConfig.xml", caLocation);

            // Test 2
            ICA subCa = OSCA.OSCA.LoadCA(caLocation, "foobar");
            Assert.AreEqual("CN=Test subCA Sys", subCa.CAName);
            Assert.IsTrue(subCa.FIPS140Mode);
        }

        /// <summary>
        /// Test creation of a FIPS (Sys crypto) sub CA using request method
        /// </summary>
        [TestMethod]
        public void CreateSubCATest2()
        {
            // Setup
            CaTestHarness.InitialiseCA(true);
            ICA ca = CaTestHarness.LoadCA();

            // Test
            //string caLocation;
            Directory.CreateDirectory(CaTestHarness.testHarnessLocation + @"\subCA");

            Pkcs10CertificationRequest p10;
            p10 = CaFactory.CreateSubCA(SysSubCaConfig);
            Assert.AreEqual("CN=Test subCA Sys", p10.GetCertificationRequestInfo().Subject.ToString());
            Assert.IsTrue(p10.Verify());

            // Test 2
            Profile profile = new Profile(CaTestHarness.testHarnessLocation + @"\subCA.xml");
            X509Certificate cert = ca.IssueCertificate(p10, profile);

            CaFactory.CreateSubCA(CaTestHarness.testHarnessLocation + @"\subCA\CAConfig.xml", cert);
            ICA subCa = OSCA.OSCA.LoadCA(CaTestHarness.testHarnessLocation + @"\subCA\CAConfig.xml", "foobar");
            Assert.AreEqual("CN=Test subCA Sys", subCa.CAName);
            Assert.IsTrue(subCa.FIPS140Mode);
        }
    }
}
