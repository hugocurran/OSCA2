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
    /// Summary description for PolicyMappingsTest
    /// </summary>
    [TestClass]
    public class PolicyMappingsTest
    {

        /*     
        OSCA Profile XML
        ================
        <Extension>
            <name description="Policy Mappings">PolicyMappings</name>
            <critical>false</critical>
            <value>
                <mapping>
                    <issuerPolicy name="Some policy">1.2.3.4</issuerPolicy>
                    <subjectPolicy name="Other policy">4.5.6.7</subjectPolicy>
                </mapping>
            </value>
        </Extension>
        */

        private XElement testData1 = new XElement("Extension",
            new XElement("name", "PolicyMappings",
                new XAttribute("displayName", "Policy Mappings")),
            new XElement("critical", "False"),
            new XElement("value",
                new XElement("mapping",
                    new XElement("issuerPolicy", "1.2.3.4",
                        new XAttribute("name", "Some policy")),
                    new XElement("subjectPolicy", "2.4.5.6.7",
                        new XAttribute("name", "Other policy"))
                )
            )
            );

        private string extVal1 = "#300d300b06032a0304060454050607";

        public PolicyMappingsTest()
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

      /// <summary>
        ///A test for PolicyMappings
        ///</summary>
        [TestMethod()]
        public void PolicyMappingsTest1()
        {
            policyMappings target = new policyMappings(testData1);
            PolicyMappings actual;
            actual = target.PolicyMappings;
            Assert.IsFalse(target.Critical);
            Assert.IsInstanceOfType(actual, typeof(PolicyMappings));
        }

        /// <summary>
        ///A test for CertPolicies
        ///</summary>
        [TestMethod()]
        public void CertPoliciesTest()
        {
            policyMappings target = new policyMappings(testData1);
            List<PolicyMapping> actual;
            actual = target.Mappings;
            Assert.AreEqual(1, actual.Count);
        }


        /// <summary>
        ///A test for ToXml
        ///</summary>
        [TestMethod()]
        public void ToXmlTest()
        {
            policyMappings target = new policyMappings(testData1);
            XNode expected = testData1;
            XNode actual;
            actual = target.ToXml();
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        /// <summary>
        ///A test for Add
        ///</summary>
        [TestMethod()]
        public void AddTest()
        {
            policyMappings target = new policyMappings(testData1);
            PolicyMapping Map = new PolicyMapping()
            {
                issuerOid = "1.7.8.9.10",
                issuerPolicyName = "NewPolicy",
                subjectOid = "2.4.5.6.7",
                subjectPolicyName = "Other policy"
            };
            target.Add(Map);
            Assert.AreEqual(2, target.Mappings.Count);
            Assert.AreEqual("1.7.8.9.10", target.Mappings[1].issuerOid);
        }

        /// <summary>
        ///A test for Remove
        ///</summary>
        [TestMethod()]
        public void RemoveTest()
        {
            policyMappings target = new policyMappings(testData1);
            PolicyMapping Map = new PolicyMapping() {
                issuerOid = "1.7.8.9.10",
                issuerPolicyName = "NewPolicy",
                subjectOid = "2.4.5.6.7",
                subjectPolicyName = "Other policy" 
            };
            target.Add(Map);
            Assert.AreEqual(2, target.Mappings.Count);
            target.Remove(Map);
            Assert.AreEqual(1, target.Mappings.Count);
            Assert.AreEqual("1.2.3.4", target.Mappings[0].issuerOid);
        }

        /// <summary>
        ///A test for policyMappings Constructor
        ///</summary>
        [TestMethod()]
        public void policyMappingsConstructorTest1()
        {
            bool Critical = false;
            policyMappings target = new policyMappings(Critical);
            Assert.IsFalse(target.Critical);
        }

        /// <summary>
        ///A test for basicConstraints Constructor (from X509Extension)
        ///</summary>
        [TestMethod()]
        public void policyMappingsConstructorTest2()
        {
            // Build an extension
            BcV3CertGen gen = new BcV3CertGen();
            policyMappings testExt = new policyMappings(testData1);
            gen.AddExtension(testExt.OID, testExt.Critical, testExt.DerEncoding);
            X509Certificate cert = CertTestHarness.RunCertGenerator(gen);
            X509Extension ext = CertTestHarness.ExtractExtension(cert, X509Extensions.PolicyMappings);

            // Test code
            policyMappings target = new policyMappings(ext);
            List<PolicyMapping> list = target.Mappings;
            Assert.IsFalse(target.Critical);
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual("1.2.3.4", list[0].issuerOid);
            Assert.IsNull(list[0].issuerPolicyName);
            Assert.AreEqual("2.4.5.6.7", list[0].subjectOid);
            Assert.IsNull(list[0].subjectPolicyName);
        }

    }
}

