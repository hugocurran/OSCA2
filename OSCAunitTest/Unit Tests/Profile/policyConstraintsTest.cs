using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.X509;
using OSCA;
using OSCA.Crypto;
using OSCA.Profile;

namespace OSCAunitTest
{
    
    
    /// <summary>
    ///This is a test class for policyConstraintsTest and is intended
    ///to contain all policyConstraintsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class policyConstraintsTest
    {

        /*  OSCA Profile XML
           ================
           <Extension>
               <name description="Policy Constraints">PolicyConstraints</name>
               <critical>true</critical>
               <value>
                <requireExplicitPolicy>0</requireExplicitPolicy>
                <inhibitPolicyMapping>2</inhibitPolicyMapping>
               </value>
           </Extension>
        */

        private XElement testData1 = new XElement("Extension",
            new XElement("name", "PolicyConstraints",
                new XAttribute("displayName", "Policy Constraints")),
            new XElement("critical", "True"),
            new XElement("value",
                new XElement("requireExplicitPolicy", "0"),
                new XElement("inhibitPolicyMapping", "2")
            )
        );

        private XElement testData2 = new XElement("Extension",
            new XElement("name", "PolicyConstraints",
                new XAttribute("displayName", "Policy Constraints")),
            new XElement("critical", "True"),
            new XElement("value",
                new XElement("inhibitPolicyMapping", "2")
            )
        );

        private string extValue1 = "[[0]0, [1]2]";
        private string extValue2 = "[[1]2]";
        private string extValue1Der = "#300aa003020100a103020102";


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
        ///A test for RequireExplicitPolicy
        ///</summary>
        [TestMethod()]
        public void RequireExplicitPolicyTest()
        {
            policyConstraints target = new policyConstraints();
            int expected = 3;
            int actual;
            target.RequireExplicitPolicy = expected;
            actual = target.RequireExplicitPolicy;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for PolicyConstraint
        ///</summary>
        [TestMethod()]
        public void PolicyConstraintTest1()
        {
            policyConstraints target = new policyConstraints(testData1);
            Asn1Sequence actual;
            actual = target.PolicyConstraint;
            Assert.AreEqual(extValue1, actual.ToString());
        }

        /// <summary>
        ///A test for PolicyConstraint
        ///</summary>
        [TestMethod()]
        public void PolicyConstraintTest2()
        {
            policyConstraints target = new policyConstraints(testData2);
            Asn1Sequence actual;
            actual = target.PolicyConstraint;
            Assert.AreEqual(extValue2, actual.ToString());
        }

        /// <summary>
        ///A test for InhibitPolicyMapping
        ///</summary>
        [TestMethod()]
        public void InhibitPolicyMappingTest()
        {
            policyConstraints target = new policyConstraints(testData2);
            int expected = -1;
            int actual;
            target.InhibitPolicyMapping = expected;
            actual = target.InhibitPolicyMapping;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ToXml
        ///</summary>
        [TestMethod()]
        public void ToXmlTest()
        {
            policyConstraints target = new policyConstraints(testData1);
            XNode expected = testData1;
            XNode actual;
            actual = target.ToXml();
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        /// <summary>
        ///A test for policyConstraints Constructor
        ///</summary>
        [TestMethod()]
        public void policyConstraintsConstructorTest()
        {
            policyConstraints target = new policyConstraints();
            Assert.IsTrue(target.Critical);
        }

        /// <summary>
        ///A test for policyConstraints Constructor
        ///</summary>
        [TestMethod()]
        public void policyConstraintsConstructorTest1()
        {
            bool Critical = false; // TODO: Initialize to an appropriate value
            policyConstraints target = new policyConstraints(Critical);
            Assert.IsFalse(target.Critical);
        }

        /// <summary>
        ///A test for policyConstraints Constructor
        ///</summary>
        [TestMethod()]
        public void policyConstraintsConstructorTest2()
        {
            XElement xml = testData1; // TODO: Initialize to an appropriate value
            policyConstraints target = new policyConstraints(xml);
            Assert.IsTrue(target.Critical);
            Assert.AreEqual(0, target.RequireExplicitPolicy);
            Assert.AreEqual(2, target.InhibitPolicyMapping);
        }

        /// <summary>
        ///A test for policyConstraints Constructor (from X509Extension)
        ///</summary>
        [TestMethod()]
        public void policyConstraintsConstructorTest4()
        {
            // Build an extension
            BcV3CertGen gen = new BcV3CertGen();
            policyConstraints testExt = new policyConstraints(testData2);
            gen.AddExtension(testExt.OID, testExt.Critical, testExt.DerEncoding);
            X509Certificate cert = CertTestHarness.RunCertGenerator(gen);
            X509Extension ext = CertTestHarness.ExtractExtension(cert, X509Extensions.PolicyConstraints);

            // Test code
            policyConstraints target = new policyConstraints(ext);
            Assert.IsTrue(target.Critical);
            Assert.AreEqual(-1, target.RequireExplicitPolicy);
            Assert.AreEqual(2, target.InhibitPolicyMapping);
        }
    }
}
