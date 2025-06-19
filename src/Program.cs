using MessageWorker;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((context, services) =>
{
    services.AddHostedService<MessageSenderWorker>();
    services.AddLogging();
});

// Configure NServiceBus with Azure Service Bus transport
builder.UseNServiceBus(context =>
{
    var endpointConfiguration = new EndpointConfiguration("MessageWorker");
    endpointConfiguration.UseSerialization<SystemJsonSerializer>();
    // Set the connection string from configuration
    // string serviceBusEndpoint = context.Configuration.GetConnectionString("AzureServiceBus")!;
    // // Configure Azure Service Bus transport
    // var transport = new AzureServiceBusTransport(serviceBusEndpoint, TopicTopology.Default);
    // endpointConfiguration.UseTransport(transport);
    endpointConfiguration.UseTransport<LearningTransport>();
    // Configure error queue
    endpointConfiguration.SendFailedMessagesTo("error");

    // Enable installers to create queues
    endpointConfiguration.EnableInstallers();
    
    return endpointConfiguration;
});

var host = builder.Build();
host.Run();