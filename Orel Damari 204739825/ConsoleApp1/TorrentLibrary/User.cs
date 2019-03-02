using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace TorrentLibrary
{
    public class User
    {
        public string name { get; set; }
        public string password { get; set; }
        public string ip { get; set; }
        public int port { get; set; }
        public Dictionary<string, string> Files { get; set; }


        public User(ConfigData cfg)
        {
            this.name = cfg.Username;
            this.password = cfg.Password;
            this.ip = cfg.Ip;
            this.port = cfg.Port;
        }
        
        public User(string name, string password, string ip, int port, Dictionary<string, string> Files)
        {
            this.name = name;
            this.password = password;
            this.ip = ip;
            this.port = port;
            this.Files = Files;
        }

        public override string ToString()
        {
            var json = new JavaScriptSerializer().Serialize(this);
            return json.ToString();
        }

    }
}
