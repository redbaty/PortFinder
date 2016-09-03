using System;
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
        }

        public void Run()
        {
            Thread t = new Thread(FindOpenPorts);
            t.Start();
        }

        void FindOpenPorts()
        {
            try
            {
                Parallel.For(Limits[0], Limits[1]+1, (index, loopState) =>
                {
                    PortSearched?.Invoke(index);
                    if (!PingHost(Host, index)) return;
                    PortFound?.Invoke(index);
                });

                PortDone?.Invoke(true);
            }
            catch (Exception)
            {
                PortDone?.Invoke(false);
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
