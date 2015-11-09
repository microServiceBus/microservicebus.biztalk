using System;
using System.Runtime.InteropServices;
using System.Collections;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.TransportProxy.Interop;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microServiceBus.BizTalkReceiveeAdapter.RunTime
{
    public class AsyncTransmitter :
        Adapter,
        System.IDisposable,
        IBTBatchTransmitter
    {
        //  default magic number
        private const int MAX_BATCH_SIZE = 50;

        //  members to initialize the batch with
        private int maxBatchSize = MAX_BATCH_SIZE;
        private Type endpointType;
        private Hashtable endpoints = new Hashtable();

        private ControlledTermination control;

        protected AsyncTransmitter(
            string name,
            string version,
            string description,
            string transportType,
            Guid clsid,
            string propertyNamespace,
            Type endpointType,
            int maxBatchSize)
            : base(
            name,
            version,
            description,
            transportType,
            clsid,
            propertyNamespace)
        {
            this.endpointType = endpointType;
            this.maxBatchSize = maxBatchSize;
            this.control = new ControlledTermination();
        }

        protected virtual int MaxBatchSize
        {
            get { return this.maxBatchSize; }
        }

        protected Type EndpointType
        {
            get { return this.endpointType; }
        }

        protected ControlledTermination ControlledTermination { get { return this.control; } }

        public void Dispose()
        {
            control.Dispose();
        }

        // IBTBatchTransmitter
        public IBTTransmitterBatch GetBatch()
        {
            IBTTransmitterBatch tb = CreateAsyncTransmitterBatch();

            return tb;
        }

        protected virtual IBTTransmitterBatch CreateAsyncTransmitterBatch()
        {
            return new AsyncTransmitterBatch(
                this.MaxBatchSize,
                this.EndpointType,
                this.PropertyNamespace,
                this.HandlerPropertyBag,
                this.TransportProxy,
                this);
        }

        // Endpoint management is the responsibility of the transmitter
        protected virtual EndpointParameters CreateEndpointParameters(IBaseMessage message)
        {
            SystemMessageContext context = new SystemMessageContext(message.Context);
            return new DefaultEndpointParameters(context.OutboundTransportLocation);
        }

        public virtual AsyncTransmitterEndpoint GetEndpoint(IBaseMessage message)
        {
            // Provide a virtual "CreateEndpointParameters" method to map message to endpoint
            EndpointParameters endpointParameters = CreateEndpointParameters(message);

            lock (endpoints)
            {
                AsyncTransmitterEndpoint endpoint = (AsyncTransmitterEndpoint)endpoints[endpointParameters.SessionKey];
                if (null == endpoint)
                {
                    //  we haven't seen this location so far this batch so make a new endpoint
                    endpoint = (AsyncTransmitterEndpoint)Activator.CreateInstance(this.endpointType, new object[] { this });

                    if (null == endpoint)
                        throw new CreateEndpointFailed(this.endpointType.FullName, endpointParameters.OutboundLocation);

                    endpoint.Open(endpointParameters, this.HandlerPropertyBag, this.PropertyNamespace);

                    if (endpoint.ReuseEndpoint)
                    {
                        endpoints[endpointParameters.SessionKey] = endpoint;
                    }
                }
                return endpoint;
            }
        }

        public override void Terminate()
        {
            try
            {
                //  Block until we are done...
                // Let all endpoints finish the work they are doing before disposing them
                this.control.Terminate();

                foreach (AsyncTransmitterEndpoint endpoint in endpoints.Values)
                {
                    //  clean up and potentially close any endpoints
                    try
                    {
                        endpoint.Dispose();
                    }
                    catch (Exception e)
                    {
                        this.TransportProxy.SetErrorInfo(e);
                    }
                }

                base.Terminate();
            }
            finally
            {
                this.Dispose();
            }
        }

        public bool Enter()
        {
            return this.control.Enter();
        }

        public void Leave()
        {
            this.control.Leave();
        }
    }
}
