using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace ClassLibrary
{
    public class Peer
    {
        private static string shortFileName = "";
        private static string fileName = "";

        public delegate void FileRecievedEventHandler(object source, double received);

        public event FileRecievedEventHandler DataReceived;

        private string uploadPath;
        private string downloadPath;

        public Peer(string uploadPath, string downloadPath)
        {
            this.uploadPath = uploadPath;
            this.downloadPath = downloadPath;
        }

        private void NewFileRecievedMethod(object sender, string fileName)
        {
            Console.WriteLine("New File Recieved\n" + fileName);
        }

        public void HandleIncomingFile(int port)
        {
            try
            {
                TcpListener tcpListener = new TcpListener(port);
                tcpListener.Start();
                while (true)
                {
                    Socket handlerSocket = tcpListener.AcceptSocket();
                    if (handlerSocket.Connected)
                    {
                        string fileName = string.Empty;
                        NetworkStream networkStream = new NetworkStream(handlerSocket);
                        int thisRead = 0;
                        double dataReceived = 0;
                        int blockSize = 1024;
                        Byte[] dataByte = new Byte[blockSize];
                        //lock (this)
                        //{
                        handlerSocket.Receive(dataByte);
                        int fileNameLen = BitConverter.ToInt32(dataByte, 0);
                        fileName = Encoding.ASCII.GetString(dataByte, 4, fileNameLen);
                        Stream fileStream = File.OpenWrite(downloadPath + fileName);
                        long size = fileStream.Length;
                        fileStream.Write(dataByte, 4 + fileNameLen, (1024 - (4 + fileNameLen)));
                        while (true)
                        {
                            thisRead = networkStream.Read(dataByte, 0, blockSize);
                            dataReceived += thisRead;
                            fileStream.Write(dataByte, 0, thisRead);
                            DataReceived(this, dataReceived);
                            if (thisRead == 0)
                                break;
                        }
                        DataReceived(this, -1);
                        fileStream.Close();

                        //}
                        handlerSocket = null;
                    }
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        public void HandleIncomingRequest(int port)
        {
            try
            {
                TcpListener tcpListener = new TcpListener(port);
                tcpListener.Start();
                while (true)
                {
                    Socket handlerSocket = tcpListener.AcceptSocket();
                    if (handlerSocket.Connected)
                    {
                        string fileName = string.Empty;
                        NetworkStream networkStream = new NetworkStream(handlerSocket);
                        int blockSize = 1024;
                        Byte[] dataByte = new Byte[blockSize];
                        handlerSocket.Receive(dataByte);
                        fileName = Encoding.ASCII.GetString(dataByte);
                        SendFile(handlerSocket.RemoteEndPoint.ToString(), 8002, @"D:\", fileName);
                        handlerSocket = null;
                    }
                }
            }
            catch
            {

            }
        }

        //TO DO: Implement Request
        public void SendRequest(string remoteHostIP, int remoteHostPort, string shortFileName)
        {
            try
            {
                if (!string.IsNullOrEmpty(remoteHostIP))

                {

                    byte[] fileNameByte = Encoding.ASCII.GetBytes(shortFileName);

                    byte[] requestNameData = new byte[fileNameByte.Length];

                    byte[] fileNameLen = BitConverter.GetBytes(fileNameByte.Length);

                    fileNameByte.CopyTo(requestNameData, 0);

                    TcpClient clientSocket = new TcpClient(remoteHostIP, remoteHostPort);

                    NetworkStream networkStream = clientSocket.GetStream();

                    networkStream.Write(requestNameData, 0, requestNameData.GetLength(0));
                    networkStream.Close();
                }
            }
            catch
            {

            }
        }

        public void SendFile(string remoteHostIP, int remoteHostPort, string uploadPath, string shortFileName)
        {
            try
            {
                if (!string.IsNullOrEmpty(remoteHostIP))

                {
                    string fileName = shortFileName.Trim('\0');
                    byte[] fileNameByte = Encoding.ASCII.GetBytes(fileName);
                    string path = uploadPath + fileName;
                    byte[] fileData = File.ReadAllBytes(path);

                    byte[] clientData = new byte[4 + fileNameByte.Length + fileData.Length];

                    byte[] fileNameLen = BitConverter.GetBytes(fileNameByte.Length);

                    fileNameLen.CopyTo(clientData, 0);

                    fileNameByte.CopyTo(clientData, 4);

                    fileData.CopyTo(clientData, 4 + fileNameByte.Length);

                    TcpClient clientSocket = new TcpClient(remoteHostIP.Split(':')[0], remoteHostPort);

                    NetworkStream networkStream = clientSocket.GetStream();

                    networkStream.Write(clientData, 0, clientData.GetLength(0));
                    networkStream.Close();
                }
            }
            catch (Exception e)
            {
                string ex = e.ToString();
            }
        }

    }
}
