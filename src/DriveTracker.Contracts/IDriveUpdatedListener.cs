namespace DriveTracker.Contracts;

public interface IDriveUpdatedListener
{
    Task SubscribeToDriveUpdatesAsync(VehicleId vehicleId, DriveId driveId, CancellationToken cancellationToken = default);

    Task UnsubscribeToDriveUpdatesAsync(VehicleId vehicleId, DriveId driveId, CancellationToken cancellationToken = default);
}
