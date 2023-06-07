using NLog;

namespace Holtz.CQRS.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public ExceptionMiddleware(RequestDelegate next)
        {
            _requestDelegate = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _requestDelegate(httpContext);
            } catch (Exception ex)
            {
                _logger.Error(ex, "ExceptionMiddleware Error.");
            }
        }
    }
}
