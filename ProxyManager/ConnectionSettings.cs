using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace ProxyManager
{
    [Flags()]
    public enum ConnectionType
    {
        NoProxy,
        ManualProxy = 1 << 1,
        AutoConfigUrl = 1 << 2,
        AutoDetect = 1 << 3,
    }

    [Serializable()]
    public class ConnectionSettings
    {
        #region P/Invoke

        private const int INTERNET_OPTION_REFRESH = 37;
        private const int INTERNET_OPTION_SETTINGS_CHANGED = 39;

        [System.Runtime.InteropServices.DllImport("wininet.dll")]
        private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);

        #endregion P/Invoke

        private const string keyName = @"Software\Microsoft\Windows\CurrentVersion\Internet Settings"; // @HKCU\
        private ProxySettings _proxies;

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="ConnectionSettings"/> class.</summary>
        public ConnectionSettings()
            : this(ConnectionType.NoProxy) { }

        /// <summary>Initializes a new instance of the <see cref="ConnectionSettings"/> class.</summary>
        /// <param name="autoConfigUrl">The automatic configuration URL.</param>
        public ConnectionSettings(string autoConfigUrl)
            : this(ConnectionType.AutoConfigUrl)
        {
            this.AutoConfigUrl = autoConfigUrl;
        }

        /// <summary>Initializes a new instance of the <see cref="ConnectionSettings"/> class.</summary>
        /// <param name="address">The proxy address to use for all protocols.</param>
        public ConnectionSettings(ProxyAddress address)
            : this(address, address, address, address) { }

        /// <summary>Initializes a new instance of the <see cref="ConnectionSettings"/> class.</summary>
        /// <param name="httpProxy">The HTTP proxy.</param>
        /// <param name="httpsProxy">The HTTPS proxy.</param>
        /// <param name="ftpProxy">The FTP proxy.</param>
        /// <param name="socksProxy">The socks proxy.</param>
        public ConnectionSettings(ProxyAddress httpProxy, ProxyAddress httpsProxy,
                                  ProxyAddress ftpProxy, ProxyAddress socksProxy)
            : this(ConnectionType.ManualProxy)
        {
            this.ConnectionType = ConnectionType.ManualProxy;
            _proxies = new ProxySettings(httpProxy, httpsProxy, ftpProxy, socksProxy);
        }

        /// <summary>Initializes a new instance of the <see cref="ConnectionSettings"/> class.</summary>
        /// <param name="proxyMode">The proxy mode.</param>
        public ConnectionSettings(ConnectionType proxyMode)
        {
            this.ConnectionType = proxyMode;
            this.AutoConfigUrl = "";
            _proxies = new ProxySettings();
        }

        #endregion Constructors

        // properties

        /// <summary>Gets or sets the url to the script for automatic configuration.</summary>
        /// <value>The Auto Configuration URL address.</value>
        [DefaultValue("")]
        public string AutoConfigUrl { get; set; }

        /// <summary>Gets or sets the type of connection.</summary>
        /// <value>The type.</value>
        public ConnectionType ConnectionType
        {
            get;
            set;
        }

        /// <summary>Gets the manual proxy configuration.</summary>
        [DefaultValue(null)]
        public ProxySettings ManualProxy
        {
            get { return _proxies; }
        }

        // Methods

        /// <summary>Gets the current configuration.</summary>
        /// <returns>
        /// A <see cref="ConnectionSettings" /> instance representing the current network configuration.
        /// </returns>
        public static ConnectionSettings GetCurrentConfig()
        {
            var sep = new[] { ';' };
            var rmvEmpty = StringSplitOptions.RemoveEmptyEntries;
            var nc = new ConnectionSettings();
            var reg = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(keyName);

            // connection type checkboxes
            var checks = reg.OpenSubKey("Connections")
                            .GetValue("DefaultConnectionSettings", new byte[10])
                            .As<byte[]>()[8];

            foreach (var flag in ((ConnectionType)checks).GetAllFlagsPresent())
                nc.ConnectionType |= flag;

            //
            // This check may not be needed since the value is likely to be set from
            // DefaultConnectionSettings. It is however kept here as a fail-safe measure
            // since Microsoft decided to set the value at two different locations.
            //
            if (reg.GetValue("ProxyEnable", 0).As<int>() == 1)
                nc.ConnectionType |= ConnectionType.ManualProxy;

            // autoconfig url
            nc.AutoConfigUrl = (string)reg.GetValue("AutoConfigUrl", "");

            // proxy overrides
            var overrides = reg.GetValue("ProxyOverride", "").As<string>().Split(sep, rmvEmpty);

            // proxy servers
            ProxyAddress http = null, https = null, ftp = null, socks = null;
            var proxies = reg.GetValue("ProxyServer", "").As<string>().ToLower().Split(sep, rmvEmpty);
            if (proxies.Length == 1)
                http = new ProxyAddress(proxies[0]);
            else
                foreach (var proxy in proxies)
                {
                    var p = proxy.Split(new[] { '=' }, rmvEmpty);
                    if (p[0] == "http")
                        http = new ProxyAddress(p[1]);
                    else if (p[0] == "https")
                        https = new ProxyAddress(p[1]);
                    else if (p[0] == "ftp")
                        ftp = new ProxyAddress(p[1]);
                    else if (p[0] == "socks")
                        socks = new ProxyAddress(p[1]);
                }

            nc._proxies = new ProxySettings(http, https, ftp, socks, overrides);

            return nc;
        }

        /// <summary>Sets the network configuration.</summary>
        /// <param name="config">The configuration to apply for the network.</param>
        /// <returns><c>true</c> if settings are applied successfully; otherwise <c>false</c>.</returns>
        public static bool SetConfig(ConnectionSettings config)
        {
            // main registry key
            var reg = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(keyName, true);

            // set the check boxes for the connection settings
            var checks = reg.OpenSubKey("Connections")
                            .GetValue("DefaultConnectionSettings", new byte[10])
                            .As<byte[]>();
            checks[8] = (byte)(config.ConnectionType);
            reg.OpenSubKey("Connections", true).SetValue("DefaultConnectionSettings",
                                                         checks, Microsoft.Win32.RegistryValueKind.Binary);
            reg.OpenSubKey("Connections", true).SetValue("SavedLegacySettings",
                                                         checks, Microsoft.Win32.RegistryValueKind.Binary);

            // proxy settings
            if (config.ConnectionType.HasFlag(ConnectionType.ManualProxy))
            {
                reg.SetValue("ProxyEnable", 1);
                reg.SetValue("ProxyServer", config.ManualProxy.ToString());
                reg.SetValue("ProxyOverride", config.ManualProxy.ProxyOverrides);
            }
            else
            {
                reg.DeleteValue("ProxyEnable", false);
                reg.DeleteValue("ProxyServer", false);
            }

            // auto config url
            if (config.ConnectionType.HasFlag(ConnectionType.AutoConfigUrl))
                reg.SetValue("AutoConfigUrl", config.AutoConfigUrl);
            else
                reg.DeleteValue("AutoConfigUrl", false);

            // apply the configuration
            bool result;
            result = InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
            result &= InternetSetOption(IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);

            return result;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(ConnectionSettings)) return false;

            var o = (ConnectionSettings)obj;
            var result = o != null && this != null;

            result &= o.ConnectionType == ConnectionType;
            result &= o.ConnectionType == ProxyManager.ConnectionType.AutoConfigUrl ?
                      o.AutoConfigUrl.Equals(AutoConfigUrl, StringComparison.OrdinalIgnoreCase) : true;
            result &= o.ConnectionType == ProxyManager.ConnectionType.ManualProxy ?
                      o.ManualProxy.NullCheckEquality(ManualProxy) : true;

            return result;
        }

        /// <summary>Returns a hash code for this instance.</summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        /// <summary>Returns a <see cref="System.String" /> that represents this ProxySettings instance.</summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            // switch on the flags present in the current connection type and return the first
            //   NB: The order is ManualProxy, AutoConfigUrl, AutoDetect when all flags are present
            foreach (var f in this.ConnectionType.GetAllFlagsPresent())
                switch (f)
                {
                    case ConnectionType.AutoDetect:
                        return "Auto detect";

                    case ConnectionType.AutoConfigUrl:
                        return "Auto Config URL: " + AutoConfigUrl;

                    case ConnectionType.ManualProxy:
                        return "Manual Proxy: " + _proxies.ToString(false);
                }

            if (this.ConnectionType == ProxyManager.ConnectionType.NoProxy)
                return "No Proxy";
            else
                return "<Unknown>";
        }

        [OnDeserialized()]
        private void OnDeserialized(StreamingContext sc)
        {
            if (_proxies == null) _proxies = new ProxySettings();
        }

        [OnSerializing()]
        private void OnSerializing(StreamingContext sc)
        {
            if (!ConnectionType.HasFlag(ConnectionType.ManualProxy))
                _proxies = null;
            if (!ConnectionType.HasFlag(ConnectionType.AutoConfigUrl))
                AutoConfigUrl = "";
        }

        [OnSerialized()]
        private void OnSerialized(StreamingContext sc)
        {
            if (_proxies == null) _proxies = new ProxySettings();
        }
    }
}