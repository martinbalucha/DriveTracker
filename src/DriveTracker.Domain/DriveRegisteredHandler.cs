using DriveTracker.Contracts;
using DriveTracker.Contracts.DriveRegistration;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DriveTracker.Domain;

public class DriveRegisteredHandler : IRequestHandler<DriveRegistered>
{
    private readonly IDriveUpdatedListener _driveUpdatedListener;
    private readonly ILogger<DriveRegisteredHandler> _logger;

    public DriveRegisteredHandler(IDriveUpdatedListener driveUpdatedListener, ILogger<DriveRegisteredHandler> logger)
    {
        _driveUpdatedListener = driveUpdatedListener;
        _logger = logger;
    }

    public async Task Handle(DriveRegistered request, CancellationToken cancellationToken)
    {
        try
        {
            await _driveUpdatedListener.SubscribeToDriveUpdatesAsync(request.VehicleId, request.DriveId, cancellationToken);

            _logger.LogInformation("Subscribed to the drive updates for for VehicleId='{VehicleId}', DriveId='{DriveId}'",
                request.VehicleId.Id, request.DriveId.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not subscribe to the topic for VehicleId='{VehicleId}', DriveId='{DriveId}'",
                request.VehicleId.Id, request.DriveId.Id);
        }
    }
}
