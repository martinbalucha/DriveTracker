using DriveTracker.Infrastructure.Mqtt.MessageBroker;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DriveAnalyzer.Tracking.Mqtt;

public class MqttListenerService : BackgroundService
{
    private IMqttBrokerClient _mqttBrokerClient;
    private ILogger<MqttListenerService> _logger;

    public MqttListenerService(IMqttBrokerClient mqttBrokerClient,
        ILogger<MqttListenerService> logger)
    {
        _mqttBrokerClient = mqttBrokerClient;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {       
        try
        {
            await _mqttBrokerClient.ConnectAsync(stoppingToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during the processing of MQTT messages.");
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await _mqttBrokerClient.DisconnectAsync(cancellationToken);

        _mqttBrokerClient.Dispose();

        await base.StopAsync(cancellationToken);
    }
}
