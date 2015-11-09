using System;
using System.Threading;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.TransportProxy.Interop;
using Microsoft.BizTalk.Message.Interop;


namespace microServiceBus.BizTalkReceiveeAdapter.RunTime
{
	/// <summary>
	/// Implements IBTBatchCallBack interface to receive a batch submission notification from the server
	/// </summary>
	sealed internal class TpBatchAsyncCallback : IBTBatchCallBack
	{
		private AutoResetEvent		_evt				= null;
		private TpBatchStatus 		_batchStatus		= null;
		private bool				_isCompleted		= false;
		private IAsyncResult		_ar					= null;
		private AsyncCallback		_asyncCallBack		= null;
		private ResponseCallback	_responseCallback	= null;


		public TpBatchAsyncCallback()
		{
			_evt = new AutoResetEvent(false);
		}

		/// <summary>
		/// Initialize batch callback instance
		/// </summary>
		/// <param name="ar">Asynch result</param>
		/// <param name="callBack">Asynch callback</param>
		public void Initialize(IAsyncResult ar, AsyncCallback callBack)
		{
			this._ar = ar;
			this._asyncCallBack = callBack;
		}

		/// <summary>
		/// Get a wait handle for callback thread
		/// </summary>
		public WaitHandle AsyncWaitHandle
		{
			get { return this._evt; }
		}

		public bool IsCompleted
		{
			get { return this._isCompleted; }
		}

		/// <summary>
		/// Get a response callback
		/// </summary>
		public ResponseCallback ResponseCallback
		{
			get { return this._responseCallback; }
			set { _responseCallback = value; }
		}

		/// <summary>
		/// Get batch submission status
		/// </summary>
		/// <returns>Batch submission status</returns>
		public TpBatchStatus GetStatus()
		{
			bool status = _evt.WaitOne();

			if ( true == status )
			{
				return _batchStatus;
			}
			else
			{
				// TODO: throw
			}

			return _batchStatus;
		}

		/// <summary>
		/// Process the results of batch submission
		/// </summary>
		/// <param name="status">Overall status for the batch</param>
		/// <param name="opCount">Count of batch operations</param>
		/// <param name="operationStatus">Status for each operation on the batch</param>
		/// <param name="callbackCookie">Callback cookie</param>
		public void BatchComplete(	int status, short opCount, BTBatchOperationStatus[] operationStatus, object callbackCookie)
		{
			try
			{
				_batchStatus = new TpBatchStatus(status, opCount, operationStatus, callbackCookie);

				_isCompleted = true;
				_evt.Set();

				// If client registered for a call back...
				if ( null != _asyncCallBack )
				{
					_asyncCallBack( _ar );
				}
			}
			finally
			{
				if ( null != _responseCallback )
					_responseCallback.SubmitComplete(_batchStatus);
			}
		}
	}
}
