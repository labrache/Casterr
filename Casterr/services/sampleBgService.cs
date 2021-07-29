using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR;

namespace Casterr.services
{
    public class sampleBgService : bgService
    {
        private readonly ILogger<sampleBgService> _logger;
        public sampleBgService(ILogger<sampleBgService> logger,
                IConfiguration configuration,
                IWebHostEnvironment env
                )
        {
            _logger = logger;
            
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"sampleBgService is starting.");

            stoppingToken.Register(() =>
                _logger.LogInformation($" sampleBgService is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"sampleBgService doing background work.");
                try
                {
                    _logger.LogInformation($"sampleBgService doing background work.");
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                }
                await Task.Delay(10000, stoppingToken);
            }

            _logger.LogInformation($"sampleBgService is stopping.");
        }
    }
}
