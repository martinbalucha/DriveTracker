using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using DriveTracker.Contracts;
using DriveTracker.Contracts.DriveStatusUpdating;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace DriveTracker.Infrastructure.Aws;

public class DynamoDbRepository : IDriveStatusRepository
{
    private readonly IAmazonDynamoDB _dynamoDb;
    private readonly DynamoDbConfig _dynamoDbConfig;
    private readonly ILogger<DynamoDbRepository> _logger;

    public DynamoDbRepository(IAmazonDynamoDB dynamoDb,
        IOptions<DynamoDbConfig> dynamoDbConfig,
        ILogger<DynamoDbRepository> logger)
    {
        _dynamoDb = dynamoDb;
        _dynamoDbConfig = dynamoDbConfig.Value;
        _logger = logger;
    }

    public async Task CreateAsync(DriveStatus driveStatus, CancellationToken cancellationToken = default)
    {
      
        var putItemRequest = new PutItemRequest
        {
            TableName = _dynamoDbConfig.DriveUpdateTableName,
            Item = ConvertDriveUpdate(driveStatus)
        };

        try
        {
            var response = await _dynamoDb.PutItemAsync(putItemRequest, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error o");
        }
        
    }

    private Dictionary<string, AttributeValue> ConvertDriveUpdate(DriveStatus driveStatus)
    {
        string vehicleStatusJson = JsonSerializer.Serialize(driveStatus.VehicleStatus);

        return new Dictionary<string, AttributeValue>
        {
            { nameof(VehicleId), new AttributeValue(driveStatus.VehicleId.Id.ToString())},
            { nameof(DriveId), new AttributeValue(driveStatus.DriveId.Id.ToString()) },
            { nameof(VehicleStatus), new AttributeValue(vehicleStatusJson)}

        };
    }
}
