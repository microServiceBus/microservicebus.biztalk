using System;
using System.IO;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.TransportProxy.Interop;
using Microsoft.BizTalk.Message.Interop;


namespace microServiceBus.BizTalkReceiveeAdapter.RunTime
{
	/// <summary>
	/// Base class for BizTalk messaging adapter
	/// </summary>
	public abstract class BizTalkMessagingBase 
	{
		/// <summary>
		/// Submit one-way message
		/// </summary>
		public abstract bool SubmitMessage(IBaseMessage message);
		public abstract IAsyncResult BeginSubmitMessage(IBaseMessage message, AsyncCallback cb, Object asyncState);
		public abstract bool EndSubmitMessage(IAsyncResult ar);

		/// <summary>
		/// Submit Batches of One-Way Messages
		/// </summary>
		public abstract bool SubmitMessages(IBaseMessage[] messages);
		public abstract IAsyncResult BeginSubmitMessages(IBaseMessage[] messages, AsyncCallback cb, Object asyncState);
		public abstract bool EndSubmitMessages(IAsyncResult ar);

		/// <summary>
		/// Submit Request-Response (synchronous message) Message Exchange Pattern
		/// </summary>
		public abstract IBaseMessage SubmitSyncMessage(IBaseMessage message);
		public abstract IAsyncResult BeginSubmitSyncMessage(IBaseMessage message, AsyncCallback cb, Object asyncState);
		public abstract IBaseMessage EndSubmitSyncMessage(IAsyncResult ar);

		/// <summary>
		/// Message Creation
		/// </summary>
		public abstract IBaseMessage CreateMessageFromString(string url, string data);
		public abstract IBaseMessage CreateMessageFromStream(string url, string charset, Stream data);
	}
}
