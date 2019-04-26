using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;
using System.Collections;
using System.Reflection;

namespace Patrol
{
    /**
     * All credits 2 szymski aka swimek aka szymon aka ten z telewizji 4 this class
     */
    class IpRange : IEnumerable<IPAddress>
    {
        private int _lower { get; set; }
        private int _upper { get; set; }

        private IpRange(int lower, int upper)
        {
            _lower = lower;
            _upper = upper;
        }

        public IEnumerator<IPAddress> GetEnumerator()
        {
            for (int ip = _lower; ip <= _upper; ip++)
            {
                yield return (IPAddress)typeof(IPAddress)
                    .GetConstructor(
                        BindingFlags.NonPublic | BindingFlags.Instance,
                        null,
                        new[] { typeof(int) },
                        null)
                    .Invoke(new object[] { IPAddress.HostToNetworkOrder(ip) });
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static IpRange Parse(string range)
        {
            Match m;

            // IP and mask form
            if ((m = Regex.Match(range, @"([0-9]{1,3})\.([0-9]{1,3})\.([0-9]{1,3})\.([0-9]{1,3})[\\/]([0-9]{1,2})")).Success)
            {
                var ip = ReverseEndian(IpFromOctets(
                    byte.Parse(m.Groups[1].Value),
                    byte.Parse(m.Groups[2].Value),
                    byte.Parse(m.Groups[3].Value),
                    byte.Parse(m.Groups[4].Value)));

                var mask = byte.Parse(m.Groups[5].Value);

                int upperAddr = (int)(ip | (uint.MaxValue >> mask));

                return new IpRange(ip, upperAddr);
            }

            // IP to IP form
            else if ((m = Regex.Match(range, @"([0-9]{1,3})\.([0-9]{1,3})\.([0-9]{1,3})\.([0-9]{1,3})\s*-\s*([0-9]{1,3})\.([0-9]{1,3})\.([0-9]{1,3})\.([0-9]{1,3})")).Success)
            {
                var lowerAddr = IpFromOctets(
                    byte.Parse(m.Groups[1].Value),
                    byte.Parse(m.Groups[2].Value),
                    byte.Parse(m.Groups[3].Value),
                    byte.Parse(m.Groups[4].Value));

                var upperAddr = IpFromOctets(
                    byte.Parse(m.Groups[5].Value),
                    byte.Parse(m.Groups[6].Value),
                    byte.Parse(m.Groups[7].Value),
                    byte.Parse(m.Groups[8].Value));

                return new IpRange(ReverseEndian(lowerAddr), ReverseEndian(upperAddr));
            }

            throw new ArgumentException($"Invalid IP range: {range}");
        }

        private static int ReverseEndian(int value)
        {
            var b1 = (value >> 0) & 0xff;
            var b2 = (value >> 8) & 0xff;
            var b3 = (value >> 16) & 0xff;
            var b4 = (value >> 24) & 0xff;

            return b1 << 24 | b2 << 16 | b3 << 8 | b4 << 0;
        }

        private static int IpFromOctets(byte o1, byte o2, byte o3, byte o4)
        {
            return o1 | (o2 << 8) | (o3 << 16) | (o4 << 24);
        }
    }
}
