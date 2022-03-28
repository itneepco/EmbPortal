using Infrastructure.Interfaces;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Behaviors
{
    public class LoggingBehavior<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly ILogger<TRequest> _logger;
        private readonly ICurrentUserService _currentUserService;

        public LoggingBehavior(ILogger<TRequest> logger, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _currentUserService = currentUserService;
        }

        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var userEmail = _currentUserService.Email ?? string.Empty;
            var userName = _currentUserService.DisplayName ?? string.Empty;

            _logger.LogInformation($"CleanArchitecture Request: {requestName} by {userName}, {@userEmail} : {@request}");

            return Task.CompletedTask;
        }
    }
}
