
namespace MessageWorker;

public class MyMessageHandler : IHandleMessages<MyMessage>
{
    private readonly ILogger<MyMessageHandler> _logger;

    public MyMessageHandler(ILogger<MyMessageHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        _logger.LogInformation("Context Message Id {messageId}, received message: {Message}", context.MessageId, message);
        return Task.CompletedTask;
    }
}
