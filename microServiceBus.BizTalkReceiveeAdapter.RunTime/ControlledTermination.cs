using System;
using System.Threading;

namespace microServiceBus.BizTalkReceiveeAdapter.RunTime
{
    public class ControlledTermination : IDisposable
    {
        private AutoResetEvent e = new AutoResetEvent(false);
        private int activityCount = 0;
        private bool terminate = false;

        //  to be called at the start of the activity
        //  returns false if terminate has been called
        public bool Enter()
        {
            lock (this)
            {
                if (true == this.terminate)
                {
                    return false;
                }

                this.activityCount++;
            }
            return true;
        }

        //  to be called at the end of the activity
        public void Leave()
        {
            lock (this)
            {
                this.activityCount--;

                // Set the event only if Terminate() is called
                if (this.activityCount == 0 && this.terminate)
                    this.e.Set();
            }
        }

        //  this method blocks waiting for any activity to complete
        public void Terminate()
        {
            bool result;

            lock (this)
            {
                this.terminate = true;
                result = (this.activityCount == 0);
            }

            // If activity count was not zero, wait for pending activities
            if (!result)
            {
                this.e.WaitOne();
            }
        }

        public bool TerminateCalled
        {
            get
            {
                lock (this)
                {
                    return this.terminate;
                }
            }
        }

        public void Dispose()
        {
            ((IDisposable)this.e).Dispose();
        }
    }
}
