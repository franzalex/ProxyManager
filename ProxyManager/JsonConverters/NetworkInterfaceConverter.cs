using System;
using JsonToken = Newtonsoft.Json.JsonToken;
using NetworkInterfaceType = System.Net.NetworkInformation.NetworkInterfaceType;

namespace ProxyManager
{
    partial class NetworkInterfaceInternal
    {
        public class JsonConverter : Newtonsoft.Json.JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(NetworkInterfaceInternal);
            }

            /// <summary>Reads the JSON representation of the object.</summary>
            /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
            /// <param name="objectType">Type of the object.</param>
            /// <param name="existingValue">The existing value of object being read.</param>
            /// <param name="serializer">The calling serializer.</param>
            /// <returns>The object value.</returns>
            public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType,
                                            object existingValue, Newtonsoft.Json.JsonSerializer serializer)
            {
                var ni = new NetworkInterfaceInternal(null);

                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.EndObject)
                        break;
                    if (reader.TokenType != JsonToken.PropertyName)
                        continue;

                    var propName = reader.Value.ToString(); // property name

                    if (propName == PropertyNames.Description)
                        ni._descr = reader.ReadAsString();
                    else if (propName == PropertyNames.Id)
                        ni._id = reader.ReadAsString();
                    else if (propName == PropertyNames.InterfaceType)
                        ni._interfaceType = (NetworkInterfaceType)reader.ReadAsInt32().Value;
                    else if (propName == PropertyNames.Name)
                        ni._name = reader.ReadAsString();
                    else if (propName == PropertyNames.PhysicalAddress)
                        ni._mac = reader.ReadAsBytes();
                }

                return ni;
            }

            /// <summary>Writes the JSON representation of the object.</summary>
            /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
            /// <param name="value">The value.</param>
            /// <param name="serializer">The calling serializer.</param>
            public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object value,
                                           Newtonsoft.Json.JsonSerializer serializer)
            {
                writer.WriteStartObject();

                var ni = (NetworkInterfaceInternal)value;

                foreach (var propName in PropertyNames.GetAllNames())
                {
                    object propValue = null;
                    if (propName == PropertyNames.Description)
                        propValue = ni.Description;
                    else if (propName == PropertyNames.Id)
                        propValue = ni.Id;
                    else if (propName == PropertyNames.InterfaceType)
                        propValue = ni.InterfaceType;
                    else if (propName == PropertyNames.MacAddress)
                        propValue = ni.MacAddress;
                    else if (propName == PropertyNames.Name)
                        propValue = ni.Name;
                    else if (propName == PropertyNames.PhysicalAddress)
                        propValue = ni.PhysicalAddress;

                    if (propValue != null)
                    {
                        writer.WritePropertyName(propName);
                        writer.WriteValue(propValue);
                    }
                }

                writer.WriteEndObject();
            }
        }
    }
}