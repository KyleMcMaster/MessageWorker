namespace MessageWorker;

public class MessageSenderWorker : BackgroundService
{
    private readonly ILogger<MessageSenderWorker> _logger;
    private readonly IMessageSession _messageSession;

    public MessageSenderWorker(ILogger<MessageSenderWorker> logger, IMessageSession messageSession)
    {
        _logger = logger;
        _messageSession = messageSession;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Guid id = Guid.NewGuid();
            _logger.LogInformation("Worker running at: {time}, sending message Id {id}", DateTimeOffset.UtcNow, id);
            var message = new MyMessage(id, "Hello, world!", DateTimeOffset.UtcNow);
            await _messageSession.SendLocal(message, stoppingToken);
            await Task.Delay(60000, stoppingToken);
        }
    }
}
