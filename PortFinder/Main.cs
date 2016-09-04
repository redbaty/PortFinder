using System;
using System.Net;
using System.Net.NetworkInformation;
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

        public event SearchDoneDelegate SearchDone;

        public delegate void SearchDoneDelegate(bool sucess);

        public event PortSearchedDelegate PortSearched;

        public delegate void PortSearchedDelegate(int index, bool opened);

        public PortFinderManager(string hostname, int min, int max)
        {
            Host = hostname;
            Limits = new[] {min, max+1};
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
                for (var index = Limits[0]; index < Limits[1]; index++)
                {
                    PortSearched?.Invoke(index, PingHost(Host, index));
                }

                SearchDone?.Invoke(true);
            }
            catch (Exception)
            {
                SearchDone?.Invoke(false);
            }
        }

        bool PingHost(string host, int port)
        {
            var client = new TcpClient();
            if (!client.ConnectAsync(host, port).Wait(1000))
            {
                return false;
            }
            return true;
        }
    }
}