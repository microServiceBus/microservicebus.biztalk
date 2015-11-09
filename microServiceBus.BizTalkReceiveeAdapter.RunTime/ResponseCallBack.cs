using System;
using System.IO;
using System.Threading;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.TransportProxy.Interop;
using Microsoft.BizTalk.Message.Interop;


namespace microServiceBus.BizTalkReceiveeAdapter.RunTime
{
	/// <summary>
	/// Class for handling response from the BizTalk server
	/// </summary>
	internal class ResponseCallback : IBTTransmitter
	{
		private AutoResetEvent			_evt				= new AutoResetEvent(false);
		private IBaseMessage			_msg				= null;
		private TpBatchAsyncCallback	_batchCallBack		= null;
		private bool					_requestSuccess		= false;
        private IBTTransportProxy       _transportProxy     = null;


		internal static Guid GuidIBaseMessage = new Guid("fff93002-75a2-450a-8a39-53120ca8d8fa");

		public ResponseCallback(TpBatchAsyncCallback batchCallBack, IBTTransportProxy transportProxy)
		{
			_batchCallBack = batchCallBack;
		    _transportProxy = transportProxy;
		}

		/// <summary>
		/// Get a wait handle for the response thread
		/// </summary>
		public WaitHandle AsyncWaitHandle
		{
			get { return _evt; }
		}

		/// <summary>
		/// Get a response message
		/// </summary>
		internal IBaseMessage Response
		{
			get { return _msg; }
		}

		internal bool RequestSuccess
		{
			get { return _requestSuccess; }
		}

		/// <summary>
		/// Empty method, needed for IBTTransmitter interface
		/// </summary>
		/// <param name="transportProxy"></param>
		public void Initialize(IBTTransportProxy transportProxy)
		{
		}

		/// <summary>
		/// Empty method, needed for IBTTransmitter interface
		/// </summary>
		public void Terminate()
		{
		}

		/// <summary>
		/// Reads the response message from the server
		/// </summary>
		/// <param name="msg">Response message from the server</param>
		/// <returns>Flag to delete message from the server or not</returns>
        public bool TransmitMessage(IBaseMessage msg)
        {
            // Note: We need to read the stream which will execute the
            // pipeline, we then replace the stream with the one we
            // have created
            IBaseMessagePart bodyPart = msg.BodyPart;
            Stream s = null;
            if (bodyPart != null)
                s = bodyPart.GetOriginalDataStream();

            // Create a memory stream to copy the data into
            Stream memStrm = new MemoryStream();
            byte[] buff = new byte[4096];
            int dataRead = 0, readOffSet = 0, writeOffSet = 0;

            if (s != null)
            {
                s.Seek(0, SeekOrigin.Begin);

                // Copy the data from the src stream
                do
                {
                    dataRead = s.Read(buff, readOffSet, 4096);
                    memStrm.Write(buff, writeOffSet, dataRead);
                } while (dataRead > 0);
            }

            // Create a new message
            IBaseMessage response = BizTalkMessaging._mf.CreateMessage();

            // Copy over the context
            response.Context = msg.Context;
            
            // Copy over the body part, note, only support single part messages
            IBaseMessagePart messageBodyPart = BizTalkMessaging._mf.CreateMessagePart();
            messageBodyPart.Data = memStrm;

            memStrm.Seek(0, SeekOrigin.Begin);
            response.AddPart(msg.BodyPartName, messageBodyPart, true);

            _msg = response;

            ThreadPool.QueueUserWorkItem(DeleteTransmitMessage, msg);

            // Return false. 
            // We'll issue a Batch.DeleteMessage() later.
            return false;
        }

        private void DeleteTransmitMessage(object state)
        {
            IBTTransportBatch batch = _transportProxy.GetBatch(new DeleteBatchCallback(this._evt), null);
            batch.DeleteMessage((IBaseMessage)state);
            batch.Done(null);
        }


		/// <summary>
		/// Notifies the Response callback object that submission completed
		/// </summary>
		/// <param name="status">Status of submission</param>
		public void SubmitComplete(TpBatchStatus status)
		{
			if ( status.status >= 0 )
				_requestSuccess = true;
			else
			{
				_requestSuccess = false;
				_evt.Set();
			}
		}
	}
}
