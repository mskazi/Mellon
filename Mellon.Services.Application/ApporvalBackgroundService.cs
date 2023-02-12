using MediatR;
using Mellon.Services.Application.Approvals;
using Mellon.Services.Infrastracture.Base;
using Mellon.Services.Infrastracture.Repositotiries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mellon.Services.Application
{
    public class ApporvalBackgroundServiceHost : BackgroundService
    {
        private readonly ILogger logger;
        private readonly IServiceScopeFactory factory;
        private bool isWaiting = true;
        private readonly IApprovalsRepository repository;
        private int ticker = 50000;
        public ApporvalBackgroundServiceHost(IServiceScopeFactory factory, IConfiguration configuration, ILogger<ApporvalBackgroundServiceHost> logger)
        {
            this.factory = factory;
            this.logger = logger;
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            Int32.TryParse(configuration["Endpoints:ticker"], out ticker);

        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken) => Task.Run(async () =>
        {
            await using AsyncServiceScope scope = factory.CreateAsyncScope();
            IApprovalProcessorHost engine = scope.ServiceProvider.GetRequiredService<IApprovalProcessorHost>();
            logger.LogInformation($"starting job execution every" + ticker);
            using var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(ticker));
            while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
            { 
                try
                {
                    if (isWaiting)
                    {
                        isWaiting = false;
                        var watch = Stopwatch.StartNew();
                        logger.LogInformation($"started job execution..");
                        //await processLock.WaitAsync();
                        await engine.TryGetApprovals(stoppingToken);
                        logger.LogInformation($"Finished job execution after " + watch.Elapsed);
                        watch.Stop();
                        // processLock.Release();
                        isWaiting = true;
                    }
                    else
                    {
                         logger.LogDebug($"Skipped job execution.");
                    }
                }
                catch (Exception ex)
                {
                   //  processLock.Release();
                    isWaiting = true;
                    logger.LogError(ex, $"Failure while background processing");
                }
            }
             logger.LogInformation($"Approval background service stop.");
        }, stoppingToken);

    }

   
   

        public interface IApprovalProcessorHost
    {
        Task TryGetApprovals(CancellationToken cancellationToken);
    }


    public class ApprovalProcessorHost : IApprovalProcessorHost
    {
        private readonly IMediator mediator;

        public ApprovalProcessorHost(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task TryGetApprovals(CancellationToken cancellationToken)
        {
            var command = new InsertERPApprovalsCommand();
            await mediator.Send(command);

        }
    }

}
