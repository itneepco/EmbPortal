using Infrastructure.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Behaviors
{
    public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<TRequest> _logger;
        private readonly ICurrentUserService _currentUserService;

        public PerformanceBehavior(ILogger<TRequest> logger, ICurrentUserService currentUserService)
        {
            _timer = new Stopwatch();
            _logger = logger;
            _currentUserService = currentUserService;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _timer.Start();

            var response = await next();

            _timer.Stop();

            var elapsedMilliseconds = _timer.ElapsedMilliseconds;

            if (elapsedMilliseconds > 600)
            {
                var requestName = typeof(TRequest).Name;
                var userEmail = _currentUserService.Email ?? string.Empty;
                var userName = _currentUserService.DisplayName ?? string.Empty;

                _logger.LogWarning($"CleanArchitecture Long Running Request: {requestName} ({elapsedMilliseconds} milliseconds) by {userName}, {@userEmail} : {@request}");
            }

            return response;
        }
    }
}
