using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FlowFix
{
    class Program
    {
        private static void StartListener(int listenPort)
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
                StartListener(Convert.ToInt32(args[1]));
            }

            if (args[0] == "s")
            {
                Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                var targetIp = args[1];
                var targetPort = args[2];

                IPAddress broadcast = IPAddress.Parse(targetIp);

                byte[] sendbuf = Encoding.ASCII.GetBytes(args[3]);
                IPEndPoint ep = new IPEndPoint(broadcast, Convert.ToInt32(targetPort));
                
                s.SendTo(sendbuf, ep);

                Console.WriteLine($"Message sent to the broadcast address {targetIp}:{targetPort}");
            }
        }
    }
}
