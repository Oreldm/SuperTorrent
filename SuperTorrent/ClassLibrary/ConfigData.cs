using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ClassLibrary
{
    public class ConfigData
    {
        public string Ip { get; set; }

        public int Port { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string UploadPath { get; set; }

        public string DownloadPath { get; set; }
    }
}
