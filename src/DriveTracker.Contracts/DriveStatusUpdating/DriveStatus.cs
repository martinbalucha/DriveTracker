namespace DriveTracker.Contracts.DriveStatusUpdating;

public record DriveStatus(
    VehicleId VehicleId,
    DriveId DriveId,
    VehicleStatus VehicleStatus,
    GeoCoordinate Coordinates,
    DateTimeOffset RecordedAtIso8601);
