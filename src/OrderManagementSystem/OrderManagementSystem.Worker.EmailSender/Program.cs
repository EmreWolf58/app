using OrderManagementSystem.Worker.EmailSender;
using MassTransit;
using MassTransit.RabbitMqTransport;
using OrderManagementSystem.Worker.EmailSender.Consumer;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderCreatedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbit", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        // Consumer için endpoint (kuyruk) tanýmý
        cfg.ReceiveEndpoint("order-created-email-queue", e =>
        {
            e.ConfigureConsumer<OrderCreatedConsumer>(context);

            // (Opsiyon) Retry: transient hata olursa tekrar dene
            e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
        });
    });
});

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
