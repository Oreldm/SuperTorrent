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
        public static List<string> totalFile=null;

        private static ManualResetEvent connectDone =
            new ManualResetEvent(false);
        private static ManualResetEvent sendDone =
            new ManualResetEvent(false);
        private static ManualResetEvent receiveDone =
            new ManualResetEvent(false);

        private static String response = String.Empty;

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
                receiveDone.WaitOne();
                totalFile.Insert(index, response);
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

        private static void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                StateObject state = (StateObject)ar.AsyncState;
                Socket client = state.workSocket;

                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 0)
                {
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                    client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReceiveCallback), state);
                }
                else
                {
                    if (state.sb.Length > 1)
                    {
                        response = state.sb.ToString();
                    }
                    receiveDone.Set();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void Send(Socket client, String data)
        {
            byte[] byteData = Encoding.ASCII.GetBytes(data);
            client.BeginSend(byteData, 0, byteData.Length, 0,
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
