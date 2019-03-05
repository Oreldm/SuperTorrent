using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TorrentLibrary
{
    [Serializable()]
    public class ConfigData
    {
        [System.Xml.Serialization.XmlElement("Ip")]
        public string Ip { get; set; }

        [System.Xml.Serialization.XmlElement("Port")]
        public int Port { get; set; }

        [System.Xml.Serialization.XmlElement("Username")]
        public string Username { get; set; }

        [System.Xml.Serialization.XmlElement("Password")]
        public string Password { get; set; }

        [System.Xml.Serialization.XmlElement("UploadPath")]
        public string UploadPath { get; set; }

        [System.Xml.Serialization.XmlElement("DownloadPath")]
        public string DownloadPath { get; set; }
    }
}
