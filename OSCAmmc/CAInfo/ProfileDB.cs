/*
 * Copyright 2011-12 Peter Curran (peter@currans.eu). All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
 *
 * 1. Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
 *
 * 2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the 
 *    documentation and/or other materials provided with the distribution.
 *
 * THIS SOFTWARE IS PROVIDED BY THE FREEBSD PROJECT "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE FREEBSD PROJECT OR CONTRIBUTORS BE 
 * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
 * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN 
 * IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 * The views and conclusions contained in the software and documentation are those of the author alone. 
 */

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using OSCA.Profile;


namespace OSCASnapin.CAinfo
{
    public struct ProfileDb
    {
        public Profile profile;
        public string file;
    }

    public class profileDB
    {
        private List<ProfileDb> profiles;

        /// <summary>
        /// A List of profiles used by the CA
        /// </summary>
        public List<ProfileDb> Profiles { get { return profiles; } }

        #region Constructors

        /// <summary>
        /// Construct a Profiles Database object by reading in all the Profiles from the CA directory
        /// Convert to a display-friendly format
        /// </summary>
        /// <param name="caInfo">CA Information object</param>
        public profileDB(CaControl caInfo) : this(caInfo.ProfilesLocation) { }

        /// <summary>
        /// Construct a Profiles Database object by reading in all the Profiles from the CA directory
        /// Convert to a display-friendly format
        /// </summary>
        /// <param name="ProfilesLocation">Pathname of folder containg the profiles</param>
        public profileDB(string ProfilesLocation)
        {
            profiles = new List<ProfileDb>();

            string[] files = Directory.GetFiles(ProfilesLocation, "*.xml");

            foreach (string file in files)
            {
                XDocument profile = XDocument.Load(file);

                IEnumerable<XElement> records;

                // Ignore .xml files that are not profiles
                if (extensions.GetStringFromChildElement(profile.Element("OSCA"), "Profile") == "")
                    continue;

                records = profile.Element("OSCA").Descendants("Profile");
                if (records.Count() != 0)
                {
                    ProfileDb entry = new ProfileDb();
                    entry.profile = new Profile(profile);
                    entry.file = file;
                    profiles.Add(entry);
                }
            }
        }

        #endregion

        public void Add(ProfileDb newEntry)
        {
            profiles.Add(newEntry);
        }

        public void Remove(ProfileDb entry)
        {
            profiles.Remove(entry);
        }
    }

    public static class extensions
    {
        /// <summary>
        /// If the parent element contains an element of the specified name, it returns the value of that element.
        /// </summary>
        /// <param name="x">The parent element.</param>
        /// <param name="elementName">The name of the child element to check for.</param>
        /// <returns>The value of the child element if it exists, or an empty string if it doesn't.</returns>
        public static string GetStringFromChildElement(this XElement x, string elementName)
        {
            return ((string)x.Element(elementName)) ?? "";
        }
    }
}
