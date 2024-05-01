using Polly;
using Polly.Extensions.Http;

namespace PoC_CircuitBreaker
{
    public static class PolicyExtensions
    {
        public static IAsyncPolicy<HttpResponseMessage>  RetryPolicy => HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(1), onRetry: (exception, _, retry, _) =>
            {
                Console.WriteLine($"Tentativa {retry} após falha");
            });

        public static IAsyncPolicy<HttpResponseMessage> CircuitBreakerPolicy => HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(3, TimeSpan.FromSeconds(30),
                onBreak: (exception, timespan) =>
                {
                    Console.WriteLine($"Circuit breaker ativado por {timespan.TotalSeconds} segundos");
                },
                onReset: () =>
                {
                    Console.WriteLine("Circuit breaker resetado");
                },
                onHalfOpen: () =>
                {
                    Console.WriteLine("Circuit breaker mudou para half-open state");
                });

    }
}
