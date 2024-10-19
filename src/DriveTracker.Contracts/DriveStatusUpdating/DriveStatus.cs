namespace DriveTracker.Contracts.DriveStatusUpdating;

public record DriveStatus(
    VehicleId VehicleId,
    DriveId DriveId,
    VehicleStatus VehicleStatus,
    GeoCoordinate Coordinate,
    DateTimeOffset RecordedAt);
