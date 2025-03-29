using System.Diagnostics.Tracing;

namespace Holtz.DotnetCounters.Api.EventSources
{
    [EventSource(Name = "Holtz.RequestProcessTimeEventSource")]
    public class RequestProcessTimeEventSource : EventSource
    {
        private EventCounter eventCounter;

        public RequestProcessTimeEventSource()
        {
            eventCounter = new EventCounter("holtz-req-count", this)
            {
                DisplayName = "[Holtz] Request Process Time",
                DisplayUnits = "ms"
            };
        }

        public void Log(string url, long elapsedTimeMs)
        {
            WriteEvent(1, url, elapsedTimeMs);
            eventCounter.WriteMetric(elapsedTimeMs);
        }

        protected override void Dispose(bool disposing)
        {
            eventCounter?.Dispose();
            base.Dispose(disposing);
        }
    }
}
