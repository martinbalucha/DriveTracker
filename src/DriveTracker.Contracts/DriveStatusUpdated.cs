using MediatR;

namespace DriveTracker.Contracts;

public record DriveStatusUpdated(VehicleId VehicleId, GeoCoordinate Coordinate) : IRequest;