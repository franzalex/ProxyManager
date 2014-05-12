using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ProxyManager
{
    internal class ConnectionSettingsDescriptor : ExpandableObjectConverter
    {
        internal static readonly string AutoConfigUrl;
        internal static readonly string FtpProxy;
        internal static readonly string HttpProxy;
        internal static readonly string HttpsProxy;
        internal static readonly string SocksProxy;
        internal static readonly string ConnType;
        internal static readonly string UseSingleProxy;

        static string GetPropertyName<T>(System.Linq.Expressions.Expression<Func<T>> exp)
        {
            return (((System.Linq.Expressions.MemberExpression)(exp.Body)).Member).Name;
        }

        static ConnectionSettingsDescriptor()
        {
            var c = new ConnectionSettings();
            AutoConfigUrl = GetPropertyName(() => c.AutoConfigUrl);
            FtpProxy = GetPropertyName(() => c.FtpProxy);
            HttpProxy = GetPropertyName(() => c.HttpProxy);
            HttpsProxy = GetPropertyName(() => c.HttpsProxy);
            SocksProxy = GetPropertyName(() => c.SocksProxy);
            ConnType = GetPropertyName(() => c.Type);
            UseSingleProxy = GetPropertyName(() => c.SingleProxy);
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context,
                                                                   object value, Attribute[] attributes)
        {
            //
            // This override returns a list of properties in order
            //
            PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(value, attributes);
            var mode = ((ConnectionSettings)value).Type;
            var singleProxy = ((ConnectionSettings)value).SingleProxy;

            var ordered = new SortedList<int, string>();
            var unOrdered = new List<string>();
            foreach (PropertyDescriptor pd in pdc)
            {
                // check if the property has the property order defined
                Attribute attribute = pd.Attributes[typeof(PropertyOrderAttribute)];
                if (attribute != null)
                {
                    // add to the ordered list
                    var poa = (PropertyOrderAttribute)attribute;
                    ordered.Add(poa.Order, pd.Name);
                }
                else
                {
                    // no order specified; add it to the unordered list
                    unOrdered.Add(pd.Name);
                }
            }

            // sort the unordered properties (will sort by name)
            unOrdered.Sort();

            // append the sorted properties
            foreach (var i in ordered)
                unOrdered.Add(i.Value);

            //
            // Pass in the ordered list for the PropertyDescriptorCollection to sort by
            //
            return FilterProperties(pdc.Sort(unOrdered.ToArray()), mode, singleProxy);
        }

        private PropertyDescriptorCollection FilterProperties(PropertyDescriptorCollection pdc,
                                                              ConnectionType ct, bool singleProxy)
        {
            var propsToKeep = new List<string>(new[] { ConnType });
            var readOnlyProps = new List<string>(new[] { FtpProxy, HttpsProxy, SocksProxy });

            switch (ct)
            {
                case ConnectionType.AutoConfig:
                    propsToKeep.Add(AutoConfigUrl);
                    break;

                case ConnectionType.ManualProxy:
                    if (!singleProxy)
                    {
                        propsToKeep.Add(FtpProxy);
                        propsToKeep.Add(HttpsProxy);
                        propsToKeep.Add(SocksProxy);
                    }
                    propsToKeep.Add(HttpProxy);
                    propsToKeep.Add(UseSingleProxy);
                    break;

                default:
                    break;
            }

            for (int i = pdc.Count - 1; i >= 0; i--)
            {
                if (!propsToKeep.Contains(pdc[i].Name))
                {
                    pdc.RemoveAt(i);
                    continue;
                }
                //    // http://www.codeproject.com/Articles/152945/Enabling-disabling-properties-at-runtime-in-the-Pr
                //if (ct == ConnectionType.ManualProxy)
                //{
                //    var pd = (PropertyDescriptor)pdc[i];
                //    var attrib = (ReadOnlyAttribute)pd.Attributes[typeof(ReadOnlyAttribute)];
                //    var field = attrib.GetType().GetField("isReadOnly",
                //                                          BindingFlags.NonPublic | BindingFlags.Instance);
                //    field.SetValue(attrib, !(readOnlyProps.Contains(pd.Name) && singleProxy));
                //}
            }

            return pdc;
        }
    }
}