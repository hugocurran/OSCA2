using System;
using System.Collections.Generic;

namespace OSCASnapin
{
    [Serializable()]
    public class InitialisationData
    {
        public Dictionary<string, bool> userPriv;
        public Dictionary<string, bool> adminPriv;
        public string configFolder;
        public string configFile;
        public byte[] adminPasswordHash;

        public InitialisationData() 
        {
            // Set default privilege values
            userPriv = new Dictionary<string, bool>() {
                {"CreateIntCA", false},
                {"CreateExtCA", false},
                {"AddCA", false},
                {"DeleteCA", false},
                {"ExportCAKey", false},
                {"ImportCA", false},
                {"StartCA", true},
                {"StopCA", true},
                {"IssueCert", true},
                {"IssueCRL", true},
                {"RevokeCert", true},
                //{"UnRevokeCert", false},
                {"RekeyCert", true},
                {"RenewCert", false},
                {"ViewCert", true},
                {"ExportCert", true},
                {"CreateProfile", false},
                {"EditProfile", false},
                {"ViewXMLProfile", true},
                {"CopyProfile", false},
                {"DeleteProfile", false}                
            };

            adminPriv = new Dictionary<string, bool>() {
                {"CreateIntCA", true},
                {"CreateExtCA", false},
                {"AddCA", true},
                {"DeleteCA", true},
                {"ExportCAKey", true},
                {"ImportCA", true},
                {"StartCA", false},
                {"StopCA", false},
                {"IssueCert", false},
                {"IssueCRL", false},
                {"RevokeCert", false},
                //{"UnRevokeCert", true},
                {"RekeyCert", false},
                {"RenewCert", false},
                {"ViewCert", true},
                {"ExportCert", true},
                {"CreateProfile", true},
                {"EditProfile", true},
                {"ViewXMLProfile", true},
                {"CopyProfile", true},
                {"DeleteProfile", true}
            };
        }

        public Dictionary<string, bool> UserPriv
        {
            get { return userPriv; }
        }

        public Dictionary<string, bool> AdminPriv
        {
            get { return adminPriv; }
        }

    }
}
