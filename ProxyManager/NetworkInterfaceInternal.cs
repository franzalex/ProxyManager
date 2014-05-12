using System;
using System.Linq;
using System.Net.NetworkInformation;

namespace ProxyManager
{
    [Serializable()]
    [Newtonsoft.Json.JsonConverter(typeof(NetworkInterfaceInternal.JsonConverter))]
    internal partial class NetworkInterfaceInternal
    {
        private string _descr;
        private string _id;
        private NetworkInterfaceType _interfaceType;
        private byte[] _mac;
        private string _name;
        private OperationalStatus _status;

        public NetworkInterfaceInternal(System.Net.NetworkInformation.NetworkInterface nic)
        {
            if (nic == null) return;

            _id = nic.Id;
            _name = nic.Name;
            _descr = nic.Description;
            _interfaceType = nic.NetworkInterfaceType;
            _status = nic.OperationalStatus;
            _mac = nic.GetPhysicalAddress().GetAddressBytes();
        }

        public string Description { get { return _descr; } }

        public string Id { get { return _id; } }

        public NetworkInterfaceType InterfaceType { get { return _interfaceType; } }

        public string MacAddress
        {
            get
            {
                // convert mac bytes to hex and separate with a dash ( - )
                return string.Join("-", _mac.Select(b => b.ToString("X2")).ToArray());
            }
        }

        public string Name { get { return _name; } }

        public byte[] PhysicalAddress { get { return _mac; } }

        public OperationalStatus Status
        {
            get { return _status; }
        }

        /// <summary>Gets the network interfaces that can transmit data.</summary>
        public static System.Collections.Generic.IEnumerable<NetworkInterface> GetTransmittingInterfaces()
        {
            var skips = new[] { NetworkInterfaceType.Loopback, NetworkInterfaceType.Tunnel };
            var ti = new System.Collections.Generic.List<NetworkInterface>();

            // TODO: Invvestigate if wrapping GetTransmittingInterfaces still throws an uncaught exception.
            try
            {
                foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
                    if (!skips.Contains(nic.NetworkInterfaceType) &&
                        nic.OperationalStatus == OperationalStatus.Up)
                        ti.Add(nic);
            }
            catch
            { /* Do nothing. For now... */
            }

            return ti;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" }, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj.GetType() == typeof(NetworkInterfaceInternal) &&
                   ((NetworkInterfaceInternal)obj).With(n => n.Id == Id);
        }

        /// <summary>Returns a hash code for this instance.</summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        /// <summary>Returns a <see cref="System.String" /> that represents this instance.</summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return Description;
        }

        #region Property Names

        private static class PropertyNames
        {
            internal static readonly string Description;
            internal static readonly string Id;
            internal static readonly string InterfaceType;
            internal static readonly string MacAddress;
            internal static readonly string Name;
            internal static readonly string PhysicalAddress;

            [System.Diagnostics.DebuggerStepThrough()]
            static PropertyNames()
            {
                var n = new NetworkInterfaceInternal(null);
                Id = GetPropertyName(() => n.Id);
                InterfaceType = GetPropertyName(() => n.InterfaceType);
                MacAddress = GetPropertyName(() => n.MacAddress);
                Name = GetPropertyName(() => n.Name);
                PhysicalAddress = GetPropertyName(() => n.PhysicalAddress);
                Description = GetPropertyName(() => n.Description);
            }

            [System.Diagnostics.DebuggerStepThrough()]
            internal static string[] GetAllNames()
            {
                return new[] { Description, Id, InterfaceType, MacAddress, Name, PhysicalAddress };
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