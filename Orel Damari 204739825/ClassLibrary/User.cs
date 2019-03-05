using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ClassLibrary
{

    public class User
    {
        public string name { get; set; }
        public string password { get; set; }
        public string mail { get; set; }
        public string ip { get; set; }
        public int port { get; set; }
        public bool isActive { get; set; }
        public Dictionary<string, long> userFiles { get; set; }

        public User() { }

        public User(string name, string password, string mail)
        {
            this.name = name;
            this.password = password;
            this.mail = mail;
        }

        public User(ConfigData configData)
        {
            this.ip = configData.Ip;
            this.port = configData.Port;
            this.name = configData.Username;
            this.password = configData.Password;
            this.mail = configData.Email.ToLower();
            this.isActive = false;
            this.userFiles = new Dictionary<string, long>();

            loadFiles(configData.UploadPath);
        }

        public int GetIsActive()
        {
            return isActive ? 1 : 0;
        }

        public void loadFiles(string uploadPath)
        {
            var dir = new DirectoryInfo(uploadPath);

            foreach (FileInfo f in dir.GetFiles())
            {
                if (!userFiles.ContainsKey(f.Name))
                    userFiles.Add(f.Name, f.Length);
            }
        }

        public override string ToString()
        {
            return "Name: " + name + ", Mail: " + mail;
        }

        public override int GetHashCode()
        {
            return this.mail.GetHashCode();
        }
    }
}
