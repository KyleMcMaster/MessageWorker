using MessageWorker;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureLogging((context, logging) =>
{
    logging.AddConfiguration(context.Configuration.GetSection("Logging"));
    logging.AddConsole();
});

builder.ConfigureServices((context, services) =>
{
    services.AddHostedService<MessageSenderWorker>();
    services.AddLogging();
});

builder.UseNServiceBus(context =>
{
    var endpointConfiguration = new EndpointConfiguration("MessageWorker");
    endpointConfiguration.UseSerialization<SystemJsonSerializer>();
    // Set the connection string from configuration
    string serviceBusEndpoint = context.Configuration["AzureServiceBusEndpoint"]!;
    // // Configure Azure Service Bus transport
    var transport = new AzureServiceBusTransport(serviceBusEndpoint, TopicTopology.Default);
    endpointConfiguration.UseTransport(transport);

    // Configure error queue
    endpointConfiguration.SendFailedMessagesTo("error");

    // Enable installers to create queues
    endpointConfiguration.EnableInstallers();
    endpointConfiguration.EnableOpenTelemetry();
    
    return endpointConfiguration;
});

var host = builder.Build();
host.Run();