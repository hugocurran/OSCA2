/*
 * Copyright 2011 Peter Curran (peter@currans.eu). All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
 *
 * 1. Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
 *
 * 2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the 
 *    documentation and/or other materials provided with the distribution.
 *
 * THIS SOFTWARE IS PROVIDED BY PETER CURRAN "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL PETER CURRAN OR CONTRIBUTORS BE 
 * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
 * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN 
 * IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 * The views and conclusions contained in the software and documentation are those of the author alone. 
 */

using System;


namespace OSCA
{
    /// <summary>
    /// RFC 5280 revocation reasons
    /// </summary>
    public enum CRLReason
    {
        /// <summary>
        /// Reasons unused or undefined
        /// </summary>
        unused = 0,
        /// <summary>
        /// Entity key has been compromised
        /// </summary>
        keyCompromise = 1,
        /// <summary>
        /// Issuing CA key compromised
        /// </summary>
        cACompromise = 2,
        /// <summary>
        /// Entity affiliation has changed
        /// </summary>
        affiliationChanged = 3,
        /// <summary>
        /// Certificate superseded
        /// </summary>
        supersede = 4,
        /// <summary>
        /// Certificate permamently out of use
        /// </summary>
        cessationOfOperation = 5,
        /// <summary>
        /// Certificate temporarily out of use
        /// </summary>
        certificateHold = 6
    }

    /// <summary>
    /// Class to access revocation reason codes
    /// </summary>
    public static class CrlReason
    {
        /// <summary>
        /// Get string associated with a revocation reason code
        /// </summary>
        /// <param name="ReasonCode">String containing numeric reason code</param>
        /// <returns>String translation of the reason code</returns>
        public static string GetReason(string ReasonCode)
        {
            int numeric = (int)Convert.ToUInt32(ReasonCode);
            return GetReason((CRLReason)numeric);
        }

        /// <summary>
        /// Get string associated with a revocation reason code
        /// </summary>
        /// <param name="ReasonCode">CRLReason enum value</param>
        /// <returns>String translation of the reason code</returns>
        public static string GetReason(CRLReason ReasonCode)
        {
            switch (ReasonCode)
            {
                case CRLReason.unused:
                    return "Unknown";

                case CRLReason.keyCompromise:
                    return "Key Compromise";

                case CRLReason.cACompromise:
                    return "CA Compromise";

                case CRLReason.affiliationChanged:
                    return "Affiliation Changed";

                case CRLReason.supersede:
                    return "Superseded";

                case CRLReason.cessationOfOperation:
                    return "Cessation of Operation";

                case CRLReason.certificateHold:
                    return "Certificate Hold";

            }
            return "";
        }
    }
}
