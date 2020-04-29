using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FlowFix
{
    class Program
    {
        private const int listenPort = 11000;
        private static void StartListener()
        {
            UdpClient listener = new UdpClient(listenPort);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);

            try
            {
                while (true)
                {
                    Console.WriteLine("Waiting for broadcast");
                    byte[] bytes = listener.Receive(ref groupEP);

                    Console.WriteLine($"Received broadcast from {groupEP} :");
                    Console.WriteLine($" {Encoding.ASCII.GetString(bytes, 0, bytes.Length)}");
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                listener.Close();
            }
        }

        static void Main(string[] args)
        {

            if (args[0] == "l")
            {
                StartListener();
            }

            if (args[0] == "s")
            {
                Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                IPAddress broadcast = IPAddress.Parse("192.168.1.9");

                byte[] sendbuf = Encoding.ASCII.GetBytes(args[1]);
                IPEndPoint ep = new IPEndPoint(broadcast, 11000);

                s.SendTo(sendbuf, ep);

                Console.WriteLine("Message sent to the broadcast address");
            }
        }
    }
}
