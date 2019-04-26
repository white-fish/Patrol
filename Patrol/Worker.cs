using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace Patrol
{
    class Worker
    {
        public static void Work(IpRange IpRange, PortRange PortRange, int timeOut)
        {

            Console.WriteLine("Starting...");
            foreach(IPAddress ip in IpRange)
            {
                foreach (int port in PortRange)
                {
                    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket.SendTimeout = timeOut;
                    socket.ReceiveTimeout = timeOut;
                    socket.SendTimeout = timeOut;
                    var result = socket.BeginConnect(ip, port, null, null);
                    bool success = result.AsyncWaitHandle.WaitOne(timeOut);
                    if (success)
                    {
                        try
                        {
                            socket.EndConnect(result);
                        }
                        catch(Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        Console.WriteLine($"Found port open {port} @ {ip.MapToIPv4().ToString()}");
                    }
                    else
                    {
                        socket.Close();
                    }
                }
            }
            Console.WriteLine("Done");
        }
    }
}
