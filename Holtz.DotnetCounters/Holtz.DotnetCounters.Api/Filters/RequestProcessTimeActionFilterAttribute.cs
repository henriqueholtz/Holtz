using Holtz.DotnetCounters.Api.EventSources;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace Holtz.DotnetCounters.Api.Filters
{
    public class RequestProcessTimeActionFilterAttribute : ActionFilterAttribute
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private readonly RequestProcessTimeEventSource _eventSource;

        public RequestProcessTimeActionFilterAttribute(RequestProcessTimeEventSource eventSource)
        {
            _eventSource = eventSource;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _stopwatch.Start();
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            _stopwatch.Stop();

            _eventSource.Log(context.HttpContext.Request.GetDisplayUrl(), _stopwatch.ElapsedMilliseconds);
        }
    }
}
