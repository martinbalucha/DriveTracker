using DriveTracker.Contracts.DriveStatusUpdating;
using MediatR;

namespace DriveTracker.Contracts;

public record DriveStatusUpdated(DriveStatus Status) : IRequest;