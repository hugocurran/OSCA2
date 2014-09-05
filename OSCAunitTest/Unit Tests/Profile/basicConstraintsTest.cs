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
    ///This is a test class for basicConstraintsTest and is intended
    ///to contain all basicConstraintsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class basicConstraintsTest
    {

     /*  OSCA Profile XML
        ================
        <Extension>
            <name description="Basic Constraints">BasicConstraints</name>
            <critical>true</critical>
            <value>
                <ca>true</ca>
                <pathLen>none</pathLen>
            </value>
        </Extension>
    */
        // CA Pathlen=none
        private XElement testData1 = new XElement("Extension",
            new XElement("name", "BasicConstraints",
                new XAttribute("displayName", "Basic Constraints")),
            new XElement("critical", "True"),
            new XElement("value",
                new XElement("ca", "True"),
                new XElement("pathLen", "none")
            )
        );

        // CA Pathlen=4
        private XElement testData2 = new XElement("Extension",
            new XElement("name", "BasicConstraints",
                new XAttribute("displayName", "Basic Constraints")),
            new XElement("critical", "True"),
            new XElement("value",
                new XElement("ca", "True"),
                new XElement("pathLen", "4")
            )
        );

        // Entity
        private XElement testData3 = new XElement("Extension",
            new XElement("name", "BasicConstraints",
                new XAttribute("displayName", "Basic Constraints")),
            new XElement("critical", "False"),
            new XElement("value",
                new XElement("ca", "False"),
                new XElement("pathLen", "none")
            )
        );

        private string extValue1 = "#30030101ff";

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
        ///A test for PathLength
        ///</summary>
        [TestMethod()]
        public void PathLengthTest1()
        {
            basicConstraints target = new basicConstraints(testData1);
            string expected = "none";
            string actual;
            actual = target.PathLength;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for PathLength
        ///</summary>
        [TestMethod()]
        public void PathLengthTest2()
        {
            basicConstraints target = new basicConstraints(testData2);
            string expected = "4";
            string actual;
            actual = target.PathLength;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for IsCA
        ///</summary>
        [TestMethod()]
        public void IsCATest1()
        {
            basicConstraints target = new basicConstraints(testData1);
            bool expected = true;
            bool actual;
            actual = target.IsCA;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for IsCA
        ///</summary>
        [TestMethod()]
        public void IsCATest2()
        {
            basicConstraints target = new basicConstraints(testData3);
            bool expected = false;
            bool actual;
            actual = target.IsCA;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for encode
        ///</summary>
        [TestMethod()]
        public void encodeTest1()
        {
            basicConstraints target = new basicConstraints(testData1);
            string expected = "BasicConstraints: isCa(True)";
            BasicConstraints actual = target.BasicConstraints;
            Assert.AreEqual(expected, actual.ToString());
        }

        /// <summary>
        ///A test for encode
        ///</summary>
        [TestMethod()]
        public void encodeTest2()
        {
            basicConstraints target = new basicConstraints(testData2);
            string expected = "BasicConstraints: isCa(True), pathLenConstraint = 4";
            BasicConstraints actual = target.BasicConstraints;
            Assert.AreEqual(expected, actual.ToString());
        }

        /// <summary>
        ///A test for encode
        ///</summary>
        [TestMethod()]
        public void encodeTest3()
        {
            basicConstraints target = new basicConstraints(testData3);
            string expected = "BasicConstraints: isCa(False)";
            BasicConstraints actual = target.BasicConstraints;
            Assert.AreEqual(expected, actual.ToString());
        }

        /// <summary>
        ///A test for ToXml
        ///</summary>
        [TestMethod()]
        public void ToXmlTest1()
        {
            basicConstraints target = new basicConstraints(testData1);
            XNode expected = testData1;
            XNode actual;
            actual = target.ToXml();
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        /// <summary>
        ///A test for ToXml
        ///</summary>
        [TestMethod()]
        public void ToXmlTest2()
        {
            basicConstraints target = new basicConstraints(testData3);
            XNode expected = testData3;
            XNode actual;
            actual = target.ToXml();
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }


        /// <summary>
        ///A test for basicConstraints Constructor - start from scratch
        ///</summary>
        [TestMethod()]
        public void basicConstraintsConstructorTest1()
        {
            basicConstraints target = new basicConstraints();
            target.IsCA = true;
            target.PathLength = "none";
            XNode expected = testData1;
            XNode actual = target.ToXml();
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        /// <summary>
        ///A test for basicConstraints Constructor - start from scratch
        ///</summary>
        [TestMethod()]
        public void basicConstraintsConstructorTest2()
        {
            basicConstraints target = new basicConstraints(false);
            target.IsCA = false;
            target.PathLength = "none";
            XNode expected = testData3;
            XNode actual = target.ToXml();
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }


        /// <summary>
        ///A test for basicConstraints Constructor (from X509Extension)
        ///</summary>
        [TestMethod()]
        public void basicConstraintsConstructorTest3()
        {
            // Build an extension
            BcV3CertGen gen = new BcV3CertGen();
            basicConstraints testExt = new basicConstraints(testData1);
            gen.AddExtension(testExt.OID, testExt.Critical, testExt.DerEncoding);
            X509Certificate cert = CertTestHarness.RunCertGenerator(gen);
            X509Extension ext = CertTestHarness.ExtractExtension(cert, X509Extensions.BasicConstraints);

            // Test code
            basicConstraints target = new basicConstraints(ext);
            Assert.IsTrue(target.IsCA);
            Assert.IsNull(target.PathLength);
        }

        /// <summary>
        ///A test for basicConstraints Constructor (from X509Extension)
        ///</summary>
        [TestMethod()]
        public void basicConstraintsConstructorTest4()
        {
            // Build an extension
            BcV3CertGen gen = new BcV3CertGen();
            basicConstraints testExt = new basicConstraints(testData2);
            gen.AddExtension(testExt.OID, testExt.Critical, testExt.DerEncoding);
            X509Certificate cert = CertTestHarness.RunCertGenerator(gen);
            X509Extension ext = CertTestHarness.ExtractExtension(cert, X509Extensions.BasicConstraints);

            // Test code
            basicConstraints target = new basicConstraints(ext);
            Assert.IsTrue(target.IsCA);
            Assert.AreEqual(target.PathLength, "4");
        }
    }
}
