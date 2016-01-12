using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable InconsistentNaming

namespace PortFinder
{
    public class PortFinderManager
    {
        public TimeSpan ElapsedTime;

        public int[] Limits;
        public string Host;

        public event PortFoundDelegate PortFound;
        public delegate void PortFoundDelegate(int index);
        
        public event PortDoneDelegate PortDone;
        public delegate void PortDoneDelegate(bool sucess);
        
        public event PortSearchedDelegate PortSearched;
        public delegate void PortSearchedDelegate(int index);

        public PortFinderManager(string hostname, int min, int max)
        {
            Host = hostname;
            Limits = new[] { min, max };
            PortFound += AntiCrash;
            PortDone += AntiCrash;
            PortSearched += AntiCrash;
        }

        private void AntiCrash(int index) { }
        private void AntiCrash(bool sucess) { }

        public void Run()
        {
            Thread t = new Thread(FindOpenPorts);
            t.Start();
        }

        void FindOpenPorts()
        {
            try
            {
                Stopwatch watcher = new Stopwatch();
                watcher.Restart();
                Parallel.For(Limits[0], Limits[1], (index, loopState) =>
                {
                    if (PortSearched != null) PortSearched(index);
                    if (!PingHost(Host, index)) return;
                    if (PortFound != null) PortFound(index);
                });
                watcher.Stop();
                ElapsedTime = watcher.Elapsed;
                if (PortDone != null) PortDone(true);
            }
            catch (Exception)
            {
                if (PortDone != null) PortDone(false);
            }
        }

        bool PingHost(string host, int port)
        {
            try
            {
                TcpClient client = new TcpClient(host, port);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }


}
