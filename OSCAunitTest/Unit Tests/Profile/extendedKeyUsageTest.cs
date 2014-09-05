using OSCA.Profile;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Xml.Linq;
using OSCA;
using OSCA.Crypto;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using System.Collections.Generic;

namespace OSCAunitTest
{
    
    
    /// <summary>
    ///This is a test class for extendedKeyUsageTest and is intended
    ///to contain all extendedKeyUsageTest Unit Tests
    ///</summary>
    [TestClass()]
    public class extendedKeyUsageTest
    {

        /*
        * <Extension>
        *   <name>ExtendedKeyUsage</name> 
        *   <critical>false</critical> 
        *   <value> 
        *     <eku>ClientAuth</eku> 
        *   </value> 
        * </Extension>
        */

        private XElement testData1 = new XElement("Extension",
            new XElement("name", "ExtendedKeyUsage",
                new XAttribute("displayName", "Extended Key Usage")),
                new XElement("critical", "False"),
                new XElement("value",
                    new XElement("eku", "ClientAuth"),
                    new XElement("eku", "ServerAuth")
                )
        );

        string extValue1 = "#301406082b0601050507030206082b06010505070301";


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
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for extendedKeyUsage Constructor using XML
        ///</summary>
        [TestMethod()]
        public void extendedKeyUsageConstructorTest()
        {
            XElement xml = testData1;
            extendedKeyUsage target = new extendedKeyUsage(xml);
            Assert.AreEqual(target.ToXml().ToString(), testData1.ToString());
        }

        /// <summary>
        ///A test for extendedKeyUsage Constructor
        ///</summary>
        [TestMethod()]
        public void extendedKeyUsageConstructorTest1()
        {
            bool Critical = true;
            extendedKeyUsage target = new extendedKeyUsage(Critical);
            Assert.IsTrue(target.Critical);
        }

        /// <summary>
        ///A test for extendedKeyUsage Constructor
        ///</summary>
        [TestMethod()]
        public void extendedKeyUsageConstructorTest2()
        {
            extendedKeyUsage target = new extendedKeyUsage();
            Assert.IsFalse(target.Critical);
        }

        /// <summary>
        ///A test for extendedKeyUsage Constructor using X509Extension
        ///</summary>
        [TestMethod()]
        public void extendedKeyUsageConstructorTest3()
        {
            // Build an extension
            BcV3CertGen gen = new BcV3CertGen();
            extendedKeyUsage testExt = new extendedKeyUsage(testData1);
            gen.AddExtension(testExt.OID, testExt.Critical, testExt.DerEncoding);
            X509Certificate cert = CertTestHarness.RunCertGenerator(gen);
            X509Extension ext = CertTestHarness.ExtractExtension(cert, X509Extensions.ExtendedKeyUsage);

            // Test code
            extendedKeyUsage target = new extendedKeyUsage(ext);
            List<string> list = target.ExtKUsage;
            Assert.IsFalse(target.Critical);
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual("ClientAuth", list[0]);
            Assert.AreEqual("ServerAuth", list[1]);
        }

        /// <summary>
        ///A test for Add
        ///</summary>
        [TestMethod()]
        public void AddTest()
        {
            extendedKeyUsage target = new extendedKeyUsage();
            string value = "CodeSigning";
            target.Add(value);
            Assert.AreEqual(1, target.ExtKUsage.Count);
            Assert.AreEqual("CodeSigning", target.ExtKUsage[0]);
        }


        /// <summary>
        ///A test for LookUp
        ///</summary>
        [TestMethod()]
        public void LookUpTest()
        {
            string eku = "TimeStamping";
            KeyPurposeID expected = KeyPurposeID.IdKPTimeStamping;
            KeyPurposeID actual;
            actual = extendedKeyUsage.LookUp(eku);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Remove
        ///</summary>
        [TestMethod()]
        public void RemoveTest()
        {
            extendedKeyUsage target = new extendedKeyUsage(testData1);
            string value = "ServerAuth";
            target.Remove(value);
            Assert.AreEqual(1, target.ExtKUsage.Count);
        }

        /// <summary>
        ///A test for ToXml
        ///</summary>
        [TestMethod()]
        public void ToXmlTest()
        {
            extendedKeyUsage target = new extendedKeyUsage(testData1);
            XNode expected = testData1;
            XNode actual;
            actual = target.ToXml();
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }


        /// <summary>
        ///A test for ExtKeyUsageOid
        ///</summary>
        [TestMethod()]
        public void ExtKeyUsageOidTest()
        {
            string[] actual;
            actual = extendedKeyUsage.ExtKeyUsageOid;
            Assert.AreEqual(KeyPurposeID.IdKPEmailProtection.ToString(), actual[4].ToString());
        }

        /// <summary>
        ///A test for ExtKeyUsageText
        ///</summary>
        [TestMethod()]
        public void ExtKeyUsageTextTest()
        {
            string[] actual;
            actual = extendedKeyUsage.ExtKeyUsageText;
            Assert.AreEqual("EmailProtection", actual[4].ToString());
        }

        /// <summary>
        ///A test for ExtendedKeyUsage
        ///</summary>
        [TestMethod()]
        public void ExtendedKeyUsageTest()
        {
            extendedKeyUsage target = new extendedKeyUsage(testData1);
            ExtendedKeyUsage actual;
            actual = target.ExtendedKeyUsage;
            Assert.IsInstanceOfType(actual, typeof(ExtendedKeyUsage));
            Assert.AreEqual(2, actual.Count);
        }
    }
}
