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
    ///This is a test class for authorityInfoAccessTest and is intended
    ///to contain all authorityInfoAccessTest Unit Tests
    ///</summary>
    [TestClass()]
    public class authorityInfoAccessTest
    {
      /*      OSCA XML
            ========
            <Extension>
            <name description="Authority Info Access">AuthorityInfoAccess</name>
            <critical>false</critical>
            <value>
                <accessDescription>
                    <method>CAIssuers</method>
                    <location type="uniformResourceIdentifier">http://foo.com/issuer.crt</location>
                <accessDescription>
                 <accessDescription>
                    <method>Ocsp</method>
                    <location type="uniformResourceIdentifier">http://foo.com:2560/ocsp</location>
                <accessDescription>    
            </value>
            </Extension>
       */

        private XElement testXmlInput = new XElement("Extension",
            new XElement("name", "AuthorityInfoAccess",
                new XAttribute("displayName", "Authority Info Access")),
            new XElement("critical", "False"),
            new XElement("value",
                new XElement("accessDescription",
                    new XElement("method", "CAIssuers"),
                    new XElement("location", "http://foo.com/issuer.crt",
                        new XAttribute("type", "uniformResourceIdentifier"))
                ),
                new XElement("accessDescription",
                    new XElement("method", "Ocsp"),
                    new XElement("location", "http://foo.com:2560/ocsp",
                        new XAttribute("type", "uniformResourceIdentifier"))
                )
            )
        );

        private AccessDesc testAccessDescription1 = new AccessDesc()
        {
            Method = AccessMethod.CAIssuers,
            Location = new OSCAGeneralName()
                {
                Name = "http://foo.com/issuer.crt",
                Type = GenName.uniformResourceIdentifier
                }
        };

        private AccessDesc testAccessDescription2 = new AccessDesc()
        {
            Method = AccessMethod.Ocsp,
            Location = new OSCAGeneralName()
            {
                Name = "http://foo.com:2560/ocsp",
                Type = GenName.uniformResourceIdentifier
            }
        };

        private string testStringOutput =
            "AuthorityInfoAccess (1.3.6.1.5.5.7.1.1)\n\tAccess Descriptions:\n\t\tCAIssuers: http://foo.com/issuer.crt (uniformResourceIdentifier)\n\t\tOcsp: http://foo.com:2560/ocsp (uniformResourceIdentifier)\n";

          
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
        ///A test for authorityInfoAccess empty Constructor
        ///</summary>
        [TestMethod()]
        public void authorityInfoAccessConstructorTest()
        {
            authorityInfoAccess target = new authorityInfoAccess();
            target.Add(testAccessDescription1);
            target.Add(testAccessDescription2);
            Assert.AreEqual(testXmlInput.ToString(), target.ToXml().ToString());
        }

        /// <summary>
        ///A test for authorityInfoAccess Constructor
        ///</summary>
        [TestMethod()]
        public void authorityInfoAccessConstructorTest1()
        {
            bool critical = false;
            authorityInfoAccess target = new authorityInfoAccess(critical);
            Assert.IsFalse(target.Critical);
        }

        /// <summary>
        ///A test for authorityInfoAccess XML Constructor
        ///</summary>
        [TestMethod()]
        public void authorityInfoAccessConstructorTest2()
        {
            authorityInfoAccess target = new authorityInfoAccess(testXmlInput);
            Assert.IsFalse(target.Critical);
            Assert.AreEqual(testXmlInput.ToString(), target.ToXml().ToString());
        }

        /// <summary>
        ///A test for authorityInfoAccess Constructor (from X509Extension)
        ///</summary>
        [TestMethod()]
        public void authorityInfoAccessConstructorTest3()
        {
            // Build an extension
            BcV3CertGen gen = new BcV3CertGen();
            authorityInfoAccess testExt = new authorityInfoAccess(testXmlInput);
            gen.AddExtension(testExt.OID, testExt.Critical, testExt.DerEncoding);
            X509Certificate cert = CertTestHarness.RunCertGenerator(gen);
            X509Extension ext = CertTestHarness.ExtractExtension(cert, X509Extensions.AuthorityInfoAccess);

            // Test code
            authorityInfoAccess target = new authorityInfoAccess(ext);
            List<AccessDesc> list = target.AuthInfoAccess;
            Assert.IsFalse(target.Critical);
            Assert.AreEqual(2, list.Count);
        }

        /// <summary>
        ///A test for encode
        ///</summary>
        [TestMethod()]
        public void encodeTest1()
        {
            authorityInfoAccess target = new authorityInfoAccess(testXmlInput);
            AuthorityInformationAccess actual = target.AuthorityInformationAccess;
            Assert.IsInstanceOfType(actual, typeof(AuthorityInformationAccess));
        }

        /// <summary>
        /// Test for ToString()
        /// </summary>
        [TestMethod()]
        public void toStringTest()
        {
            authorityInfoAccess target = new authorityInfoAccess(testXmlInput);
            string actual = target.ToString();
            Assert.AreEqual(testStringOutput, actual);
        }
    }
}
