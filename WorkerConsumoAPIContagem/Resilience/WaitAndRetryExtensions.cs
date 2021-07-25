using System;
using System.Collections.Generic;
using Polly;
using Polly.Retry;

namespace WorkerConsumoAPIContagem.Resilience
{
    public static class WaitAndRetryExtensions
    {
        public static AsyncRetryPolicy CreateWaitAndRetryPolicy(IEnumerable<TimeSpan> sleepsBeetweenRetries)
        {
            return Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    sleepDurations: sleepsBeetweenRetries,
                    onRetry: (_, span, retryCount, _) =>
                    {
                        var previousBackgroundColor = Console.BackgroundColor;
                        var previousForegroundColor = Console.ForegroundColor;
                        
                        Console.BackgroundColor = ConsoleColor.Yellow;
                        Console.ForegroundColor = ConsoleColor.Black;
                        
                        Console.Out.WriteLineAsync($" ***** {DateTime.Now:HH:mm:ss} | " +
                            $"Retentativa: {retryCount} | " +
                            $"Tempo de Espera em segundos: {span.TotalSeconds} **** ");
                        
                        Console.BackgroundColor = previousBackgroundColor;
                        Console.ForegroundColor = previousForegroundColor;
                    });
        }
    }
}