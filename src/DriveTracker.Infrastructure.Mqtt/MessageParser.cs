using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace DriveAnalyzer.Tracking.Mqtt;

public class MessageParser : IMessageParser
{
    private ILogger<MessageParser> _logger;

    public MessageParser(ILogger<MessageParser> logger)
    {
        _logger = logger;
    }

    public bool TryExtractMessageType(JsonNode node, out Type? messageType)
    {
        messageType = null;

        string? messageTypeName = node["MessageType"]?.GetValue<string>();

        if (messageTypeName is null)
        {
            _logger.LogWarning("There is no 'MessageType' node in the request.");

            return false;
        }

        messageType = Type.GetType($"DriveAnalyzer.Contracts.Tracking.{messageTypeName}");

        return messageType is not null;
    }

    public bool TryExtractMessageBody(JsonNode node, Type messageType, out object? messageBody)
    {
        messageBody = null;

        var bodyJson = node["Body"];

        if (bodyJson is null)
        {
            _logger.LogWarning("There is no 'Body' node in the request.");

            return false;
        }

        messageBody = bodyJson.Deserialize(messageType);

        return messageBody is not null;
    }
}
