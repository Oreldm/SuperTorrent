using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TorrentLibrary;

namespace WpfApp1
{
    public partial class FileManagerWindow : Window
    {
        private static List<JObject> peers;
        private static int file_size = -1;
        private static string fileToDownload;
        public FileManagerWindow()
        {
            InitializeComponent();
            this.MyFiles.Content = readFilesToStr();

            //Starting Async Server
            AsynchronousSocketListener.ipStr = MainWindow.configData.Ip;
            AsynchronousSocketListener.port = MainWindow.configData.Port;
            Thread thread1 = new Thread(AsynchronousSocketListener.StartListening);
            thread1.Start();
        }

        private void Search_Btn_Click(object sender, RoutedEventArgs e)
        {

            SignInService.SignInServiceClient siService = new SignInService.SignInServiceClient();
            string fileName = Search_TextBox.Text.ToString();
            string[] jsonUsers = siService.getPeers(fileName);
            List<JObject> userDataJson = new List<JObject>();
            for (int i = 0; i < jsonUsers.Length; i++)
            {
                Console.WriteLine("FILE USERS IS " + jsonUsers[i]);
                JObject jobj = JsonConvert.DeserializeObject<JObject>(jsonUsers[i]);
                if(!checkIfExists(jobj, userDataJson))
                {
                    userDataJson.Add(jobj);
                }
                
               // searchResult.Content = (jsonUsers[i]);
            }

            string ret = "";
            foreach(JObject j in userDataJson)
            {
                //  ret += Regex.Replace(j.ToString(), @"\t|\n|\r", "") + "\n";
                ret += "Peer Name: " + j.GetValue("name")+"\n";
            }
            try
            {
                JObject filesObject = JsonConvert.DeserializeObject<JObject>(userDataJson.First().GetValue("Files").ToString());
                fileToDownload = fileName;
                file_size = Int32.Parse(filesObject.GetValue(fileName)+"");
                ret += "Size: " + file_size;
                peers = userDataJson;
            }catch(Exception x)
            {
                fileToDownload = null;
                peers = null;
                file_size = -1;
                ret += "file not found!";
            }
            
            searchResult.Content = ret;
        }

        private bool checkIfExists(JObject jobj, List<JObject> userDataJson)
        {
            foreach(JObject j in userDataJson)
            {
                if (j.ToString().Equals(jobj.ToString()) ||
                    j.GetValue("name").Equals(jobj.GetValue("name")) ||
                    j.GetValue("ip").Equals(jobj.GetValue("ip")))
                {
                    return true;
                }
            }
            return false;
        }

        private string readFilesToStr()
        {

            Dictionary<string, string> fileMap = new Dictionary<string, string>();
            string[] fileNameDownload = Directory.GetFiles(MainWindow.configData.DownloadPath, "*")
             .Select(System.IO.Path.GetFileName).ToArray();
            string[] fileNameUpload = Directory.GetFiles(MainWindow.configData.UploadPath, "*")
             .Select(System.IO.Path.GetFileName).ToArray();

            string ret = "";
            for(int i=0; i < fileNameDownload.Length; i++)
            {
                ret += fileNameDownload[i] + "\n";
            }

            for (int i = 0; i < fileNameUpload.Length; i++)
            {
                ret += fileNameUpload[i] + "\n";
            }

            return ret;
        }

        private void Download_Button_Click(object sender, RoutedEventArgs e)
        {
            if (file_size == -1)
            {
                //Error box - theres not peers available
                return;
            }
            if (file_size == 0)
            {
                //just create the file locally
                return;
            }

            AsynchronousClient.totalFile = new List<string>();
            int numOfBitsEachPeer = file_size / peers.Count;
      
            int startedBit = 0;
            int index = 0;
            foreach(JObject peer in peers)
            {
                AsynchronousClient.totalFile.Add(null);
                string ip = peer.GetValue("ip").ToString();
                int port = Int32.Parse(peer.GetValue("port").ToString());
                int finalBit = startedBit + numOfBitsEachPeer;
                if (finalBit >= file_size)
                {
                    finalBit = file_size;
                    AsynchronousClient.StartClient(ip, port, startedBit, finalBit, fileToDownload,index);
                    index++;



                    //END OF FUNCTION
                    var result = String.Join("", AsynchronousClient.totalFile.ToArray());
                    byte[] bytes = Encoding.ASCII.GetBytes(result);
                    //To return back : string someString = Encoding.ASCII.GetString(bytes);

                    Console.WriteLine(result);

                    Console.WriteLine("Writing to : " + MainWindow.configData.DownloadPath + fileToDownload);
                   
                    //Write to file
                    Console.WriteLine(ByteArrayToFile(MainWindow.configData.DownloadPath + fileToDownload, bytes));

                    AsynchronousClient.totalFile = null;

                    return;
                }

                AsynchronousClient.StartClient(ip, port, startedBit, finalBit, fileToDownload,index);
                startedBit += numOfBitsEachPeer;
                index++;
            }
        }


        public static bool ByteArrayToFile(string fileName, byte[] byteArray)
        {
            try
            {
                using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(byteArray, 0, byteArray.Length);
                    fs.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in process: {0}", ex);
                return false;
            }
        }
    }

}
