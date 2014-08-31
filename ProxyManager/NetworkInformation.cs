using System;
using System.Collections.Generic;
using System.Linq;
using AddressFamily = System.Net.Sockets.AddressFamily;
using IPAddress = System.Net.IPAddress;
using IPInterfaceProperties = System.Net.NetworkInformation.IPInterfaceProperties;
using NetInfo = System.Net.NetworkInformation;
using NetworkInterface = System.Net.NetworkInformation.NetworkInterface;

namespace ProxyManager
{
    [Serializable()]
    [Newtonsoft.Json.JsonConverter(typeof(NetworkInformation.JsonConverter))]
    internal partial class NetworkInformation
    {
        private bool _autoAssignedAddress;
        private string _dnsSuffix;
        private bool _initializedSuccessfully;
        private NetworkInterfaceInternal nic;
        private Dictionary<string, IPAddress[]> ntwkInfo;


        public NetworkInformation(NetworkInterface ntwkInterface)
        {
            try
            {
                ntwkInfo = new Dictionary<string, IPAddress[]>();

                if (ntwkInterface == null) return;
                this.nic = new NetworkInterfaceInternal(ntwkInterface);
                var ipProps = ntwkInterface.GetIPProperties();

                foreach (var name in PropertyNames.GetIPAddressPropertyNames())
                    ntwkInfo[name] = GetAddress(name, ipProps);

                foreach (var addr in ipProps.UnicastAddresses)
                {
                    var pre = addr.PrefixOrigin != NetInfo.PrefixOrigin.Manual;
                    var suf = addr.SuffixOrigin != NetInfo.SuffixOrigin.Manual;

                    _autoAssignedAddress = pre && suf;
                    if (_autoAssignedAddress) break;
                }

                _dnsSuffix = ipProps.DnsSuffix;
                _initializedSuccessfully = true;
            }
            catch
            {
                _initializedSuccessfully = false;
            }
        }

        public bool AutoAssignedIPAddress { get { return _autoAssignedAddress; } }

        /// <summary>Gets the default gateway address.</summary>
        public IPAddress[] DefaultGateway { get { return ntwkInfo[PropertyNames.DefaultGateway]; } }

        /// <summary>Gets the DHCP server.</summary>
        public IPAddress[] DhcpServer { get { return ntwkInfo[PropertyNames.DhcpServer]; } }

        /// <summary>Gets the DNS server.</summary>
        public IPAddress[] DnsServer { get { return ntwkInfo[PropertyNames.DnsServer]; } }

        /// <summary>Gets the DNS suffix.</summary>
        public string DnsSuffix
        {
            get { return _dnsSuffix; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance was initialized successfully.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance was initialized successfully; otherwise, <c>false</c>.
        /// </value>
        public bool InitializedSuccessfully
        {
            get { return _initializedSuccessfully; }
        }

        /// <summary>Gets the IPv4 address.</summary>
        public IPAddress IPv4Address { get { return ntwkInfo[PropertyNames.IPv4Address][0]; } }

        /// <summary>Gets the IPv4 subnet mask.</summary>
        public IPAddress IPv4SubnetMask { get { return ntwkInfo[PropertyNames.IPv4Subnet][0]; } }

        /// <summary>Gets the network interface that is connected to the network.</summary>
        public NetworkInterfaceInternal NetworkInterface { get { return nic; } }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" }, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(NetworkInformation)) return false;

            var n = ((NetworkInformation)obj);
            var result = n.NetworkInterface.Equals(NetworkInterface);

            foreach (var key in PropertyNames.GetIPAddressPropertyNames())
            {
                if (result == false) break;

                var nValues = string.Join(" | ", n.ntwkInfo[key].Select(i => i.ToString()).ToArray());
                var thisValues = string.Join(" | ", n.ntwkInfo[key].Select(i => i.ToString()).ToArray());
                result &= nValues.Equals(thisValues);
            }


            return result;
        }

        /// <summary>Returns a hash code for this instance.</summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return NetworkInterface.GetHashCode() ^ (IPv4Address.ToString().GetHashCode());
        }

        /// <summary>Returns a <see cref="System.String" /> that represents this instance.</summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            var l = new List<string>();
            l.Add(NetworkInterface.Name);
            if (!NetworkInterface.MacAddress.IsNullOrEmpty())
                l.Add("MAC: " + NetworkInterface.MacAddress);

            l.Add("IP: " + IPv4Address.ToString());
            if (DnsServer.Any())
                l.Add("DNS: " + DnsServer[0]);

            return string.Join("; ", l.ToArray());
        }

        private static IPAddress[] GetAddress(string name, IPInterfaceProperties ipProperties)
        {
            if (name == PropertyNames.DefaultGateway)
                return GetIPv4Address(ipProperties.GatewayAddresses.Select(a => a.Address));
            else if (name == PropertyNames.DhcpServer)
                return GetIPv4Address(ipProperties.DhcpServerAddresses.Select(a => a));
            else if (name == PropertyNames.DnsServer)
                return GetIPv4Address(ipProperties.DnsAddresses.Select(a => a));
            else if (name == PropertyNames.IPv4Address)
                return GetIPv4Address(ipProperties.UnicastAddresses.Select(a => a.Address));
            else if (name == PropertyNames.IPv4Subnet)
                return GetIPv4Address(ipProperties.UnicastAddresses.Select(a => a.IPv4Mask));
            else
                return new[] { IPAddress.None };
        }

        private static IPAddress[] GetIPv4Address(IEnumerable<IPAddress> collection)
        {
            var IPv4 = collection.Where(a => a != null && a != IPAddress.None &&
                                             a.AddressFamily == AddressFamily.InterNetwork);

            if (IPv4.Any())
                return IPv4.ToArray();
            else if (collection.Any())
                // no IPv4 addresses found. return non-null addresses
                return collection.Where(a => a != null).ToArray();
            else
                // no elements in collection
                return new[] { IPAddress.None };
        }

        #region Property Names

        private static class PropertyNames
        {
            internal static readonly string AutoAssignedIP;
            internal static readonly string DefaultGateway;
            internal static readonly string DhcpServer;
            internal static readonly string DnsServer;
            internal static readonly string DnsSuffix;
            internal static readonly string IPv4Address;
            internal static readonly string IPv4Subnet;
            internal static readonly string NetworkInterface;

            [System.Diagnostics.DebuggerStepThrough()]
            static PropertyNames()
            {
                var n = new NetworkInformation(null);
                AutoAssignedIP = GetPropertyName(() => n.AutoAssignedIPAddress);
                DefaultGateway = GetPropertyName(() => n.DefaultGateway);
                DhcpServer = GetPropertyName(() => n.DhcpServer);
                DnsServer = GetPropertyName(() => n.DnsServer);
                DnsSuffix = GetPropertyName(() => n.DnsSuffix);
                IPv4Address = GetPropertyName(() => n.IPv4Address);
                IPv4Subnet = GetPropertyName(() => n.IPv4SubnetMask);
                NetworkInterface = GetPropertyName(() => n.NetworkInterface);
            }

            [System.Diagnostics.DebuggerStepThrough()]
            internal static string[] GetAllNames()
            {
                return new[] { AutoAssignedIP, DefaultGateway, DhcpServer, DnsServer,
                               DnsSuffix, IPv4Address, IPv4Subnet, NetworkInterface };
            }

            [System.Diagnostics.DebuggerStepThrough()]
            internal static string[] GetIPAddressPropertyNames()
            {
                return new[] { IPv4Address, IPv4Subnet, DefaultGateway, DhcpServer, DnsServer };
            }

            [System.Diagnostics.DebuggerStepThrough()]
            private static string GetPropertyName<T>(System.Linq.Expressions.Expression<Func<T>> exp)
            {
                return (((System.Linq.Expressions.MemberExpression)(exp.Body)).Member).Name;
            }
        }

        #endregion Property Names
    }
}