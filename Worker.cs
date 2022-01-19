using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataExchangeWorkerService.Helpers;
using DataExchangeWorkerService.Services;

namespace DataExchangeWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly EtlProcessor _etlProcessor;

        public Worker(ILogger<Worker> logger, EtlProcessor etlProcessor)
        {
            _logger = logger;
            _etlProcessor = etlProcessor;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                await _etlProcessor.DoWork();

                // millisecond(1000) * second(60) * minute(10) = 10 minute
                await Task.Delay(1000 * 60 * 1, stoppingToken);
            }
        }
    }
}
