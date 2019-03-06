using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Forms;
using TorrentLibrary;
using System.Security.Cryptography;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using System.Reflection;

namespace WpfApp1
{

    public partial class MainWindow : Window
    {
        public static String CONFIGURATION_FILE_NAME = "Config2018.xml";
        public static String CONFIGURATION_PATH = "C:\\Users\\oreldm\\Desktop\\Orel Damari 204739825\\Config\\";
        public static ConfigData configData = new ConfigData();
        public static User user;
        public static FileManagerWindow fileManagerWindow;

        public MainWindow()
        {
            this.InitializeComponent();
            this.LoadConfig();
        }

        private void SignIn_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (fileManagerWindow != null)
                fileManagerWindow = null;

            SignInService.SignInServiceClient siService = new SignInService.SignInServiceClient();
            user = new User(configData);

            bool isLogin = siService.login(user.name, user.password);
            if (!isLogin)
            {
                System.Windows.MessageBox.Show("Error in sign in.");
                return;
            }
            user.Files=readUsersFiles();
            var json = new JavaScriptSerializer().Serialize(user);
            Console.WriteLine(json.ToString());
            siService.sendUserData(json.ToString());
            fileManagerWindow = new FileManagerWindow();
            fileManagerWindow.ShowDialog();
            
        }

        private Dictionary<string,string> readUsersFiles()
        {
            Dictionary<string, string> fileMap = new Dictionary<string, string>();
            string[] fileName = Directory.GetFiles(configData.UploadPath, "*")
             .Select(Path.GetFileName).ToArray();

            for(int i = 0; i < fileName.Length; i++)
            {
                Console.WriteLine(fileName[i]);
                try
                {
                    long length = new System.IO.FileInfo(configData.UploadPath + fileName[i]).Length;
                    fileMap.Add(fileName[i], length+"");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Same File Name Twice");
                }
            }

            return fileMap;
        }

        private void Settings_Btn_Click(object sender, RoutedEventArgs e)
        {
            SettingWindow settings = new SettingWindow(configData);
            settings.ShowDialog();
            settings.Closed += new EventHandler(loadConfigEventHandler);
        }

        private void loadConfigEventHandler(object sender, EventArgs args)
        {
            LoadConfig();
        }

        private void LoadConfig()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ConfigData));
                StreamReader reader = new StreamReader(CONFIGURATION_PATH + CONFIGURATION_FILE_NAME);
                configData = (ConfigData)serializer.Deserialize(reader);
                reader.Close();
                AsynchronousSocketListener.filePath = configData.UploadPath;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("You have problem in the config\nplease fix it in the settings button", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("PROBLEM IN CONFIG");
            }



            //Reflection That prints the hour
            try
            {
                var DLL = Assembly.LoadFile("C:\\DownloadPath\\TorrentLibrary.dll");

                foreach (Type type in DLL.GetExportedTypes())
                {
                    Console.WriteLine(type);
                    if (type.ToString().Contains("Class1"))
                    {
                        var c = Activator.CreateInstance(type);
                        Console.WriteLine("HERE IS REFLECTION STRING: ");
                        type.InvokeMember("PrintHour", BindingFlags.InvokeMethod, null, c, new object[] { @"I dont actually need to send this string" });
                    }
                }
            }
            catch (Exception e) { }


            Console.ReadLine();


        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            SignInService.SignInServiceClient siService = new SignInService.SignInServiceClient();
            try
            {
                siService.signOut(user.name);
            }catch(Exception g)
            { 
                //username is null or no username in the server
            }

            e.Cancel = false;
        }

        //UNUSED FUNCTIONS
        private void md5Ret()
        {
            /*using (var md5 = MD5.Create())
              {
        using (var stream = File.OpenRead(configData.UploadPath+fileName[i]))
        {
            var md5Str = Encoding.Default.GetString(md5.ComputeHash(stream));
            try
            {

                fileMap.Add(fileName[i], md5Str);
            }catch(Exception e)
            {
                Console.WriteLine("Same File Name Twice");
            }

          }
              }*/
        }

    }
}
