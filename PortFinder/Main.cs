using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

// ReSharper disable InconsistentNaming

namespace PortFinder
{
    public class PortFinderManager
    {
        public TimeSpan ElapsedTime { get; set; }
        public Range PortsRange { get; set; }
        public string Host { get; set; }

        public bool Success { get; set; }
        public Exception Exception { get; set; }

        public Dictionary<int, bool> ResultsDictionary { get; set; }

        public Dictionary<int, bool> OpenPortsDictionary =>
            ResultsDictionary.Where(keyValuePair => keyValuePair.Value)
                .ToDictionary(keyValuePair => keyValuePair.Key, keyValuePair => keyValuePair.Value);

        public Dictionary<int, bool> ClosedPortsDictionary =>
            ResultsDictionary.Where(keyValuePair => !keyValuePair.Value)
                .ToDictionary(keyValuePair => keyValuePair.Key, keyValuePair => keyValuePair.Value);

        #region Events

        public event SearchDoneDelegate Completed;

        public delegate void SearchDoneDelegate(bool sucess);

        public event PortSearchedDelegate PortSearched;

        public delegate void PortSearchedDelegate(int index, bool opened);

        #endregion

        public PortFinderManager(string hostname, int min, int max)
        {
            Host = hostname;
            PortsRange = new Range
            {
                Min = min,
                Max = max
            };
        }

        public PortFinderManager(string hostname, Range range)
        {
            Host = hostname;
            PortsRange = range;
        }


        public void FindOpenPorts()
        {
            ResultsDictionary = new Dictionary<int, bool>();

            try
            {
                for (var index = PortsRange.Min; index <= PortsRange.Max; index++)
                {
                    var result = PingHost(Host, index);
                    PortSearched?.Invoke(index, result);
                    ResultsDictionary.Add(index, result);
                }
                Success = true;
                Completed?.Invoke(Success);
            }
            catch(Exception ex)
            {
                Success = false;
                Exception = ex;
                Completed?.Invoke(Success);
            }
        }

        private static bool PingHost(string host, int port)
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