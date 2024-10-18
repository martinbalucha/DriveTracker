using MediatR;

namespace DriveTracker.Contracts.DriveRegistration;

public record DriveRegistered(VehicleId VehicleId, DriveId DriveId) : IRequest;
