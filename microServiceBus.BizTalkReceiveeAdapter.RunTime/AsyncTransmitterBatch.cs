using System;
using System.Collections;
using System.Xml;
using System.Runtime.InteropServices;
using Microsoft.BizTalk.TransportProxy.Interop;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Message.Interop;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microServiceBus.BizTalkReceiveeAdapter.RunTime
{
    public class AsyncTransmitterBatch : IBTTransmitterBatch
    {
        protected int maxBatchSize;
        protected Type endpointType;
        protected string propertyNamespace;
        protected IPropertyBag handlerPropertyBag;
        protected IBTTransportProxy transportProxy;
        protected AsyncTransmitter asyncTransmitter;

        private ArrayList messages;

        protected ArrayList Messages
        {
            get { return messages; }
        }

        private delegate void WorkerDelegate();

        public AsyncTransmitterBatch(int maxBatchSize, Type endpointType, string propertyNamespace, IPropertyBag handlerPropertyBag, IBTTransportProxy transportProxy, AsyncTransmitter asyncTransmitter)
        {
            this.maxBatchSize = maxBatchSize;
            this.endpointType = endpointType;
            this.propertyNamespace = propertyNamespace;
            this.handlerPropertyBag = handlerPropertyBag;
            this.transportProxy = transportProxy;
            this.asyncTransmitter = asyncTransmitter;

            this.messages = new ArrayList();

            // this.worker = new WorkerDelegate(Worker);
        }

        public string PropertyNamespace { get { return this.propertyNamespace; } }

        // IBTTransmitterBatch
        public object BeginBatch(out int nMaxBatchSize)
        {
            nMaxBatchSize = this.maxBatchSize;
            return null;
        }
        // Just build a list of messages for this batch - return false means we are asynchronous
        public bool TransmitMessage(IBaseMessage message)
        {
            this.messages.Add(message);
            return false;
        }
        public void Clear()
        {
            this.messages.Clear();
        }
        public void Done(IBTDTCCommitConfirm commitConfirm)
        {
            if (this.messages.Count == 0)
            {
                Exception ex = new InvalidOperationException("Send adapter received an emtpy batch for transmission from BizTalk");
                this.transportProxy.SetErrorInfo(ex);

                return;
            }

            //  The Enter/Leave is used to implement the Terminate call from BizTalk.

            //  Do an "Enter" for every message
            int MessageCount = this.messages.Count;
            for (int i = 0; i < MessageCount; i++)
            {
                if (!this.asyncTransmitter.Enter())
                    throw new InvalidOperationException("Send adapter Enter call was false within Done. This is illegal and should never happen."); ;
            }

            try
            {
                new WorkerDelegate(Worker).BeginInvoke(null, null);
            }
            catch (Exception)
            {
                //  If there was an error we had better do the "Leave" here
                for (int i = 0; i < MessageCount; i++)
                    this.asyncTransmitter.Leave();
            }
        }

        protected virtual void Worker()
        {
            //  Depending on the circumstances we might want to sort the messages form BizTalk into
            //  sub-batches each of which would be processed independently.

            //  If we sort into subbatches we will need a "Leave" for each otherwise do all the "Leaves" here
            //  In this code we have only one batch. If a sort into subbatches was added this number might change.
            int BatchCount = 1;
            int MessageCount = this.messages.Count;
            int LeaveCount = MessageCount - BatchCount;

            for (int i = 0; i < LeaveCount; i++)
                this.asyncTransmitter.Leave();

            bool needToLeave = true;

            try
            {
                using (Batch batch = new TransmitResponseBatch(this.transportProxy, new TransmitResponseBatch.AllWorkDoneDelegate(AllWorkDone)))
                {

                    foreach (IBaseMessage message in this.messages)
                    {
                        AsyncTransmitterEndpoint endpoint = null;

                        try
                        {
                            // Get appropriate endpoint for the message. Should always be non-null
                            endpoint = (AsyncTransmitterEndpoint)asyncTransmitter.GetEndpoint(message);

                            //  ask the endpoint to process the message
                            IBaseMessage responseMsg = endpoint.ProcessMessage(message);
                            //  success so tell the EPM we are finished with this message
                            batch.DeleteMessage(message);

                            if (responseMsg != null)
                            {
                                batch.SubmitResponseMessage(message, responseMsg);
                            }
                            //if we crash we could send duplicates - documentation point
                        }
                        catch (AdapterException e)
                        {
                            HandleException(e, batch, message);
                        }
                        catch (Exception e)
                        {
                            HandleException(new ErrorTransmitUnexpectedClrException(e.Message), batch, message);
                        }
                        finally
                        {
                            if (endpoint != null && endpoint.ReuseEndpoint == false)
                            {
                                endpoint.Dispose();
                                endpoint = null;
                            }
                        }
                    }

                    //  If the Done call is successful then the callback will be called and Leave will be called there
                    batch.Done(null);
                    needToLeave = false;  //  Done didn't throw so no Need To Leave
                }
            }
            finally
            {
                //  We need to Leave from this thread because the Done call failed and so there won't be any callback happening
                if (needToLeave)
                    this.asyncTransmitter.Leave();
            }
        }

        private void HandleException(AdapterException e, Batch batch, IBaseMessage message)
        {
            message.SetErrorInfo(e);

            SystemMessageContext context = new SystemMessageContext(message.Context);

            if (context.RetryCount > 0)
            {
                DateTime now = DateTime.Now;
                int retryInterval = context.RetryInterval;
                DateTime retryTime = now.AddMinutes(retryInterval);

                batch.Resubmit(message, retryTime);
            }
            else
            {
                batch.MoveToNextTransport(message);
            }
        }

        private void AllWorkDone()
        {
            this.asyncTransmitter.Leave();
        }
    }
}
