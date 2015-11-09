using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Collections;
using System.Threading;
using Microsoft.BizTalk.TransportProxy.Interop;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Message.Interop;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace microServiceBus.BizTalkReceiveeAdapter.RunTime
{
    public class microServiceBusTransmitterEndpoint : AsyncTransmitterEndpoint
    {

        #region Private Fields
        
        private bool _shutdownRequested;
        private AsyncTransmitter _asyncTransmitter = null;
        private string _propertyNamespace;
        #endregion

        public microServiceBusTransmitterEndpoint(AsyncTransmitter asyncTransmitter)
            : base(asyncTransmitter)
		{
            this._asyncTransmitter = asyncTransmitter;
        }
        public override void Open(
            EndpointParameters endpointParameters,
            IPropertyBag handlerPropertyBag,
            string propertyNamespace)
        {
            this._propertyNamespace = propertyNamespace;
        }
        public override IBaseMessage ProcessMessage(IBaseMessage message)
        {
            var propertyNS = "http://microservicebus.biztalk";
            var properties = new microServiceBusTransmitProperties(message, propertyNS);
           
            try
            {
                if (!this._shutdownRequested)
                {
                    try
                    {
                        var client = new TcpClient();
                        // Connect
                        client.Connect(properties.HostAddress, properties.Port);
                        
                        // Send
                        var stream = client.GetStream();
                        // Create messaege
                        var btsStream = message.BodyPart.Data;
                        btsStream.CopyTo(stream);
                        
                        // Receive
                        var bytesToRead = new byte[client.ReceiveBufferSize];
                        var bytesRead = stream.Read(bytesToRead, 0, client.ReceiveBufferSize);
                        
                        client.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                
            }
            return null;
        }
        
        /// <summary>
        /// Executed on termination (Stop Host instance)
        /// </summary>
        public override void Dispose()
        {
            this._shutdownRequested = true;
            base.Dispose();
            
        }
        
    }
}
