namespace DriveTracker.Infrastructure.Mqtt.MessageBroker;

public record MqttConfiguration
{
    public required string Host { get; init; }
    public int Port { get; init; }
    public string ClientId { get; init; } = string.Empty;
    public required string DriveRegisteredTopic { get; init; }
    public required string DriveUpdatesTopicPrefix { get; init; }
    public required string DeadLetterQueueTopic { get; init; }
}
