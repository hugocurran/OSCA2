/*
 * Copyright 2011-14 Peter Curran (peter@currans.eu). All rights reserved.
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
using System.Windows.Forms;
using OSCA.Profile;

namespace OSCASnapin.Profiles
{
    public static class ProfileEditor
    {
        /// <summary>
        /// Create an extension and add to the profile
        /// </summary>
        /// <param name="profile">Profile object</param>
        /// <param name="Extension">Extension name</param>
        internal static bool CreateExtension(Profile profile, string Extension)
        {
            bool ok = true;
            switch (Extension)
            {
                case "Basic Constraints":
                    BasicConstraints bc = new BasicConstraints(null);
                    if (bc.ShowDialog() == DialogResult.OK)
                    {
                        try 
                        { 
                            profile.AddExtension(bc.basCon); 
                        }
                        catch (ArgumentException) 
                        { 
                            ok = false;
                            MessageBox.Show("Duplicate extensions not permitted", "Profile Editor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    else
                        ok = false; // User clicked cancel
                    break;

                case "Key Usage":
                    KeyUsage ku = new KeyUsage(null);
                    if (ku.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            profile.AddExtension(ku.keyUsage);
                        }
                        catch (ArgumentException) 
                        { 
                            ok = false;
                            MessageBox.Show("Duplicate extensions not permitted", "Profile Editor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    else
                        ok = false;
                    break;

                case "Extended Key Usage":
                    ExtendedKeyUsage eku = new ExtendedKeyUsage(null);
                    if (eku.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            profile.AddExtension(eku.extKeyUsage);
                        }
                        catch (ArgumentException)
                        {
                            ok = false;
                            MessageBox.Show("Duplicate extensions not permitted", "Profile Editor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    else
                        ok = false;
                    break;

                case "CRL Distribution Points":
                    CrlDistributionPoint cdp = new CrlDistributionPoint(null);
                    if (cdp.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            profile.AddExtension(cdp.crlDP);
                        }
                        catch (ArgumentException)
                        {
                            ok = false;
                            MessageBox.Show("Duplicate extensions not permitted", "Profile Editor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    else
                        ok = false;
                    break;

                case "Subject Alternative Name":
                    SubjectAltNames san = new SubjectAltNames(null);
                    if (san.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            profile.AddExtension(san.san);
                        }
                        catch (ArgumentException)
                        {
                            ok = false;
                            MessageBox.Show("Duplicate extensions not permitted", "Profile Editor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    else
                        ok = false;
                    break;

                case "Issuer Alternative Name":
                    IssuerAltNames ian = new IssuerAltNames(null);
                    if (ian.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            profile.AddExtension(ian.ian);
                        }
                        catch (ArgumentException)
                        {
                            ok = false;
                            MessageBox.Show("Duplicate extensions not permitted", "Profile Editor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    else
                        ok = false;
                    break;

                case "Authority Information Access":
                    AuthorityInfoAccess aia = new AuthorityInfoAccess(null);
                    if (aia.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            profile.AddExtension(aia.aia);
                        }
                        catch (ArgumentException)
                        {
                            ok = false;
                            MessageBox.Show("Duplicate extensions not permitted", "Profile Editor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    else
                        ok = false;
                    break;

                case "Subject Information Access":
                    SubjectInfoAccess sia = new SubjectInfoAccess(null);
                    if (sia.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            profile.AddExtension(sia.sia);
                        }
                        catch (ArgumentException)
                        {
                            ok = false;
                            MessageBox.Show("Duplicate extensions not permitted", "Profile Editor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    else
                        ok = false;
                    break;

                case "Name Constraints":
                    NameConstraints nc = new NameConstraints(null);
                    if (nc.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            profile.AddExtension(nc.nc);
                        }
                        catch (ArgumentException)
                        {
                            ok = false;
                            MessageBox.Show("Duplicate extensions not permitted", "Profile Editor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    else
                        ok = false;
                    break;

                case "Certificate Policies":
                    CertificatePolicies cp = new CertificatePolicies(null);
                    if (cp.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            profile.AddExtension(cp.cp);
                        }
                        catch (ArgumentException)
                        {
                            ok = false;
                            MessageBox.Show("Duplicate extensions not permitted", "Profile Editor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    else
                        ok = false;
                    break;

                case "Policy Mappings":
                    // Can't process this until CertPolicies is defined
                    certificatePolicies policy = (certificatePolicies)FindExtension(profile, typeof(certificatePolicies));
                    if (policy == null)
                    {
                        MessageBox.Show("You must define Certificate Policies before adding this extension", "Policy Mappings", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ok = false;
                    }
                    else
                    {
                        PolicyMappings map = new PolicyMappings(null, policy);
                        if (map.ShowDialog() == DialogResult.OK)
                        {
                            try
                            {
                                profile.AddExtension(map.mapping);
                            }
                            catch (ArgumentException)
                            {
                                ok = false;
                                MessageBox.Show("Duplicate extensions not permitted", "Profile Editor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                        }
                        else
                            ok = false;
                    }
                    break;

                case "OCSP Nocheck":
                    OcspNocheck on = new OcspNocheck(null);
                    if (on.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            profile.AddExtension(on.ocsp);
                        }
                        catch (ArgumentException)
                        {
                            ok = false;
                            MessageBox.Show("Duplicate extensions not permitted", "Profile Editor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    else
                        ok = false;
                    break;

                case "Inhibit AnyPolicy":
                    InhibitAnyPolicy iap = new InhibitAnyPolicy(null);
                    if (iap.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            profile.AddExtension(iap.iAnypol);
                        }
                        catch (ArgumentException)
                        {
                            ok = false;
                            MessageBox.Show("Duplicate extensions not permitted", "Profile Editor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    else
                        ok = false;
                    break;

                case "Policy Constraints":
                    PolicyConstraints pc = new PolicyConstraints(null);
                    if (pc.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            profile.AddExtension(pc.polCon);
                        }
                        catch (ArgumentException)
                        {
                            ok = false;
                            MessageBox.Show("Duplicate extensions not permitted", "Profile Editor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    else
                        ok = false;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(Extension);
            }
            return ok;
        }

        /// <summary>
        /// Remove an extension from the profile
        /// </summary>
        /// <param name="profile">Profile object</param>
        /// <param name="Extension">Extension name</param>
        internal static void RemoveExtension(Profile profile, string Extension)
        {
            switch (Extension)
            {
                case "Basic Constraints":
                    profile.RemoveExtension(FindExtension(profile, typeof(basicConstraints)));
                    break;

                case "Key Usage":
                    profile.RemoveExtension(FindExtension(profile, typeof(keyUsage)));
                    break;

                case "Extended Key Usage":
                    profile.RemoveExtension(FindExtension(profile, typeof(extendedKeyUsage)));
                    break;

                case "CRL Distribution Points":
                    profile.RemoveExtension(FindExtension(profile, typeof(CrlDistributionPoint)));
                    break;

                case "Subject Alternative Name":
                    profile.RemoveExtension(FindExtension(profile, typeof(subjectAltName)));
                    break;

                case "Issuer Alternative Name":
                    profile.RemoveExtension(FindExtension(profile, typeof(issuerAltName)));
                    break;

                case "Authority Information Access":
                    profile.RemoveExtension(FindExtension(profile, typeof(authorityInfoAccess)));
                    break;

                case "Subject Information Access":
                    profile.RemoveExtension(FindExtension(profile, typeof(subjectInfoAccess)));
                    break;

                case "Name Constraints":
                    profile.RemoveExtension(FindExtension(profile, typeof(nameConstraints)));
                    break;

                case "Certificate Policies":
                    // Remove PolicyMappings as well as the two are linked
                    if (FindExtension(profile, typeof(policyMappings)) != null)
                    {
                        if (MessageBox.Show("Removing Policy Mappings extension as well", "Certificate Policies", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            RemoveExtension(profile, "Policy Mappings");
                            break;
                        }
                    }
                    profile.RemoveExtension(FindExtension(profile, typeof(certificatePolicies)));
                    break;

                case "Policy Mappings":
                    profile.RemoveExtension(FindExtension(profile, typeof(policyMappings)));
                    break;

                case "OCSP Nocheck":
                    profile.RemoveExtension(FindExtension(profile, typeof(ocspNocheck)));
                    break;

                case "Inhibit AnyPolicy":
                    profile.RemoveExtension(FindExtension(profile, typeof(inhibitAnyPolicy)));
                    break;

                case "Policy Constraints":
                    profile.RemoveExtension(FindExtension(profile, typeof(policyConstraints)));
                    break;

                default:
                    throw new ArgumentOutOfRangeException(Extension);
            }
        }

        /// <summary>
        /// Edit an extension within a profile
        /// </summary>
        /// <param name="profile">Profile object</param>
        /// <param name="Extension">Extension name</param>
        internal static void EditExtension(Profile profile, string Extension)
        {
            switch (Extension)
            {
                case "Basic Constraints":
                    BasicConstraints bc = new BasicConstraints((basicConstraints)FindExtension(profile, typeof(basicConstraints)));
                    bc.ShowDialog();
                    break;

                case "Key Usage":
                    KeyUsage ku = new KeyUsage((keyUsage)FindExtension(profile, typeof(keyUsage)));
                    ku.ShowDialog();
                    break;

                case "Extended Key Usage":
                    ExtendedKeyUsage eku = new ExtendedKeyUsage((extendedKeyUsage)FindExtension(profile, typeof(extendedKeyUsage)));
                    eku.ShowDialog();
                    break;

                case "CRL Distribution Points":
                    CrlDistributionPoint cdp = new CrlDistributionPoint((crlDistPoint)FindExtension(profile, typeof(crlDistPoint)));
                    cdp.ShowDialog();
                    break;

                case "Subject Alternative Name":
                    SubjectAltNames san = new SubjectAltNames((subjectAltName)FindExtension(profile, typeof(subjectAltName)));
                    san.ShowDialog();
                    break;

                case "Issuer Alternative Name":
                    IssuerAltNames ian = new IssuerAltNames((issuerAltName)FindExtension(profile, typeof(issuerAltName)));
                    ian.ShowDialog();
                    break;

                case "Authority Information Access":
                    AuthorityInfoAccess aia = new AuthorityInfoAccess((authorityInfoAccess)FindExtension(profile, typeof(authorityInfoAccess)));
                    aia.ShowDialog();
                    break;

                case "Subject Information Access":
                    SubjectInfoAccess sia = new SubjectInfoAccess((subjectInfoAccess)FindExtension(profile, typeof(subjectInfoAccess)));
                    sia.ShowDialog();
                    break;

                case "Name Constraints":
                    NameConstraints nc = new NameConstraints((nameConstraints)FindExtension(profile, typeof(nameConstraints)));
                    nc.ShowDialog();
                    break;

                case "Certificate Policies":
                    CertificatePolicies cp = new CertificatePolicies((certificatePolicies)FindExtension(profile, typeof(certificatePolicies)));
                    cp.ShowDialog();
                    break;

                case "Policy Mappings":
                    PolicyMappings map = new PolicyMappings((policyMappings)FindExtension(profile, typeof(policyMappings)), 
                        (certificatePolicies)FindExtension(profile, typeof(certificatePolicies)));
                    map.ShowDialog();
                    break;

                case "OCSP Nocheck":
                    OcspNocheck on = new OcspNocheck((ocspNocheck)FindExtension(profile, typeof(ocspNocheck)));
                    on.ShowDialog();
                    break;

                case "Inhibit AnyPolicy":
                    InhibitAnyPolicy iap = new InhibitAnyPolicy((inhibitAnyPolicy)FindExtension(profile, typeof(inhibitAnyPolicy)));
                    iap.ShowDialog();
                    break;

                case "Policy Constraints":
                    PolicyConstraints pc = new PolicyConstraints((policyConstraints)FindExtension(profile, typeof(policyConstraints)));
                    pc.ShowDialog();
                    break;

                default:
                    break;
            }
        }

        // Find an extension in the profile
        private static ProfileExtension FindExtension(Profile profile, Type obj)
        {
            foreach (ProfileExtension ext in profile.Extensions)
            {
                if (ext.GetType() == obj)
                {
                    return ext;
                }
            }
            return null;
        }
    }
}
