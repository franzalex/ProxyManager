using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ProxyManager
{
    [Serializable()]
    public class ProxySettings
    {
        private const string Local = "<local>";
        private List<string> _exceptions;
        private ProxyAddress _ftp;
        private ProxyAddress _http;
        private ProxyAddress _https;
        private ProxyAddress _socks;
        private bool serializing;

        /// <summary>Initializes a new instance of the <see cref="ProxySettings"/> class.</summary>
        public ProxySettings()
            : this(new ProxyAddress()) { }

        /// <summary>Initializes a new instance of the <see cref="ProxySettings"/> class.</summary>
        /// <param name="proxyAddress">The proxy address to use for all protocols.</param>
        public ProxySettings(ProxyAddress proxyAddress)
            : this(proxyAddress, Local) { }

        /// <summary>Initializes a new instance of the <see cref="ProxySettings" /> class.</summary>
        /// <param name="proxyAddress">The proxy address to use for all protocols.</param>
        /// <param name="exceptions">The host addresses that do not require a proxy server.</param>
        public ProxySettings(ProxyAddress proxyAddress, params string[] exceptions)
            : this(proxyAddress, null, null, null, exceptions) { }

        /// <summary>Initializes a new instance of the <see cref="ProxySettings"/> class.</summary>
        /// <param name="httpProxy">The proxy address to use for the HTTP protocol.</param>
        /// <param name="httpsProxy">The proxy address to use for the HTTPS protocol.</param>
        /// <param name="ftpProxy">The proxy address to use for the FTP protocol.</param>
        /// <param name="socksProxy">The proxy address to use for the SOCKS protocol.</param>
        public ProxySettings(ProxyAddress httpProxy, ProxyAddress httpsProxy,
                             ProxyAddress ftpProxy, ProxyAddress socksProxy)
            : this(httpProxy, httpsProxy, ftpProxy, socksProxy, Local) { }

        /// <summary>Initializes a new instance of the <see cref="ProxySettings" /> class.</summary>
        /// <param name="httpProxy">The proxy address to use for the HTTP protocol.</param>
        /// <param name="httpsProxy">The proxy address to use for the HTTPS protocol.</param>
        /// <param name="ftpProxy">The proxy address to use for the FTP protocol.</param>
        /// <param name="socksProxy">The proxy address to use for the SOCKS protocol.</param>
        /// <param name="exceptions">The host addresses which do not require a proxy server.</param>
        public ProxySettings(ProxyAddress httpProxy, ProxyAddress httpsProxy,
                             ProxyAddress ftpProxy, ProxyAddress socksProxy,
                             params string[] exceptions)
        {
            _http = httpProxy;
            _https = httpsProxy != null ? httpsProxy : ProxyAddress.Default;
            _ftp = ftpProxy != null ? ftpProxy : ProxyAddress.Default;
            _socks = socksProxy != null ? socksProxy : ProxyAddress.Default;
            _exceptions = new List<string>(exceptions);
            _exceptions.RemoveAll((s) => s.IsNullOrBlank()); // remove blanks
            BypassForLocal = _exceptions.Remove(Local);      // set the local bypass
        }

        /// <summary>
        /// Gets or sets a value indicating whether the proxy server should be bypassed for local addresses.
        /// </summary>
        /// <value>
        /// <c>true</c> if proxy server should be bypass for local addresses; otherwise, <c>false</c>.
        /// </value>
        [DefaultValue(true)]
        public bool BypassForLocal { get; set; }

        /// <summary>Gets or sets the hosts which do not require proxy addresses.</summary>
        /// <value>The hosts which do not require proxy addresses.</value>
        [DefaultValue(null)]
        public List<string> BypassList
        {
            get { return _exceptions; }
        }

        /// <summary>Gets or sets the proxy address to use for the FTP protocol.</summary>
        /// <value>The FTP proxy address.</value>
        public ProxyAddress Ftp
        {
            get { return _ftp; }
            set { _ftp = value; }
        }

        /// <summary>Gets or sets the proxy address to use for the HTTP protocol.</summary>
        /// <value>The HTTP proxy address.</value>
        public ProxyAddress Http
        {
            get { return _http; }
            set { _http = value; }
        }

        /// <summary>Gets or sets the proxy address to use for the HTTPS protocol.</summary>
        /// <value>The HTTPS proxy address.</value>
        public ProxyAddress Https
        {
            get { return _https; }
            set { _https = value; }
        }

        /// <summary>Gets the hosts that do not require a proxy server.</summary>
        [DefaultValue(null)]
        public string ProxyOverrides
        {
            get
            {
                if (serializing) return null;

                var result = new List<string>();

                // copy non-blank addresses to the result
                foreach (var ex in _exceptions)
                    if (!ex.IsNullOrBlank())
                        result.Add(ex.Trim());

                if (BypassForLocal) result.Add(Local);

                return string.Join(";", result.ToArray());
            }
        }

        /// <summary>Gets a value indicating whether all protocols use the same proxy address.</summary>
        /// <value><c>true</c> if all protocols use the same proxy address; otherwise, <c>false</c>.</value>
        [DefaultValue(false)]
        public bool SingleProxy
        {
            get
            {
                var result = true;

                // all other protocols must be null, have no address or equal to HTTP's proxy
                foreach (var addr in new[] { _https, _ftp, _socks })
                    result &= addr == null || !addr.HasAddress || addr.Equals(_http);

                return result;
            }
        }

        /// <summary>Gets or sets the proxy address to use for the SOCKS protocol.</summary>
        /// <value>The SOCKS proxy address.</value>
        public ProxyAddress Socks
        {
            get { return _socks; }
            set { _socks = value; }
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
            return obj.GetType() == typeof(ProxySettings) &&
                   ((ProxySettings)obj).With(p => p._http.Equals(_http) &&
                                                       p._https.Equals(_https) &&
                                                       p._ftp.Equals(_ftp) &&
                                                       p._socks.Equals(_socks) &&
                                                       p.ProxyOverrides == ProxyOverrides);
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
            return this.ToString(true);
        }

        /// <summary>Returns a <see cref="System.String" /> that represents this instance.</summary>
        /// <param name="allAddresses">
        /// if set to <c>true</c> returns all proxy addresses when proxy addresses are different.
        /// </param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public string ToString(bool allAddresses)
        {
            if (SingleProxy)
                return Http != null && Http.HasAddress ? Http.ToString() : "";
            else
            {
                if (!allAddresses)
                    return "<multiple>";

                var l = new List<string>();

                foreach (var p in new[]{new {Protocol = "http",  Address = _http},
                                        new {Protocol = "https", Address = _https},
                                        new {Protocol = "ftp",   Address = _ftp},
                                        new {Protocol = "socks", Address = _ftp}})
                {
                    if (p.Address != null && p.Address.HasAddress)
                        l.Add(p.Protocol + "=" + p.Address.ToString());
                }

                return string.Join(";", l.ToArray());
            }
        }

        #region Pre- and post-serialization Methods

        [System.Runtime.Serialization.OnDeserialized()]
        private void OnDeserialized(System.Runtime.Serialization.StreamingContext c)
        {
            serializing = false;

            if (_http == null) _http = ProxyAddress.Default;
            if (_https == null) _https = ProxyAddress.Default;
            if (_ftp == null) _ftp = ProxyAddress.Default;
            if (_socks == null) _socks = ProxyAddress.Default;
        }

        [System.Runtime.Serialization.OnSerialized()]
        private void OnSerialized(System.Runtime.Serialization.StreamingContext c)
        {
            serializing = false;

            if (_http == null) _http = ProxyAddress.Default;
            if (_https == null) _https = ProxyAddress.Default;
            if (_ftp == null) _ftp = ProxyAddress.Default;
            if (_socks == null) _socks = ProxyAddress.Default;
        }

        [System.Runtime.Serialization.OnSerializing()]
        private void OnSerializing(System.Runtime.Serialization.StreamingContext c)
        {
            serializing = true;
            _exceptions.RemoveAll((s) => s.IsNullOrBlank());

            if (!_http.HasAddress) _http = null;
            if (!_https.HasAddress) _https = null;
            if (!_ftp.HasAddress) _ftp = null;
            if (!_socks.HasAddress) _socks = null;
        }

        #endregion Pre- and post-serialization Methods
    }
}