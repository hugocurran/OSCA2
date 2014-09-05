using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace OSCA.Crypto
{
    class SysCSP
    {
        [DllImport("Advapi32.dll")]
        internal static extern bool CryptEnumProviders(
            int dwIndex,
            IntPtr pdwReserved,
            int dwFlags,
            ref int pdwProvType,
            StringBuilder pszProvName,
            ref int pcbProvName);

        public static Dictionary<string, int> ReadAllProviders()
        {
            Dictionary<string, int> installedCSPs = new Dictionary<string, int>();
            int cbName;
            int dwType;
            int dwIndex;
            StringBuilder pszName;
            dwIndex = 0;
            dwType = 1;
            cbName = 0;
            while (CryptEnumProviders(dwIndex, IntPtr.Zero, 0, ref dwType, null, ref cbName))
            {
                pszName = new StringBuilder(cbName);

                if (CryptEnumProviders(dwIndex++, IntPtr.Zero, 0, ref dwType, pszName, ref cbName))
                {
                    installedCSPs.Add(pszName.ToString(), dwType);
                }
            }
            return installedCSPs;
        }
    }
}
