using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace ServerSocketApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ExecuteServer();
        }

        public static void ExecuteServer()
        {
            try
            {
                //Create the local endpoint for the socket. Using local port 62000:
                IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddr = ipHost.AddressList[0];
                IPEndPoint local = new IPEndPoint(ipAddr, 62000);

                //Create TCP/IP socket
                Socket listener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                //Use Bind() method to associate a IP address to the server socket:
                listener.Bind(local);

                //Use listen() method to place server in a listening state with (maximum pending connections queue):
                listener.Listen(10);
                Console.WriteLine("[SERVER] The server is running at port 62000");

                Console.WriteLine("[SERVER] Waiting for a client's connection........");

                Socket handler = listener.Accept();
                Console.Write("\n[SERVER] Connection accpeted from a client.");

                byte[] incomingData = new byte[1024];
                string data = null;

                int numByte = handler.Receive(incomingData);
                data += Encoding.ASCII.GetString(incomingData, 0, numByte);

                Console.WriteLine("\n[SERVER] Message of client recieved: {0}", data);

                Console.WriteLine("[SERVER] Hit 'Enter' to send acknowledgement to client and end the session....");
                Console.ReadLine();
				string message = ("[SERVER] Your message was received.");
                byte[] severMessage = Encoding.ASCII.GetBytes(message);

                int sendbytes = handler.Send(severMessage);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }

            catch (SocketException se)
            {
                Console.WriteLine("[SERVER] Socket Error Code: " + se.ErrorCode.ToString());
                Console.WriteLine("       " + se.StackTrace);
            }
        }
    }
}
