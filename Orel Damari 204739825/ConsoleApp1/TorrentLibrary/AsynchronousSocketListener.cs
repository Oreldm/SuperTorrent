using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

/**
 * learned from here https://docs.microsoft.com/en-us/dotnet/framework/network-programming/asynchronous-server-socket-example
 * */

namespace TorrentLibrary
{

    public class AsynchronousSocketListener
    {
        private static int firstByte = -1;
        private static int lastByte = -1;
        private static string fileName;
        public static string filePath;
        public static string ipStr;
        public static int port;

        public static ManualResetEvent allDone = new ManualResetEvent(false);

        public AsynchronousSocketListener()
        {
        }

        //Main Method is StartLstening
        public static void StartListening()
        {
            IPAddress ipAddress = IPAddress.Parse(ipStr);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);
            Socket listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    allDone.Reset();
                    Console.WriteLine("Waiting for a connection...");
                    listener.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        listener);
                    allDone.WaitOne();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();

        }

        public static void AcceptCallback(IAsyncResult ar)
        {
            allDone.Set();
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);
            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
        }

        public static void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                state.sb.Append(Encoding.ASCII.GetString(
                    state.buffer, 0, bytesRead));
                content = state.sb.ToString();
                if (content.IndexOf("<EOF>") > -1)
                {
                    Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                        content.Length, content);
                    content = content.Substring(0, content.Length - 5); //Removing <EOF>

                    JObject json = JObject.Parse(content);
                    firstByte = Int32.Parse(json.GetValue("Start").ToString());
                    lastByte = Int32.Parse(json.GetValue("End").ToString());
                    fileName = json.GetValue("Filename").ToString();

                    //HERE TO READ FILE INTO STRING AND SEND IT BACK INSTEAD OF content
                    byte[] byteData = File.ReadAllBytes(filePath+fileName);
                    byte[] partByte = new byte[lastByte-firstByte];
                    for (int i = 0; i < lastByte - firstByte; i++)
                    {
                        partByte[i] = byteData[i + firstByte];
                    }
                    Send(handler, partByte);
                }
                else
                {
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
                }
            }
        }

        private static void Send(Socket handler, byte[] byteData)
        {
            //byte[] byteData = Encoding.ASCII.GetBytes(data);
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket handler = (Socket)ar.AsyncState;
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
