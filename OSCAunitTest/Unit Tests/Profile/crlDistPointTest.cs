using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.X509;
using OSCA.Crypto;
using OSCA.Profile;
using OSCA;

namespace OSCAunitTest
{
    
    
    /// <summary>
    ///This is a test class for crlDistPointTest and is intended
    ///to contain all crlDistPointTest Unit Tests
    ///</summary>
    [TestClass()]
    public class crlDistPointTest
    {
        /*  OSCA XML Config
         * ================
           <Extension>
            <name description="CRL Distribution Points">CrlDistributionPoints</name>
            <critical>true</critical>
            <value>
                <cdp>
                    <name type="uniformResourceIdentifier">http://crl.foo.org/wotsit.crl</name>
                </cdp>
                <cdp>
                    <name type="uniformResourceIdentifier">http://www.bar.org/wotsit.crl</name>
                </cdp>
            </value>
           </Extension>
         */

        // TEST DATA
        private XElement testInputXml = new XElement("Extension",
            new XElement("name", "CrlDistributionPoints",
                new XAttribute("displayName", "CRL Distribution Points")),
            new XElement("critical", "True"),
            new XElement("value",
                new XElement("cdp", "http://crl.foo.org/wotsit.crl",
                    new XAttribute("type", "uniformResourceIdentifier")),
                new XElement("cdp", "http://www.bar.org/wotsit.crl",
                    new XAttribute("type", "uniformResourceIdentifier"))
            )
        );

        private OSCAGeneralName testInputGeneralName1 = new OSCAGeneralName()
        {
            Name = "http://crl.foo.org/wotsit.crl",
            Type = GenName.uniformResourceIdentifier
        };

        private OSCAGeneralName testInputGeneralName2 = new OSCAGeneralName()
        {
            Name = "http://www.bar.org/wotsit.crl",
            Type = GenName.uniformResourceIdentifier
        };

        private string testRawDerValue = "#304a3023a021a01f861d687474703a2f2f63726c2e666f6f2e6f72672f776f747369742e63726c3023a021a01f861d687474703a2f2f7777772e6261722e6f72672f776f747369742e63726c";

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
        ///A test for DistPoints
        ///</summary>
        [TestMethod()]
        public void DistPointsTest()
        {
            crlDistPoint target = new crlDistPoint(testInputXml);
            List<OSCAGeneralName> actual;
            actual = target.DistPoints;
            Assert.AreEqual(testInputGeneralName1, actual[0]);
        }

        /// <summary>
        ///A test for CrlDistributionPoint
        ///</summary>
        [TestMethod()]
        public void CrlDistributionPointTest()
        {
            crlDistPoint target = new crlDistPoint(testInputXml);
            CrlDistPoint actual;
            actual = target.CrlDistributionPoint;
            Assert.IsInstanceOfType(actual, typeof(CrlDistPoint));
        }

        /// <summary>
        ///A test for ToXml
        ///</summary>
        [TestMethod()]
        public void ToXmlTest()
        {
            crlDistPoint target = new crlDistPoint(testInputXml);
            XNode expected = testInputXml;
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
            crlDistPoint target = new crlDistPoint(testInputXml);
            OSCAGeneralName Name = testInputGeneralName1;
            target.Remove(Name);
            Assert.AreEqual(1, target.DistPoints.Count);
        }

        /// <summary>
        ///A test for Add
        ///</summary>
        [TestMethod()]
        public void AddTest()
        {
            crlDistPoint target = new crlDistPoint(true);
            OSCAGeneralName Name = testInputGeneralName1;
            target.Add(Name);
            Assert.AreEqual(testInputGeneralName1, target.DistPoints[0]);
        }

        /// <summary>
        ///A test for crlDistPoint Constructor
        ///</summary>
        [TestMethod()]
        public void crlDistPointConstructorTest()
        {
            crlDistPoint target = new crlDistPoint();
            Assert.IsFalse(target.Critical);
            Assert.AreEqual("CrlDistributionPoints", target.Name);
            Assert.AreEqual("CRL Distribution Points", target.DisplayName);
        }

        /// <summary>
        ///A test for crlDistPoint Constructor
        ///</summary>
        [TestMethod()]
        public void crlDistPointConstructorTest1()
        {
            bool Critical = true;
            crlDistPoint target = new crlDistPoint(Critical);
            Assert.IsTrue(target.Critical);
            Assert.AreEqual("CrlDistributionPoints", target.Name);
            Assert.AreEqual("CRL Distribution Points", target.DisplayName);
        }

        /// <summary>
        ///A test for crlDistPoint Constructor
        ///</summary>
        [TestMethod()]
        public void crlDistPointConstructorTest2()
        {
            XElement xml = testInputXml;
            crlDistPoint target = new crlDistPoint(xml);
            Assert.IsTrue(target.Critical);
            Assert.AreEqual("CrlDistributionPoints", target.Name);
            Assert.AreEqual("CRL Distribution Points", target.DisplayName);
        }
    }
}
