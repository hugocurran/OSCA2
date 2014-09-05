using OSCA.Profile;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.X509.Extension;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using System.Xml.Linq;
using OSCA;
using OSCA.Crypto;

namespace OSCAunitTest
{
    
    
    /// <summary>
    ///This is a test class for inhibitAnyPolicyTest and is intended
    ///to contain all inhibitAnyPolicyTest Unit Tests
    ///</summary>
    [TestClass()]
    public class inhibitAnyPolicyTest
    {

        /*
        <Extension>
            <name description="Inhibit AnyPolicy">InhibitAnyPolicy</name>
            <critical>true</critical>
            <value>1</value>
        </Extension>
        */

        private XElement testData1 = new XElement("Extension",
            new XElement("name", "InhibitAnyPolicy",
                new XAttribute("displayName", "Inhibit AnyPolicy")),
            new XElement("critical", "False"),
            new XElement("value", "1")
        );

        string extValue1 = "#020101";

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
        ///A test for Skip
        ///</summary>
        [TestMethod()]
        public void SkipTest()
        {
            inhibitAnyPolicy target = new inhibitAnyPolicy(false);
            int expected = 0;
            int actual;
            target.Skip = expected;
            actual = target.Skip;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for InhibitAnyPolicy
        ///</summary>
        [TestMethod()]
        public void InhibitAnyPolicyTest()
        {
            inhibitAnyPolicy target = new inhibitAnyPolicy();
            DerInteger actual;
            actual = target.InhibitAnyPolicy;
            Assert.IsInstanceOfType(actual, typeof(DerInteger));
        }

        /// <summary>
        ///A test for ToXml
        ///</summary>
        [TestMethod()]
        public void ToXmlTest()
        {
            inhibitAnyPolicy target = new inhibitAnyPolicy(testData1);
            XNode expected = testData1;
            XNode actual;
            actual = target.ToXml();
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        /// <summary>
        ///A test for inhibitAnyPolicy Constructor
        ///</summary>
        [TestMethod()]
        public void inhibitAnyPolicyConstructorTest()
        {
            inhibitAnyPolicy target = new inhibitAnyPolicy();
            Assert.IsTrue(target.Critical);
        }

        /// <summary>
        ///A test for inhibitAnyPolicy Constructor
        ///</summary>
        [TestMethod()]
        public void inhibitAnyPolicyConstructorTest1()
        {
            bool Critical = false;
            inhibitAnyPolicy target = new inhibitAnyPolicy(Critical);
            Assert.IsFalse(target.Critical);
        }

        /// <summary>
        ///A test for inhibitAnyPolicy Constructor (using X509Extension)
        ///</summary>
        [TestMethod()]
        public void inhibitAnyPolicyConstructorTest2()
        {
            // Build an extension
            BcV3CertGen gen = new BcV3CertGen();
            inhibitAnyPolicy testExt = new inhibitAnyPolicy(testData1);
            gen.AddExtension(testExt.OID, testExt.Critical, testExt.DerEncoding);
            X509Certificate cert = CertTestHarness.RunCertGenerator(gen);
            X509Extension ext = CertTestHarness.ExtractExtension(cert, X509Extensions.InhibitAnyPolicy);

            // Test code
            inhibitAnyPolicy target = new inhibitAnyPolicy(ext);
            Assert.IsFalse(target.Critical);
            Assert.AreEqual(1, target.Skip);
        }

        /// <summary>
        ///A test for inhibitAnyPolicy Constructor using XML
        ///</summary>
        [TestMethod()]
        public void inhibitAnyPolicyConstructorTest3()
        {
            XElement xml = testData1;
            inhibitAnyPolicy target = new inhibitAnyPolicy(xml);
            Assert.AreEqual(1, target.Skip);
        }
    }
}
