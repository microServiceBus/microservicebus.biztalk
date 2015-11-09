using System;
using System.Xml;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.Component.Interop;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microServiceBus.BizTalkReceiveeAdapter.RunTime
{
    public abstract class EndpointParameters
    {
        public abstract string SessionKey { get; }
        public string OutboundLocation { get { return this.outboundLocation; } }
        public EndpointParameters(string outboundLocation)
        {
            this.outboundLocation = outboundLocation;
        }
        protected string outboundLocation;
    }

    internal class DefaultEndpointParameters : EndpointParameters
    {
        public override string SessionKey
        {
            //  the SessionKey is the outboundLocation in the default case
            get { return this.outboundLocation; }
        }
        public DefaultEndpointParameters(string outboundLocation) : base(outboundLocation)
        {
        }
    }

    public abstract class AsyncTransmitterEndpoint : System.IDisposable
    {
        public AsyncTransmitterEndpoint(AsyncTransmitter transmitter) { }

        public virtual bool ReuseEndpoint { get { return true; } }
        public abstract void Open(EndpointParameters endpointParameters, IPropertyBag handlerPropertyBag, string propertyNamespace);
        public abstract IBaseMessage ProcessMessage(IBaseMessage message);
        public virtual void Dispose() { }
    }
}
