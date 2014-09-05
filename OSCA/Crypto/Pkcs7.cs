using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.BouncyCastle.Cms;
using System.Collections;
using Org.BouncyCastle.X509.Store;

namespace OSCA.Crypto
{
    /// <summary>
    /// Static class containing tools to handle PKCS#7 objects
    /// </summary>
    public static class Pkcs7
    {
        /// <summary>
        /// Returns a new PKCS#7 instance containing certificates and (optionally CRLs)
        /// </summary>
        /// <param name="CertificateList">The certificate list.</param>
        /// <param name="CrlList">The CRL list.</param>
        /// <returns>
        /// PKCS#7 instance
        /// </returns>
        /// <exception cref="System.ArgumentException">No input</exception>
        public static CmsSignedData Create(ArrayList CertificateList, ArrayList CrlList)
        {
            if ((CertificateList == null) && (CrlList == null))
                throw new ArgumentException("No input");

            CmsSignedDataGenerator p7Gen = new CmsSignedDataGenerator();

            IX509Store certs = X509StoreFactory.Create("CERTIFICATE/COLLECTION", new X509CollectionStoreParameters(CertificateList));
            p7Gen.AddCertificates(certs);

            // If CRL is required
            if (CrlList != null)
            {
                IX509Store crls = X509StoreFactory.Create("CRL/COLLECTION", new X509CollectionStoreParameters(CrlList));
                p7Gen.AddCrls(crls);
            }

            return p7Gen.Generate(null);
        }
    }
}


