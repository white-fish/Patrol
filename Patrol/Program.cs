using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using System.Net;

namespace Patrol
{
    class Program
    {
        public static IpRange IpRange;
        public static PortRange PortRange;
        public class Options
        {
            [Option('p', "ports", Default = "1", HelpText = "Port list to scan")]
            public string Ports { get; set; }

            [Option('r', "range", Default = "192.168.0.1/24", HelpText = "IP range to scan")]
            public string IpRange { get; set; }

            [Option('t', "timeout", Default = 500, HelpText = "Timeout in mili sec")]
            public int timeOut { get; set; }
        }

        private static void Run(Options options)
        {
            //Checking input errors
            try
            {
                IpRange = IpRange.Parse(options.IpRange);
                PortRange = PortRange.Parse(options.Ports);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Worker.Work(IpRange, PortRange, options.timeOut);
        }

        static void Main(string[] args)
        {
            Console.WriteLine(@" ______  ______  ______  ______  ______  __        
/\  == \/\  __ \/\__  _\/\  == \/\  __ \/\ \       
\ \  _-/\ \  __ \/_/\ \/\ \  __<\ \ \/\ \ \ \____  
 \ \_\   \ \_\ \_\ \ \_\ \ \_\ \_\ \_____\ \_____\ 
  \/_/    \/_/\/_/  \/_/  \/_/ /_/\/_____/\/_____/ 
        Port Scanner                                                      
");
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(o => { Run(o); });
            Console.ReadKey();
        }
    }
}