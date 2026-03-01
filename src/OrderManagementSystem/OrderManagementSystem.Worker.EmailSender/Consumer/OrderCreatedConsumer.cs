using System;
using System.Collections.Generic;
using System.Text;
using MassTransit;
using Microsoft.Extensions.Logging;
using OrderManagementSystem.Core.Events;

namespace OrderManagementSystem.Worker.EmailSender.Consumer
{
    public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly ILogger<OrderCreatedConsumer> _logger;

        public OrderCreatedConsumer(ILogger<OrderCreatedConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var msg = context.Message;

            // "Email simülasyonu"
            _logger.LogInformation("EMAIL_SIMULATION -> To:{Email} | OrderId:{OrderId} | Total:{Total} | CreatedAt:{At}", msg.CustomerEmail, msg.OrderId, msg.TotalAmount, msg.CreatedAtUtc);

            return Task.CompletedTask;
        }
    }
}
