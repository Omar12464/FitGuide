using System.Diagnostics;

namespace FitGuide.MiddleWares
{
    public class ProfileTimerMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ProfileTimerMiddleWare> _logger;

        public ProfileTimerMiddleWare(RequestDelegate next, ILogger<ProfileTimerMiddleWare> logger)
        {
            _next = next;
            _logger = logger;
        }


        public async Task Invoke(HttpContext context)
        {
            var watch=new Stopwatch();
            watch.Start();
            await _next(context);
            watch.Stop();
            _logger.LogInformation($"Request {context.Request.Path} took {watch.ElapsedMilliseconds} ms to execute");



        }

    }
}
