using DriveTracker.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;

namespace DriveTracker.Infrastructure.Mqtt.MessageBroker;

public class MqttBrokerClient : IMqttBrokerClient
{
    private readonly IMediator _mediator;
    private readonly MqttFactory _mqttFactory;
    private readonly IMqttClient _mqttClient;
    private readonly MqttConfiguration _config;
    private readonly ILogger<MqttBrokerClient> _logger;

    public MqttBrokerClient(IMediator mediator,
        MqttFactory mqttFactory,
        IOptions<MqttConfiguration> mqttConfiguration,
        ILogger<MqttBrokerClient> logger)
    {
        _mediator = mediator;
        _mqttFactory = mqttFactory;
        _config = mqttConfiguration.Value;
        _logger = logger;

        _mqttClient = mqttFactory.CreateMqttClient();

        //_mqttClient.ApplicationMessageReceivedAsync += ProcessMessageAsync;

        SetupClientLogging();
    }

    public async Task<MqttClientConnectResult> ConnectAsync(CancellationToken cancellationToken = default)
    {
        var mqttOptions = new MqttClientOptionsBuilder()
            .WithClientId(_config.ClientId)
            .WithTcpServer(_config.Host, _config.Port)
            .Build();

        var connectResult = await _mqttClient.ConnectAsync(mqttOptions, cancellationToken);

        if (connectResult.ResultCode != MqttClientConnectResultCode.Success)
        {
            //throw new 
        }

        var mqttSubscribeOptions = _mqttFactory.CreateSubscribeOptionsBuilder()
            .WithTopicFilter(_config.DriveInitiatedTopic)
            .Build();

        await _mqttClient.SubscribeAsync(mqttSubscribeOptions);

        return connectResult;
    }

    public async Task DisconnectAsync(CancellationToken cancellationToken = default)
    {
        var disconnectOptions = _mqttFactory.CreateClientDisconnectOptionsBuilder()
            .WithReason(MqttClientDisconnectOptionsReason.NormalDisconnection)
            .Build();

        await _mqttClient.DisconnectAsync(disconnectOptions, cancellationToken);
    }

    public void Dispose()
    {
        _mqttClient?.Dispose();
    }

    //private async Task ProcessMessageAsync(MqttApplicationMessageReceivedEventArgs e)
    //{
    //    if (e.ApplicationMessage.PayloadSegment.Array is null)
    //    {
    //        _logger.LogWarning("No payload included. Message is being discarded.");
    //        return;
    //    }

    //    string textMessage = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment.Array);

    //    IBaseRequest request;

    //    if (e.ApplicationMessage.Topic == _config.DriveInitiatedTopic)
    //    {
    //        request = JsonSerializer.Deserialize<StartDriveRequest>(textMessage)!;
    //    }
    //    else if (e.ApplicationMessage.Topic.StartsWith(_config.DriveUpdatesTopicPrefix))
    //    {
    //        request = JsonSerializer.Deserialize<DriveStatusUpdated>(textMessage)!;
    //    }
    //    else
    //    {
    //        _logger.LogWarning("Topic '{Topic}' is not supported.", e.ApplicationMessage.Topic);

    //        await PublishToDeadLetterQueueAsync(textMessage);

    //        return;
    //    }

    //    await _mediator.Send(request);
    //}

    private async Task PublishToDeadLetterQueueAsync(string invalidMessage, CancellationToken cancellationToken = default)
    {
        await _mqttClient.PublishStringAsync(_config.DeadLetterQueueTopic, invalidMessage,
            retain: true, cancellationToken: cancellationToken);
    }

    private void SetupClientLogging()
    {
        _mqttClient.ConnectedAsync += e =>
        {
            _logger.LogInformation("Connected to the MQTT broker.");

            return Task.CompletedTask;
        };

        _mqttClient.DisconnectedAsync += e =>
        {
            _logger.LogInformation("Disconnected from the MQTT broker.");

            return Task.CompletedTask;
        };
    }

    private string BuildDriveUpdatesTopicName(VehicleId vehicleId, DriveId driveId)
    {
        return $"{_config.DriveUpdatesTopicPrefix}/{vehicleId.Id}/{driveId.Id}";
    }
}
