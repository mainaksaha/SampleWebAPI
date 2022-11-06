using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.WindowsServer.Channel.Implementation;
using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;

namespace SampleWebAPI
{
    public class AdaptiveTelemetryProcessor : ITelemetryProcessor
    {
        private ITelemetryProcessor Next;

        public AdaptiveTelemetryProcessor(ITelemetryProcessor next)
        {
            Next = next;
        }

        public void Process(ITelemetry item)
        {
            if (item is ExceptionTelemetry)
                return;

            this.Next.Process(item);
        }
    }
}
