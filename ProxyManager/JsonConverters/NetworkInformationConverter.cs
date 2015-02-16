using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ProxyManager
{
    internal partial class NetworkConnectionInfo
    {
        public class JsonConverter : Newtonsoft.Json.JsonConverter
        {
            private const string sep = " | ";

            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(NetworkConnectionInfo);
            }

            /// <summary>Reads the JSON representation of the object.</summary>
            /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
            /// <param name="objectType">Type of the object.</param>
            /// <param name="existingValue">The existing value of object being read.</param>
            /// <param name="serializer">The calling serializer.</param>
            /// <returns>The object value.</returns>
            public override object ReadJson(JsonReader reader, Type objectType,
                                            object existingValue, JsonSerializer serializer)
            {
                var ni = new NetworkConnectionInfo(null);

                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.EndObject)
                        break;
                    if (reader.TokenType != JsonToken.PropertyName)
                        continue;

                    var propName = reader.Value.ToString(); // property name

                    if (PropertyNames.GetIPAddressPropertyNames().Contains(propName))
                    {
                        var values = reader.ReadAsString().Split(new[] { sep }, StringSplitOptions.RemoveEmptyEntries);
                        ni.ntwkInfo[propName] = values.Select(s => IPAddress.Parse(s)).ToArray();
                    }
                    else if (propName == PropertyNames.NetworkInterface)
                    {
                        // add a json converter for NetworkInterfaceInternal if none present
                        if (!serializer.Converters.Where(c => c.CanConvert(typeof(NetworkInterfaceInternal))).Any())
                            serializer.Converters.Add(new NetworkInterfaceInternal.JsonConverter());

                        reader.Read();
                        ni.nic = serializer.Deserialize<NetworkInterfaceInternal>(reader);
                    }
                    else if (propName == PropertyNames.AutoAssignedIP)
                    {
                        ni._autoAssignedAddress = bool.Parse(reader.ReadAsString());
                    }
                    else if (propName == PropertyNames.DnsSuffix)
                    {
                        ni._dnsSuffix = reader.ReadAsString();
                    }
                }

                return ni;
            }

            /// <summary>Writes the JSON representation of the object.</summary>
            /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
            /// <param name="value">The value.</param>
            /// <param name="serializer">The calling serializer.</param>
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                writer.WriteStartObject();
                var ni = (NetworkConnectionInfo)value;

                // add a json converter for NetworkInterfaceInternal if none present
                if (!serializer.Converters.Where(c => c.CanConvert(typeof(NetworkInterfaceInternal))).Any())
                    serializer.Converters.Add(new NetworkInterfaceInternal.JsonConverter());

                writer.WritePropertyName(PropertyNames.NetworkInterface);
                serializer.Serialize(writer, ni.NetworkInterface);

                // serialize the ip properties
                writer.WritePropertyName(PropertyNames.DnsSuffix);
                serializer.Serialize(writer, ni.DnsSuffix);

                foreach (var prop in new[] { new{Name = PropertyNames.IPv4Address, Value = ni.IPv4Address},
                                             new{Name = PropertyNames.IPv4Subnet , Value = ni.IPv4SubnetMask}})
                {
                    if (prop.Value != null && prop.Value != IPAddress.None)
                    {
                        writer.WritePropertyName(prop.Name);
                        writer.WriteValue(prop.Value.ToString());
                    }
                }

                writer.WritePropertyName(PropertyNames.AutoAssignedIP);
                writer.WriteValue(ni.AutoAssignedIPAddress.ToString());

                foreach (var prop in new[] {new{Name = PropertyNames.DefaultGateway, Value = ni.DefaultGateway},
                                            new{Name = PropertyNames.DhcpServer,     Value = ni.DhcpServer},
                                            new{Name = PropertyNames.DnsServer,      Value = ni.DnsServer}})
                {
                    if (prop.Value.Length > 0)
                    {
                        writer.WritePropertyName(prop.Name);
                        var l = new List<string>();
                        foreach (var ip in prop.Value)
                            l.Add(ip.ToString());
                        writer.WriteValue(string.Join(sep, l.ToArray()));
                    }
                }


                writer.WriteEndObject();
            }
        }
    }
}
