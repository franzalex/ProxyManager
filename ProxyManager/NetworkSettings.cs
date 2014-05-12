using System;
using System.Linq;
using System.Collections.Generic;

namespace ProxyManager
{
    [Serializable()]
    internal class NetworkSettings
    {
        private List<NetworkInformation> _networks;

        public NetworkSettings()
        {
            _networks = new List<NetworkInformation>();
        }

        public NetworkSettings(IEnumerable<NetworkInformation> netwkInfo, ConnectionSettings configuration)
            : this()
        {
            this.Networks.AddRange(netwkInfo);
            this.Configuration = configuration;
        }

        public ConnectionSettings Configuration { get; set; }

        public List<NetworkInformation> Networks { get { return _networks; } }

        internal double NetworkMatchScore(IEnumerable<NetworkInformation> matchNetworks)
        {
            var scores = new List<double>();

            foreach (var ni1 in Networks)
            {
                var matchScore = 0.0;
                foreach (var ni2 in matchNetworks)
                {
                    double score = 0;

                    score += ni2.DnsSuffix == ni2.DnsSuffix ? 1 : 0;
                    score += ni2.DefaultGateway.Intersect(ni1.DefaultGateway).With(x => x.Count() / ni1.DefaultGateway.Length);
                    score += ni2.DhcpServer.Intersect(ni1.DhcpServer).With(i => i.Count() / ni1.DhcpServer.Length);
                    score += ni2.DnsServer.Intersect(ni1.DnsServer).With(c => c.Count() / ni1.DnsServer.Length);

                    // LOW: May need to review NetworkMatchScore algorithm
                    //if (ni2.NetworkInterface.InterfaceType == ni1.NetworkInterface.InterfaceType)
                    if (score >= 2)
                    {
                        foreach (var param in new[]{ (ni2.AutoAssignedIPAddress && ni1.AutoAssignedIPAddress) ||
                                                     (ni2.IPv4Address.Equals(ni1.IPv4Address)),
                                                     ni2.NetworkInterface.InterfaceType == ni1.NetworkInterface.InterfaceType,
                                                     ni2.NetworkInterface.MacAddress == ni1.NetworkInterface.MacAddress,
                                                     ni2.NetworkInterface.Id == ni1.NetworkInterface.Id
                        })
                        {
                            score += param ? 1 : 0;
                        }
                    }

                    // normalize the score
                    score /= 8;

                    matchScore = Math.Max(matchScore, score);
                }
                scores.Add(matchScore);
            }

            // if networks in this instance and networks to match are both empty return 100% else return score
            return Networks.Count == 0 && matchNetworks.Any() == false ? 1 : scores.Sum() / scores.Count;
        }
    }
}