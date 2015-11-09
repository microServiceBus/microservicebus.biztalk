using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microServiceBus.BizTalkReceiveeAdapter.RunTime
{
    public class microServiceBusTransmitAdapter : AsyncTransmitter
    {
        public microServiceBusTransmitAdapter()
            : base(
			"microServiceBus Transmit Adapter",
			"1.0",
			"Send files from BizTalk to microservicebus.com",
            "microservicebus",
            new Guid("3EBA24E5-8904-4A41-BDF6-72ECEF00271E"),
            "microServiceBus.BizTalkReceiveeAdapter.RunTime",
			typeof(microServiceBusTransmitterEndpoint),
            1)
		{
        }
    }
}
