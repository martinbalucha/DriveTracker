
using System.Text.Json.Nodes;

namespace DriveAnalyzer.Tracking.Mqtt;

public interface IMessageParser
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="node"></param>
    /// <param name="messageType"></param>
    /// <returns></returns>
    bool TryExtractMessageType(JsonNode node, out Type? messageType);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="node"></param>
    /// <param name="messageBody"></param>
    /// <returns></returns>
    bool TryExtractMessageBody(JsonNode node, Type bodyType, out object? messageBody);
}