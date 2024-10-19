using MQTTnet.Client;

namespace DriveTracker.Infrastructure.Mqtt.MessageBroker;

public interface IMqttBrokerClient : IDisposable
{
    Task<MqttClientConnectResult> ConnectAsync(CancellationToken cancellationToken = default);

    Task DisconnectAsync(CancellationToken cancellationToken = default);
}
