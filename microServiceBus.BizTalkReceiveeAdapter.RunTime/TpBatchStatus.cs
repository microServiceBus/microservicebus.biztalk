using System;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.TransportProxy.Interop;
using Microsoft.BizTalk.Message.Interop;


namespace microServiceBus.BizTalkReceiveeAdapter.RunTime
{
	/// <summary>
	/// Class for storing the status of batch operations
	/// </summary>
	internal class TpBatchStatus
	{
		internal int						status			= 0;
		internal short						opCount			= 0;
		internal BTBatchOperationStatus[]	operationStatus	= null;
		internal object						callbackCookie	= null;				
			

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="status">Overall status of batch processing</param>
		/// <param name="opCount">Number of operations performed on the batch</param>
		/// <param name="operationStatus">Array of statuses for each opearation</param>
		/// <param name="callbackCookie">Callback cookie</param>
		public TpBatchStatus(	int status, 
								short opCount, 
								BTBatchOperationStatus[] operationStatus, 
								object callbackCookie )
		{
			this.status = status;
			this.opCount = opCount;
			this.operationStatus = operationStatus;
			this.callbackCookie = callbackCookie;
		}
	}
}
