using System.Text.Json.Serialization;
using BashChelik;
using Newtonsoft.Json;

namespace BasCelik;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            try
            {
                var data = ElectronicIdReader.ReadAll();

                _logger.LogInformation(JsonConvert.SerializeObject(data));

                await Task.Delay(10000, stoppingToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
