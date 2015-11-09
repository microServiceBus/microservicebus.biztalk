
using System;
using System.Threading;
using Microsoft.BizTalk.TransportProxy.Interop;

namespace microServiceBus.BizTalkReceiveeAdapter.RunTime
{
    internal class DeleteBatchCallback : IBTBatchCallBack
    {
        private readonly AutoResetEvent _resetEvent;

        public DeleteBatchCallback(AutoResetEvent resetEvent)
        {
            if(resetEvent == null)
            {
                throw new ArgumentNullException("resetEvent");
            }

            _resetEvent = resetEvent;
        }

        #region IBTBatchCallBack Members

        public void BatchComplete(int status, short opCount, BTBatchOperationStatus[] operationStatus, object callbackCookie)
        {
            //Ignoring BatchComplete for sample adapter. Ideally, the logic for status check and acting correspondingly should be present.
            _resetEvent.Set();
        }

        #endregion
    }
}