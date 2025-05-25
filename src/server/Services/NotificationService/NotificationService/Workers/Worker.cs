using Microsoft.Extensions.Hosting;

namespace NotificationService.Workers
{
    public class Worker : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Background worker started...");

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }

            Console.WriteLine("Background worker stopping...");
        }
    }
}
