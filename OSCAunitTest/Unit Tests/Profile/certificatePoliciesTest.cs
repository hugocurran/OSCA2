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
using OSCA.Crypto.X509;

namespace OSCAunitTest
{
    
    
    /// <summary>
    ///This is a test class for certificatePoliciesTest and is intended
    ///to contain all certificatePoliciesTest Unit Tests
    ///</summary>
    [TestClass()]
    public class certificatePoliciesTest
    {


        /*      OSCA XML
                ========
        <Extension>
        <name description="Certificate Policies">CertificatePolicies</name>
        <critical>false</critical>
        <value>
            <policyInfo>
                <oid>1.2.860.0.1311.1.1</oid>
                <cps>http://foo.com/cps.htm</cps>
                <unotice>This is a test notice</unotice>
                <name></name>
            </policyInfo>
            <policyInfo>
                <oid>1.2.860.0.1311.1.3</oid>
                <cps></cps>
                <unotice></unotice>
                <name></name>
            </policyInfo>   
        </value>
        </Extension>
   */

        private XElement testData1 = new XElement("Extension",
            new XElement("name", "CertificatePolicies",
                new XAttribute("displayName", "Certificate Policies")),
            new XElement("critical", "False"),
            new XElement("value",
                new XElement("policyInfo",
                    new XElement("oid", "1.2.860.0.1311.1.1"),
                    new XElement("cps", "http://foo.com/cps.htm"),
                    new XElement("unotice", "This is a test notice"),
                    new XElement("name", "")
                ),
                new XElement("policyInfo",
                    new XElement("oid", "1.2.860.0.1311.1.3"),
                    new XElement("cps", ""),
                    new XElement("unotice", ""),
                    new XElement("name", "")
                )
            )
        );

        private string extVal1 = "#3063305506082a865c008a1f01013049302206082b060105050702011616687474703a2f2f666f6f2e636f6d2f6370732e68746d302306082b06010505070202301716155468697320697320612074657374206e6f74696365300a06082a865c008a1f0103";

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
        ///A test for CertificatePolicies
        ///</summary>
        [TestMethod()]
        public void CertificatePoliciesTest()
        {
            certificatePolicies target = new certificatePolicies(testData1);
            CertificatePolicies actual;
            actual = target.CertificatePolicies;
            Assert.IsFalse(target.Critical);
            Assert.AreEqual("CertificatePolicies: 1.2.860.0.1311.1.1, 1.2.860.0.1311.1.3", actual.ToString());
        }

        /// <summary>
        ///A test for CertPolicies
        ///</summary>
        [TestMethod()]
        public void CertPoliciesTest()
        {
            certificatePolicies target = new certificatePolicies(testData1);
            List<CertPolicy> actual;
            actual = target.CertPolicies;
            Assert.AreEqual(2, actual.Count);
        }


        /// <summary>
        ///A test for ToXml
        ///</summary>
        [TestMethod()]
        public void ToXmlTest()
        {
            certificatePolicies target = new certificatePolicies(testData1);
            XNode expected = testData1;
            XNode actual;
            actual = target.ToXml();
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        /// <summary>
        ///A test for Remove
        ///</summary>
        [TestMethod()]
        public void RemoveTest()
        {
            certificatePolicies target = new certificatePolicies(testData1);
            CertPolicy Policy = new CertPolicy() { Oid = "1.2.860.0.1311.1.3", Cps="", Name = "", Unotice = "" };
            target.Remove(Policy);
            Assert.AreEqual("CertificatePolicies: 1.2.860.0.1311.1.1", target.CertificatePolicies.ToString());
        }

        /// <summary>
        ///A test for certificatePolicies Constructor
        ///</summary>
        [TestMethod()]
        public void certificatePoliciesConstructorTest1()
        {
            bool Critical = false;
            certificatePolicies target = new certificatePolicies(Critical);
            Assert.IsFalse(target.Critical);
        }

        /// <summary>
        ///A test for basicConstraints Constructor (from X509Extension)
        ///</summary>
        [TestMethod()]
        public void certificatePoliciesConstructorTest2()
        {
            // Build an extension
            BcV3CertGen gen = new BcV3CertGen();
            certificatePolicies testExt = new certificatePolicies(testData1);
            gen.AddExtension(testExt.OID, testExt.Critical, testExt.DerEncoding);
            X509Certificate cert = CertTestHarness.RunCertGenerator(gen);
            X509Extension ext = CertTestHarness.ExtractExtension(cert, X509Extensions.CertificatePolicies);

            // Test code
            certificatePolicies target = new certificatePolicies(ext);
            List<CertPolicy> list = target.CertPolicies;
            Assert.IsFalse(target.Critical);
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual("http://foo.com/cps.htm", list[0].Cps);
            Assert.AreEqual("This is a test notice", list[0].Unotice);
            Assert.AreEqual("1.2.860.0.1311.1.3", list[1].Oid);
        }

    }
}
