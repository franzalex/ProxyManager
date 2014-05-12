using System;
using System.ComponentModel;

namespace ProxyManager
{
    [Serializable()]
    public class ProxyAddress
    {
        private static System.Text.RegularExpressions.Regex ValidHostname;
        private static System.Text.RegularExpressions.Regex ValidIpAddress;
        public static readonly ProxyAddress NoProxy;
        private string _address;

        static ProxyAddress()
        {
            ValidIpAddress = new System.Text.RegularExpressions.Regex(@"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$");
            ValidHostname = new System.Text.RegularExpressions.Regex(@"^(([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*([A-Za-z0-9]|[A-Za-z0-9][A-Za-z0-9\-]*[A-Za-z0-9])$");
            NoProxy = new ProxyAddress("", 80);
        }

        /// <summary>Initializes a new instance of the <see cref="ProxyAddress"/> class.</summary>
        public ProxyAddress()
            : this(ProxyAddress.Default.Address, ProxyAddress.Default.Port)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ProxyAddress"/> class.</summary>
        /// <param name="address">The proxy address.</param>
        /// <param name="port">The port.</param>
        public ProxyAddress(string address, UInt16 port)
            : this(address.Trim() + ":" + port.ToString()) { }

        /// <summary>Initializes a new instance of the <see cref="ProxyAddress"/> class.</summary>
        /// <param name="proxyAddress">The proxy address.</param>
        public ProxyAddress(string proxyAddress)
        {
            var p = proxyAddress.Split(':');
            _address = p[0].Trim();

            if (!IsValidAddress(_address))
                throw new ArgumentException("'" + _address + "' not a valid IP address or host name.");

            this.Port = Convert.ToUInt16(p.Length >= 2 && !string.IsNullOrEmpty(p[1]) ? p[1] : "0");
        }

        public static ProxyAddress Default
        {
            get { return new ProxyAddress("", 0); }
        }

        /// <summary>Gets or sets the address.</summary>
        /// <value>The address.</value>
        [DefaultValue("")]
        public string Address
        {
            get { return _address; }
            set
            {
                value = value.Trim();
                if (IsValidAddress(value))
                    _address = value;
                else
                    throw new ArgumentException("The specified value is not a valid IP address or host name.");
            }
        }

        /// <summary>Gets a value indicating whether an address is set.</summary>
        /// <value><c>true</c> if an address has been set; otherwise, <c>false</c>.</value>
        [DefaultValue(true)]
        public bool HasAddress { get { return !string.IsNullOrEmpty(_address); } }

        /// <summary>Gets or sets the port.</summary>
        /// <value>The port.</value>
        public UInt16 Port { get; set; }

        public static implicit operator ProxyAddress(string proxyAddress)
        {
            return new ProxyAddress(proxyAddress);
        }

        public static implicit operator string(ProxyAddress pa)
        {
            return pa.ToString();
        }

        /// <summary>Determines whether the specified string is a valid address.</summary>
        /// <param name="address">The address to validate.</param>
        /// <returns><c>true</c> if the address is valid; else <c>false</c>.</returns>
        public static bool IsValidAddress(string address)
        {
            address = address.Trim();
            return (string.IsNullOrEmpty(address) || ValidIpAddress.IsMatch(address) || ValidHostname.IsMatch(address));
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
            return obj != null && obj.GetType() == this.GetType() && obj.ToString() == this.ToString();
        }

        /// <summary>Returns a hash code for this instance.</summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        /// <summary>Returns a <see cref="System.String" /> that represents this instance.</summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Address))
                return Address + ":" + Port.ToString();
            else
                return "";
        }
    }
}