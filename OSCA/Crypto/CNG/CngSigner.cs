using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace OSCA.Crypto.CNG
{
    class CngSigner
    {

        internal static byte[] Sign(byte[] buffer, CngKey key, CngAlgorithm hashAlgorithm)
        {
            if (key.AlgorithmGroup != CngAlgorithmGroup.ECDsa)
                throw new ArgumentOutOfRangeException("Can only sign using ECDsa");
  
            ECDsaCng ecdsa = new ECDsaCng(key);

            ecdsa.HashAlgorithm = hashAlgorithm;
            return ecdsa.SignData(buffer);
        }
    }
}
