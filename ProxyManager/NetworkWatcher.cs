using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

namespace ProxyManager
{
    #region Network Event Args

    /// <summary>Provides data for the <seealso cref="NetworkAddressChanged"/> event.</summary>
    internal class NetworkAddressChangedEventArgs : EventArgs
    {
        private NetworkConnectionInfo _ni;

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkAddressChangedEventArgs" /> class.
        /// </summary>
        /// <param name="networkInfo">The network information.</param>
        public NetworkAddressChangedEventArgs(NetworkConnectionInfo networkInfo)
        {
            _ni = networkInfo;
        }

        /// <summary>Gets the network information.</summary>
        public NetworkConnectionInfo NetworkInfo { get { return _ni; } }
    }

    /// <summary>Provides data for the <seealso cref="NetworkConnect"/> event.</summary>
    internal class NetworkConnectEventArgs : EventArgs
    {
        private NetworkInterface nic;

        /// <summary>Initializes a new instance of the <see cref="NetworkConnectEventArgs" /> class.</summary>
        /// <param name="nic">The network interface triggering the <seealso cref="NetworkConnect" /> event.</param>
        public NetworkConnectEventArgs(NetworkInterface nic)
        {
            this.nic = nic;
        }

        /// <summary>
        /// Gets the network interface raising the <see cref="NetworkConnect" /> event.
        /// </summary>
        public NetworkInterface NetworkInterface { get { return nic; } }
    }

    /// <summary>Provides data for the <seealso cref="NetworkDisconnect"/> event.</summary>
    internal class NetworkDisconnectEventArgs : EventArgs
    {
        private NetworkInterface nic;

        /// <summary>Initializes a new instance of the <see cref="NetworkDisconnectEventArgs" /> class.</summary>
        /// <param name="nic">The network interface triggering the <seealso cref="NetworkDisconnect" /> event.</param>
        public NetworkDisconnectEventArgs(NetworkInterface nic)
        {
            this.nic = nic;
        }

        /// <summary>
        /// Gets the network interface raising the <see cref="NetworkDisconnect" /> event.
        /// </summary>
        public NetworkInterface NetworkInterface { get { return nic; } }
    }

    #endregion Network Event Args

    internal class NetworkWatcher
    {
        private bool _isRunning;
        private Dictionary<string, System.Net.IPAddress> nicAddress;
        private Dictionary<string, OperationalStatus> nicState;

        /// <summary>Initializes a new instance of the <see cref="NetworkWatcher"/> class.</summary>
        public NetworkWatcher()
        {
            nicState = new Dictionary<string, OperationalStatus>();
            nicAddress = new Dictionary<string, System.Net.IPAddress>();
        }

        /// <summary>Occurs when the IP Address of a network interface changes.</summary>
        public event EventHandler<NetworkAddressChangedEventArgs> NetworkAddressChanged;

        /// <summary>Occurs when a network interface connects to a network.</summary>
        public event EventHandler<NetworkConnectEventArgs> NetworkConnect;

        /// <summary>Occurs when a network interface disconnects from a network.</summary>
        public event EventHandler<NetworkDisconnectEventArgs> NetworkDisconnect;

        public bool IsRunning
        {
            get { return _isRunning; }
        }

        /// <summary>Starts monitoring all connected network interfaces for changes.</summary>
        public void Start()
        {
            foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                nicState[nic.Id] = nic.OperationalStatus;
                if (nic.OperationalStatus == OperationalStatus.Up)
                    nicAddress[nic.Id] = (new NetworkConnectionInfo(nic)).IPv4Address;
                else
                    nicAddress[nic.Id] = System.Net.IPAddress.None;
            }

            NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAddressChanged;
            _isRunning = true;
        }

        /// <summary>Stops monitoring network interfaces for changes.</summary>
        public void Stop()
        {
            NetworkChange.NetworkAddressChanged -= NetworkChange_NetworkAddressChanged;
            nicAddress.Clear();
            nicState.Clear();
            _isRunning = false;
        }

        private void NetworkChange_NetworkAddressChanged(object sender, EventArgs e)
        {
            var loopbacks = new[] { NetworkInterfaceType.Loopback, NetworkInterfaceType.Tunnel };

            // look through all interfaces for the one whose address has changed
            foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                // skip loopback & tunnel interfaces
                if (loopbacks.Contains(nic.NetworkInterfaceType))
                    continue;

                var ntwkInfo = new NetworkConnectionInfo(nic);

                if (nicState[nic.Id] == OperationalStatus.Up &&
                    nic.OperationalStatus == OperationalStatus.Down) // disconnect = Up -> Down
                {
                    OnNetworkDisconnect(new NetworkDisconnectEventArgs(nic));
                    break;
                }
                else if (nicState[nic.Id] != OperationalStatus.Up &&
                         nic.OperationalStatus == OperationalStatus.Up) // connect = any state -> Up
                {
                    OnNetworkConnect(new NetworkConnectEventArgs(nic));
                    break;
                }
                else if (!nicAddress[nic.Id].Equals(ntwkInfo.IPv4Address)) // changed address
                {
                    OnNetworkAddressChanged(new NetworkAddressChangedEventArgs(ntwkInfo));
                    break;
                }
            }
        }

        /// <summary>Raises the <see cref="E:NetworkAddressChanged" /> event.</summary>
        /// <param name="e">The <see cref="NetworkAddressChangedEventArgs"/> instance containing the event data.</param>
        private void OnNetworkAddressChanged(NetworkAddressChangedEventArgs e)
        {
            var h = NetworkAddressChanged;
            if (h != null)
                h(this, e);
        }

        /// <summary>Raises the <see cref="E:NetworkConnect" /> event.</summary>
        /// <param name="e">The <see cref="NetworkConnectEventArgs"/> instance containing the event data.</param>
        private void OnNetworkConnect(NetworkConnectEventArgs e)
        {
            var h = NetworkConnect;
            if (h != null)
                NetworkConnect(this, e);
        }

        /// <summary>Raises the <see cref="E:NetworkDisconnect" /> event.</summary>
        /// <param name="e">The <see cref="NetworkDisconnectEventArgs"/> instance containing the event data.</param>
        private void OnNetworkDisconnect(NetworkDisconnectEventArgs e)
        {
            var h = NetworkDisconnect;
            if (h != null)
                NetworkDisconnect(this, e);
        }
    }
}