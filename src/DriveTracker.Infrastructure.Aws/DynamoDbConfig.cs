using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveTracker.Infrastructure.Aws;
public record DynamoDbConfig
{
    public required string DriveUpdateTableName { get; init; }
}
