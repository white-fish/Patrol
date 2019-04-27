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
            DateTime start = DateTime.Now;
            Console.WriteLine("Starting...");
            Parallel.ForEach(IpRange, new ParallelOptions { MaxDegreeOfParallelism = 50 }, (ip) =>
            {
                foreach (int port in PortRange)
                {
                    Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
                    var result = socket.BeginConnect(ip, port, null, null);
                    bool success = result.AsyncWaitHandle.WaitOne(timeOut);
                    if (success)
                    {
                        Console.WriteLine($"Found port open {port} @ {ip.MapToIPv4().ToString()}");
                    }
                    else
                    {
                        socket.Close();
                    }
                }
            });
            DateTime end = DateTime.Now;
            Console.WriteLine($"Done in {end - start}");
        }
    }
}
