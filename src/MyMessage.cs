
namespace MessageWorker;

public record MyMessage(Guid Id, string Text, DateTimeOffset Timestamp);
