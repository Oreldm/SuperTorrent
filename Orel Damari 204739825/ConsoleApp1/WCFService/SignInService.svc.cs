using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web.Script.Serialization;
using TorrentLibrary;

namespace WCFService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SignInService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select SignInService.svc or SignInService.svc.cs at the Solution Explorer and start debugging.
    public class SignInService : ISignInService
    {
        public static List<User> users = new List<User>();
        public static Dictionary<string, string> fileMap = new Dictionary<string, string>(); //Name,Size

        //Add list of users active
        public bool login(string user, string password)
        {
            return DalService.getInstance().login(user, password);
        }

        public bool sendUserData(string userData)
        {
            JObject userAsJson = JObject.Parse(userData).ToObject<JObject>();
            int port = Int32.Parse(userAsJson.GetValue("port").ToString());
            string jsonDictionary = userAsJson.GetValue("Files").ToString();
            Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonDictionary);

            User user = new User(userAsJson.GetValue("name").ToString(), userAsJson.GetValue("password").ToString(), userAsJson.GetValue("ip").ToString(), port,
                values);
            users.Add(user);
            foreach (KeyValuePair<string, string> entry in values)
            {
                try
                {
                    fileMap.Add(entry.Key, entry.Value);
                } catch (Exception e)
                {
                    //TODO: add case that value with the same key added
                    Console.WriteLine("Value with the same key added");
                }

            }

            return true;
        }



        public List<string> getPeers(string fileName)
        {
            List<string> ret = new List<string>();
            foreach (User user in users)
            {
                if (user.Files.ContainsKey(fileName))
                {
                    /* JObject json = new JObject();
                     json.Add("name", user.name);
                     json.Add("ip", user.ip);
                     json.Add("port", user.port);
                     json.Add("fileSize", user.Files[fileName]);*/

                    //json.
                    ret.Add(user.ToString());
                }
            }
            return ret;
        }

        public List<string> getAllFiles()
        {
            List<string> ret = new List<string>();
            foreach (User user in users)
            {
                foreach (string filename in user.Files.Keys)
                {
                    if (!isFileNameAlreadyInList(filename, ret))
                    {
                        ret.Add(filename);
                    }
                }
            }

            return ret;
        }

        private bool isFileNameAlreadyInList(string fileName, List<string> files)
        {
            foreach (string file in files)
            {
                if (file.Equals(fileName))
                {
                    return true;
                }
            }
            return false;
        }

        public int getNumOfUsers()
        {
            return users.Count();
        }

        public string getActiveUsers()
        {
            string ret = "";
            foreach(User user in users)
            {
                ret += user.name;
                ret += "\n";
            }
            return ret;
        }

        public string getTotalUsers()
        {
            string ret = "";
            List<string>totalUsers = DalService.getInstance().TotalUsers();
            foreach(string s in totalUsers)
            {
                ret += s + "\n";
            }
            return ret;
         }

        public bool signOut(string userName)
        {
            foreach(User user in users)
            {
                if (user.name.Equals(userName))
                {
                    users.Remove(user);
                    return true;
                }
            }

            return false;
        }

        public void unusedFunc()
        {
            /*public List<string> findFile(string fileName)
{
    List<string> ret = new List<string>();
    foreach (KeyValuePair<string, string> entry in fileMap)
    {
        if (entry.Value.Equals(fileName))
        {
            ret.Add(entry.Key);
        }
    }
    return ret;
}*/

            /**   public List<string>findFile(string str)
   {
       List<string> ret = new List<string>();
       foreach (string key in fileMap.Keys)
       {
           if (key.Contains(str))
           {
               ret.Add(key);
           }
       }
       return ret;
   }**/

        }

    }
}
