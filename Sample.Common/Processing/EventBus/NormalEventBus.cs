using Sample.Common.CQRS.DomainEvents;
using Sample.Common.CQRS.EventBus;
using Sample.Common.Domain.DBProviders;
using Sample.Common.Processing.Providers;
using MediatR;
using Newtonsoft.Json;
using RestSharp;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Common.Processing.EventBus
{
    public class NormalEventBus : IEventBus
    {
        private readonly IConfigProvider _configProvider;
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private List<Type> _integrateEvents;

        public List<Type> IntegrateEvents => _integrateEvents ??= typeof(NormalEventBus).Assembly
            .GetTypes()
            .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(typeof(DomainEventBase)))
            .ToList();

        public NormalEventBus(IConfigProvider configProvider, ILogger logger, IMediator mediator)
        {
            _configProvider = configProvider;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task Publish<TDomainEvent>(TDomainEvent @event) where TDomainEvent : IntegrationEventBase
        {
            var hosts = await _configProvider.GetsAsync<HostEvent>(ConfigKeys.ServicesHosts);
            if (hosts == null || hosts.Count < 1) return;

            foreach (var host in hosts)
            {
                try
                {
                    var client = new RestClient(host.Host);
                    //client.UseNewtonsoftJson();

                    var request = new RestRequest(host.Url).AddParameter("event", typeof(TDomainEvent).Name).AddJsonBody(host);
                    var response = await client.PostAsync(request);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, $"Publish event fail: {JsonConvert.SerializeObject(new { host, @event })}");
                }
            }
        }

        public async Task Execute(string eventName, string dataJson)
        {
            try
            {
                var types = IntegrateEvents.Where(x => x.Name == eventName).ToList();
                if (types.Count > 1)
                    _logger.Warning($"Integration Event name {eventName} have two or more. Event found: {String.Join(",", types.Select(x => x.FullName))}");
                var type = types.FirstOrDefault();
                if (type == null)
                    return;
                dynamic domainEvent = JsonConvert.DeserializeObject(dataJson, type);

                await _mediator.Publish(domainEvent);
            }
            catch (Exception ex)
            {
                _logger.Error($"{eventName} - data {dataJson}", ex);
                throw;
            }
        }
    }
}
