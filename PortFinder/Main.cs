using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

// ReSharper disable InconsistentNaming

namespace PortFinder
{
    public class PortFinderManager
    {
        public Exception Exception { get; set; }
        public TimeSpan ElapsedTime { get; set; }   
        public Range PortsRange { get; set; }
           
        public string Host { get; set; }
        public bool Success { get; set; }
   
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

        /// <summary>
        /// Initializes a new instance of the <see cref="PortFinderManager"/> class.
        /// </summary>
        /// <param name="hostname">The hostname.</param>
        /// <param name="min">The minimum port value.</param>
        /// <param name="max">The maximum port value.</param>
        public PortFinderManager(string hostname, int min, int max)
        {
            Host = hostname;
            PortsRange = new Range
            {
                Min = min,
                Max = max
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PortFinderManager"/> class.
        /// </summary>
        /// <param name="hostname">The hostname.</param>
        /// <param name="range">The ports range.</param>
        public PortFinderManager(string hostname, Range range)
        {
            Host = hostname;
            PortsRange = range;
        }

        /// <summary>
        /// Finds the open ports.
        /// </summary>
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

        /// <summary>
        /// Pings the host.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="port">The port.</param>
        /// <returns></returns>
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