using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patrol
{
    class PortRange : IEnumerable<int>
    {
        public List<int> ports;

        private PortRange(List<int> p)
        {
            ports = p;
        }

        public static PortRange Parse(string input)
        {
            string[] splited = input.Split(',');
            if (splited.Length == 0)
                splited[0] = input;
            List<int> ports = new List<int>();
            foreach(string port in splited)
            {
                if(port.Contains('-'))
                {
                    int[] pairSplited = Array.ConvertAll(port.Split('-'), s => int.Parse(s));
                    for(int i = pairSplited[0]; i <= pairSplited[1]; i++)
                        ports.Add(i);
                }
                else
                    ports.Add(int.Parse(port));
            }

            return new PortRange(ports.Distinct().ToList());
        }

        public IEnumerator<int> GetEnumerator()
        {
            foreach (int port in ports)
                yield return port;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
