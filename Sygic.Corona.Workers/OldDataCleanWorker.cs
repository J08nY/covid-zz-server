using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Sygic.Corona.Application.Commands;

namespace Sygic.Corona.Workers
{
    public class OldDataCleanWorker
    {
        private readonly IMediator mediator;

        public OldDataCleanWorker(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Singleton]
        [FunctionName("OldDataCleanWorker")]
        public async Task Run([TimerTrigger("%OldDataCleanCron%")]TimerInfo myTimer, ILogger log, CancellationToken cancellationToken)
        {
            var contactsCommand = new DeleteOldContactsCommand(TimeSpan.Parse(Environment.GetEnvironmentVariable("OldDataCleanInterval")));
            var locationsCommand = new DeleteOldLocationsCommand(TimeSpan.Parse(Environment.GetEnvironmentVariable("OldDataCleanInterval")));
            await mediator.Send(contactsCommand, cancellationToken);
            await mediator.Send(locationsCommand, cancellationToken);
        }
    }
}
