using Microsoft.BizTalk.Message.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace microServiceBus.BizTalkReceiveeAdapter.RunTime
{
    internal class microServiceBusTransmitProperties : ConfigProperties
    {
        public string HostAddress { get; set; }
        public int Port { get; set; }
        public string ContentType { get; set; }
        public string Uri
        {
            get
            {
                return string.Format("microservicebus://{0}:{1}", this.HostAddress, this.Port);
            }
        }

        #region Public Methods
        public microServiceBusTransmitProperties(IBaseMessage message, string propertyNamespace)
        {
            XmlDocument locationConfigDom = null;

            //  get the adapter configuration off the message
            IBaseMessageContext context = message.Context;
            string config = (string)context.Read("AdapterConfig", propertyNamespace);

            if (null != config)
            {
                locationConfigDom = new XmlDocument();
                locationConfigDom.LoadXml(config);

                this.ReadLocationConfiguration(locationConfigDom);
            }
            else //  the config can be null all that means is that we are doing a dynamic send
            {
                this.ReadLocationConfiguration(message.Context);
            }
        }
        public void ReadLocationConfiguration(XmlDocument endpointConfig)
        {
            this.HostAddress = Extract(endpointConfig, "CustomProps/address", "localhost");
            this.Port = ExtractInt(endpointConfig, "CustomProps/port");
            this.ContentType = Extract(endpointConfig, "CustomProps/contentType", "application/xml");
        }
        public void ReadLocationConfiguration(IBaseMessageContext context)
        {
            string propertyNS = "http://microservicebus.biztalk";
            this.HostAddress = (string)Extract(context, "address", propertyNS, false, true);
            this.Port = (int)Extract(context, "port", propertyNS, false, true);
            this.ContentType = (string)Extract(context, "contentType", propertyNS, false, true);
        }
        public static void ReadTransmitHandlerConfiguration(XmlDocument configDOM)
        {
            // Handler properties
        }
        #endregion
        #region Private Methods
        private object Extract(IBaseMessageContext context, string prop, string propNS, object fallback, bool isRequired)
        {
            Object o = context.Read(prop, propNS);
            if (!isRequired && null == o)
                return fallback;
            if (null == o)
                throw new NoSuchProperty(propNS + "#" + prop);
            return o;
        }

        #endregion
    }
}
