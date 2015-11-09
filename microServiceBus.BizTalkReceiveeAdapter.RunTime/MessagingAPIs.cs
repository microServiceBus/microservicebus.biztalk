using System;
using System.IO;
using System.Xml;
using System.Collections;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.TransportProxy.Interop;
using Microsoft.BizTalk.Message.Interop;
using System.Diagnostics;
using System.Runtime.InteropServices;



namespace microServiceBus.BizTalkReceiveeAdapter.RunTime
{
	/// <summary>
	/// Implements the receive adapter for direct message submission
	/// </summary>
	/// <remarks>
	/// This class is the implementation of receive adapter that is used 
	/// for direct submission of messages into the BizTalk Server
	/// </remarks>
	public class BizTalkMessaging : BizTalkMessagingBase, IBTTransport, IBaseComponent, IBTTransportConfig
	{
		private static string				_name			= "microServiceBus";
		private static string				_version		= "1.0";
        private static string               _description    = "microServiceBus";
        private static string               _protocol       = "microServiceBus";
        private static Guid                 _clsid          = new Guid("11E62A28-D6C8-41ed-AF7F-369EFBFDA2BE");
		private IBTTransportProxy			_tp				= null;
		internal static IBaseMessageFactory	_mf				= null;
		private bool						_initialized	= false;
	
#region IBTTransport
		
		/// <summary>
		/// Name of the adapter
		/// </summary>
		public string Name
		{
			get { return _name; }
		}
        
		/// <summary>
		/// Version of the adapter
		/// </summary>
		public string Version
		{
			get { return _version; }
		}
        
		/// <summary>
		/// Description of the adapter
		/// </summary>
		public string Description
		{
			get { return _description; }
		}

		/// <summary>
		/// Type of the adapter
		/// </summary>
		public string TransportType 
		{
			get { return _protocol; }
		}

		/// <summary>
		/// Class ID of the adapter
		/// </summary>
		public Guid ClassID
		{
			get { return _clsid; }
		}

#endregion // IBTTransport

#region IBTTransportConfig

		/// <summary>
		/// Adds a receive location configuration to the adapter.
		/// Not used in this adapter
		/// </summary>
		public void AddReceiveEndpoint(string url, IPropertyBag adapterConfig, IPropertyBag bizTalkConfig)
		{
		}

		/// <summary>
		/// Updates a receive location configuration fot the adapter
		/// Not used in this adapter
		/// </summary>
		public void UpdateEndpointConfig(string url, IPropertyBag adapterConfig, IPropertyBag bizTalkConfig)
		{
		}

		/// <summary>
		/// Removes a receive location from the adapter
		/// Not used in this adapter
		/// </summary>
		public void RemoveReceiveEndpoint(string url )
		{
		}
#endregion

#region Constructors and Initialization

		/// <summary>
		/// Creatable receiver constructor
		/// </summary>
		public BizTalkMessaging()
		{
		}

		/// <summary>
		/// Creatable receiver constructor
		/// </summary>
		/// <param name="tp">Transport proxy object reference</param>
		public BizTalkMessaging(IBTTransportProxy tp)
		{
			if ( null == tp )
				throw new ArgumentNullException("IBTTransportProxy may not be null");

			this._tp = tp;
			_mf = _tp.GetMessageFactory();

			_initialized = true;
		}

		/// <summary>
		/// Initialize the adapter by explicitely specifying the address of receive location
		/// </summary>
		/// <param name="url>Address of the receive location that uses this adapter</param>
		public void Initialize(string url)
		{
			if ( null == url )
				throw new ArgumentNullException("Url may not be null");

			InternalInitialize( null, url, null );
		}

		/// <summary>
		/// Initialize the adapter by taken the address of receive location from message context
		/// </summary>
		/// <param name="tp">Transport proxy object reference</param>
		/// <param name="message">BizTalk message object reference</param>
		/// <param name="callBack">Reference to the configuration callback interface</param>
		private void InternalInitialize(IBTTransportProxy tp, IBaseMessage message, IBTTransportConfig callBack)
		{
			IBaseMessageContext	ctx;
			ctx = message.Context;
			SystemMessageContext smc = new SystemMessageContext(ctx);
			string url = smc.InboundTransportLocation;

			InternalInitialize(tp, url, callBack);
		}
		
		/// <summary>
		/// Initialize the adapter by specifying the address of the receive location that uses the adapter
		/// </summary>
		/// <param name="tp">Transport proxy object reference</param>
		/// <param name="url">Address of the receive location</param>
		/// <param name="callBack">Reference to the configuration callback interface</param>
		private void InternalInitialize(IBTTransportProxy tp, string url, IBTTransportConfig callBack)
		{
			lock(this)
			{
				while ( false == _initialized )
				{
					IBTTransportConfig cb = null;

					if ( null == url )
						throw new ArgumentNullException("Url may not be null");

					if ( null == callBack )
					{
						cb = this;
					}
					else
					{
						cb = callBack;
					}
				
					if ( null == tp && null == _tp )
					{
						_tp = new IBTTransportProxy();
					}
					else if ( null != tp )
					{
						_tp = tp;
					}

					_tp.RegisterIsolatedReceiver(url, cb);
					_mf = _tp.GetMessageFactory();

					_initialized = true;
				}
			}
		}

		/// <summary>
		/// Initialize the adapter by explicitely setting the receive location URI.
		/// </summary>
		/// <param name="tp">Transport proxy object reference</param>
		/// <param name="url">Address of the receive location that used this adapter</param>
		/// <param name="callBack">Reference to the configuration callback interface</param>
		public void Initialize(IBTTransportProxy tp, string url, IBTTransportConfig callBack)
		{
			InternalInitialize( tp, url, callBack );
		}

		/// <summary>
		/// Close the adapter
		/// </summary>
		public void Close()
		{
			if ( null != _tp )
			{
				_tp.TerminateIsolatedReceiver();
			}

            _initialized = false;
			_tp = null;
		}


#endregion // Constructors and Initialization

#region Single Message Submition API's

		/// <summary>
		/// Submits a BizTalk message object through the receive location to the server
		/// </summary>
		/// <param name="message">The IBaseMessage reference</param>
		/// <returns>Status of message submission</returns>
		/// <remarks>
		/// The url of the receive locatoin must be stamped on the message context
		/// prior to calling this method
		/// </remarks>
		override public bool SubmitMessage(
								IBaseMessage					message)
		{
			IAsyncResult ar = BeginSubmitMessage( message, null, null);
			return EndSubmitMessage(ar);
		}

		override public IAsyncResult BeginSubmitMessage(
								IBaseMessage					message, 
								AsyncCallback					cb, 
								Object							asyncState)
		{
			if ( false == _initialized )
			{
				InternalInitialize(null, message, null);
			}

			EpmAsyncResult ar = new EpmAsyncResult(cb, asyncState);

			// Get a new Transport Proxy Batch
			IBTTransportBatch batch = _tp.GetBatch( ar.BatchCallback, null );

			// Submit the message
			batch.SubmitMessage( message );

			// Commit the batch
			batch.Done( null );

			return ar;
		}

		override public bool EndSubmitMessage(
								IAsyncResult					ar)
		{
			if ( false == _initialized )
				throw new InvalidOperationException("BizTalkMessaging is not initialized.");

			EpmAsyncResult tpAr = (EpmAsyncResult)ar;
			TpBatchStatus tpStatus = tpAr.BatchCallback.GetStatus();
			if ( 0 <= tpStatus.status )
				return true;
			else 
				return false;
		}

#endregion // Single Message Submition API's

#region Batched Submition API's

		/// <summary>
		/// Submits an array of BizTalk messages through a receive location into the server
		/// </summary>
		/// <param name="messages">Array of IBaseMessage objects</param>
		/// <returns>Status of submission</returns>
		/// <remarks>
		/// Each message in array must have an address of receive location stamped on a context
		/// </remarks>
		override public bool SubmitMessages(
			IBaseMessage[]					messages)
		{
			IAsyncResult ar = BeginSubmitMessages( messages, null, null);
			return EndSubmitMessage(ar);
		}

		override public IAsyncResult BeginSubmitMessages(
			IBaseMessage[]					messages, 
			AsyncCallback					cb, 
			Object							asyncState)
		{
			if ( false == _initialized )
			{
				InternalInitialize(null, messages[0], null);
			}

			EpmAsyncResult ar = new EpmAsyncResult(cb, asyncState);
			// Get a new Transport Proxy Batch
			IBTTransportBatch batch = _tp.GetBatch( ar.BatchCallback, null );

			for ( int c = 0; c < messages.Length; c++ )
			{
				// Submit the message
				batch.SubmitMessage( messages[c] );
			}

			// Commit the batch
			batch.Done( null );

			return ar;
		}

		override public bool EndSubmitMessages(
			IAsyncResult					ar)
		{
			if ( false == _initialized )
				throw new InvalidOperationException("BizTalkMessaging is not initialized.");

			EpmAsyncResult tpAr = (EpmAsyncResult)ar;
			TpBatchStatus tpStatus = tpAr.BatchCallback.GetStatus();
			if ( 0 <= tpStatus.status )
				return true;
			else 
				return false;
		}

#endregion // Batched Submition API's

#region Request-Response API's

		/// <summary>
		/// Submits a synchronous BizTalk message and returns a response
		/// </summary>
		/// <param name="message">IBaseMessage object for request message</param>
		/// <returns>IBaseMessage object for response message</returns>
		override public IBaseMessage SubmitSyncMessage(
			IBaseMessage					message)
		{
			IAsyncResult ar = BeginSubmitSyncMessage( message, null, null);
			return EndSubmitSyncMessage(ar);
		}

		override public IAsyncResult BeginSubmitSyncMessage(
			IBaseMessage					message, 
			AsyncCallback					cb, 
			Object							asyncState)
		{
			if ( false == _initialized )
			{
				InternalInitialize(null, message, null);
			}

			Guid ct = Guid.NewGuid();
			string correlationToken = ct.ToString();

			EpmAsyncResult ar = new EpmAsyncResult(cb, asyncState);
			ResponseCallback responseCallback = new ResponseCallback(ar.BatchCallback, _tp);
			ar.ResponseCallback = responseCallback;

			// Get a new Transport Proxy Batch
			IBTTransportBatch batch = _tp.GetBatch( ar.BatchCallback, null );

			// Submit the message
			batch.SubmitRequestMessage( message, correlationToken, true, new DateTime(0), responseCallback );

			// Commit the batch
			batch.Done( null );

			return ar;
		}

		override public IBaseMessage EndSubmitSyncMessage(
			IAsyncResult					ar)
		{
			if ( false == _initialized )
				throw new InvalidOperationException("BizTalkMessaging is not initialized.");

			EpmAsyncResult epmAr = (EpmAsyncResult)ar;
			bool res = epmAr.AsyncWaitHandle.WaitOne();
			if ( true == res )
				return epmAr.ResponseCallback.Response;
			else
				return null;
		}

#endregion // Request-Response API's

#region Message Creation API's

		/// <summary>
		/// Creates a IBaseMessage from string
		/// </summary>
		/// <param name="url">Address of receive location where this message will be submitted</param>
		/// <param name="data">Payload of the message</param>
		/// <returns>IBaseMessage object</returns>
		override public IBaseMessage CreateMessageFromString(string url, string data)
		{
			if ( false == _initialized )
			{
				InternalInitialize(null, url, null);
			}

			return MessageHelper.CreateMessage(_mf, url, data);
		}

		/// <summary>
		/// Create a IBaseMessage object from stream
		/// </summary>
		/// <param name="url">Address of receive location where this message will be submitted</param>
		/// <param name="charset">Charset of the data in stream</param>
		/// <param name="data">Message payload</param>
		/// <returns>IBaseMessage object</returns>
		override public IBaseMessage CreateMessageFromStream(string url, string charset, Stream data)
		{
			if ( false == _initialized )
			{
				InternalInitialize(null, url, null);
			}

			return MessageHelper.CreateMessage(_mf, url, charset, data);
		}

#endregion

#region Message Factory Propery

		/// <summary>
		/// Gets the message factory object
		/// </summary>
		public IBaseMessageFactory MessageFactory
		{
			get
			{
				if ( false == _initialized )
					throw new InvalidOperationException("BizTalkMessaging is not initialized.");

				return _mf;
			}
		}

#endregion // Message Factory 

	}
}
