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
            Download_ProgressBar.Value = 0;
            SignInService.SignInServiceClient siService = new SignInService.SignInServiceClient();
            string fileName = Search_TextBox.Text.ToString();
            if (fileName.Equals("*"))
            {
                file_size = -1;
                string[] files= siService.getAllFiles();
                for(int i = 0; i < files.Length; i++)
                {
                    searchResult.Content += files[i]+"\n";
                }
                return;
            }
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
                
            }

            string ret = "";
            ret += "Number of peers: " + userDataJson.Count + "\n";
            foreach (JObject j in userDataJson)
            {
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

/*        public bool changeToUpload(string fileName)
        {
            string ret ="/n"+ fileName + " is being uploaded";
            this.Dispatcher.Invoke(() =>
            {
                this.MyFiles.Content = readFilesToStr() + ret;
            });
                
            return true;
        }*/

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

            for (int i = 0; i < fileNameDownload.Length; i++)
            {
                fileMap.Add(fileNameDownload[i], fileNameDownload[i]);
            }

            for (int i = 0; i < fileNameUpload.Length; i++)
            {
                try
                {
                    fileMap.Add(fileNameUpload[i], fileNameUpload[i]);
                }
                catch (Exception e) { }
            }


            string ret = "";

            foreach(string key in fileMap.Keys)
            {
                ret += key + "\n";
            }

            return ret;
        }

        private void Download_Button_Click(object sender, RoutedEventArgs e)
        {
            Download_ProgressBar.Value = 0;
            if (file_size == -1)
            {
                //Unable to download
                MessageBox.Show("You can download only after you search for 1 file that has found",
                    "Unable to download",
                    MessageBoxButton.OK);
                return;
            }
            if (file_size == 0)
            {
                //Find is empty so just creating it locally
                byte[] bytes = new byte[0];
                ByteArrayToFile(MainWindow.configData.DownloadPath + fileToDownload, bytes);
                try
                {
                    System.IO.File.Copy(MainWindow.configData.DownloadPath + fileToDownload, MainWindow.configData.UploadPath + fileToDownload); //become resource
                }catch(Exception ex) { Console.WriteLine("file already exists"); }
                return;
            }

            AsynchronousClient.totalFile = new List<byte[]>();
            if (peers.Count == 1)
            {
                peers.Add(peers[0]);
            }
            int numOfBitsEachPeer = file_size / peers.Count;

            int startedBit = 0;
            int index = 0;
            foreach(JObject peer in peers)
            {
                AsynchronousClient.connectDone = new ManualResetEvent(false);
                AsynchronousClient.sendDone = new ManualResetEvent(false);
                AsynchronousClient.receiveDone = new ManualResetEvent(false);
                AsynchronousClient.totalFile.Add(null);
                string ip = peer.GetValue("ip").ToString();
                int port = Int32.Parse(peer.GetValue("port").ToString());
                int finalBit = startedBit + numOfBitsEachPeer;
                if (index == peers.Count - 1)
                {
                    finalBit= finalBit + numOfBitsEachPeer;
                }
                
                if (finalBit >= file_size)
                {
                    finalBit = file_size;
                    AsynchronousClient.StartClient(ip, port, startedBit, finalBit, fileToDownload,index);
                    index++;
                    
                    int lengthOfFile = 0;
                  foreach (byte[] a in AsynchronousClient.totalFile)
                    {
                        for(int i = 0; i < a.Length; i++)
                        {
                            lengthOfFile++;
                        }
                    }
                    byte[] bytes = new byte[lengthOfFile];
                    int indexInByteArray = 0;
                    foreach (byte[] a in AsynchronousClient.totalFile)
                    {
                        for (int i = 0; i < a.Length; i++)
                        {
                            bytes[indexInByteArray] = a[i];
                            indexInByteArray++;
                        }
                    }

                    Console.WriteLine("Writing to : " + MainWindow.configData.DownloadPath + fileToDownload);

                    //Write to file
                    ByteArrayToFile(MainWindow.configData.DownloadPath + fileToDownload, bytes);

                    AsynchronousClient.totalFile = null;
                    Download_ProgressBar.Value = 100;
                    return;
                }

                AsynchronousClient.StartClient(ip, port, startedBit, finalBit, fileToDownload,index);
                startedBit += numOfBitsEachPeer; //first mistake that happened and I fixed (the +1)
                index++;

                Download_ProgressBar.Value +=100/peers.Count;
            }

            try
            {
                System.IO.File.Copy(MainWindow.configData.DownloadPath + fileToDownload, MainWindow.configData.UploadPath + fileToDownload); //become resource
            }
            catch (Exception ex) { Console.WriteLine("file already exists"); }
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

        private void Download_ProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
    }

}
