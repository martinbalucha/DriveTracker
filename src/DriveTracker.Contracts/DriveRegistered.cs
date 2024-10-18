using MediatR;

namespace DriveTracker.Contracts;

public record DriveRegistered(VehicleId VehicleId, DriveId DriveId) : IRequest;
