namespace BotwTrainer
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Net.NetworkInformation;
    using System.Text;
    using System.Threading;

    public class IpAddress
    {
        private const int TimeOut = 250;

        private const int Ttl = 5;

        private static readonly List<Ping> Pingers = new List<Ping>();

        private static readonly object Lock = new object();

        private static int instances;

        private readonly string baseIp;

        public readonly List<string> FoundList; 

        public IpAddress()
        {
            FoundList = new List<string>();
            baseIp = "192.168.1.";

            CreatePingers(255);

            var po = new PingOptions(Ttl, true);
            var enc = new ASCIIEncoding();
            var data = enc.GetBytes("abababababababababababababababab");

            var cnt = 1;

            foreach (var ping in Pingers)
            {
                lock (Lock)
                {
                    instances += 1;
                }

                ping.SendAsync(string.Concat(baseIp, cnt.ToString(CultureInfo.InvariantCulture)), TimeOut, data, po);
                cnt += 1;
            }

            DestroyPingers();
        }

        private void PingCompleted(object s, PingCompletedEventArgs e)
        {
            lock (Lock)
            {
                instances -= 1;
            }

            if (e.Reply.Status == IPStatus.Success)
            {
                FoundList.Add(e.Reply.Address.ToString());
            }
        }

        private void CreatePingers(int cnt)
        {
            for (var i = 1; i <= cnt; i++)
            {
                var p = new Ping();
                p.PingCompleted += PingCompleted;
                Pingers.Add(p);
            }
        }

        private void DestroyPingers()
        {
            foreach (var p in Pingers)
            {
                p.PingCompleted -= PingCompleted;
                p.Dispose();
            }

            Pingers.Clear();
        }
    }
}
