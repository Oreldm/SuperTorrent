using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace TorrentLibrary
{

    public class AsynchronousClient
    {
        public static List<byte[]> totalFile =null;

        public static ManualResetEvent connectDone =
            new ManualResetEvent(false);
        public static ManualResetEvent sendDone =
            new ManualResetEvent(false);
        public static ManualResetEvent receiveDone =
            new ManualResetEvent(false);

        private static String response = String.Empty;
        private static byte[] byteData;

        //Main Method is StartClient
        public static void StartClient(string ipAddrStr, int port, int startByte, int endByte, string fileName, int index)
        {

            try
            {
                IPAddress ipAddress = IPAddress.Parse(ipAddrStr);
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

                Socket client = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                client.BeginConnect(remoteEP,
                    new AsyncCallback(ConnectCallback), client);
                connectDone.WaitOne();

                // HERE TO SEND THE JSON WITH EOF AT THE END
                JObject json = new JObject();
                json.Add("Start", startByte);
                json.Add("End", endByte);
                json.Add("Filename", fileName);

                Send(client, json+"<EOF>");
                sendDone.WaitOne();

                // Receive the response from the remote device.  
                Receive(client);
                receiveDone.WaitOne(); //Problem is here
                totalFile.Insert(index, byteData);
                totalFile.RemoveAt(index + 1);
                Console.WriteLine("Response received : {0}", response);

                //Here write it to a list 
                //When the list is full then you can write it to a file

                client.Shutdown(SocketShutdown.Both);
                client.Close();

    }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;

                client.EndConnect(ar);

                Console.WriteLine("Socket connected to {0}",
                    client.RemoteEndPoint.ToString());

                connectDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void Receive(Socket client)
        {
            try
            {
                StateObject state = new StateObject();
                state.workSocket = client;

                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static List<byte[]> tempByteList = new List<byte[]>();
        private static void ReceiveCallback(IAsyncResult ar)
        {
            //FIX HERE
            try
            {
                StateObject state = (StateObject)ar.AsyncState;
                Socket client = state.workSocket;

                int bytesRead = client.EndReceive(ar);
                

                if (bytesRead > 0)
                {
                    byte[] byteToAdd = new byte[bytesRead];
                    for(int i = 0; i < bytesRead; i++)
                    {
                        byteToAdd[i] = state.buffer[i];
                    }
                    tempByteList.Add(byteToAdd);


                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead)); //String is here

                    client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReceiveCallback), state);
                }
                else
                {
                    if (state.sb.Length > 1)
                    {

                        // byte[] buffer = Encoding.ASCII.GetBytes(data.ToString());
                        // byteData = state.buffer;
                        //**byteData = Encoding.ASCII.GetBytes(state.sb.ToString());
                        int totalSize = 0;
                        foreach(byte[]a in tempByteList)
                        {
                            for(int i = 0; i < a.Length; i++)
                            {
                                totalSize++;
                            }
                        }
                        byteData = new byte[totalSize];
                        int currentIndex = 0;
                        foreach (byte[] a in tempByteList)
                        {
                            for (int i = 0; i < a.Length; i++)
                            {
                                byteData[currentIndex] = a[i];
                                currentIndex++;
                            }
                        }


                        response = state.sb.ToString();
                    }
                    receiveDone.Set();
                    tempByteList = new List<byte[]>();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void Send(Socket client, String data)
        {
            byte[] byteDatas = Encoding.ASCII.GetBytes(data);
            client.BeginSend(byteDatas, 0, byteDatas.Length, 0,
                new AsyncCallback(SendCallback), client);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;
                int bytesSent = client.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to server.", bytesSent);
                sendDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
