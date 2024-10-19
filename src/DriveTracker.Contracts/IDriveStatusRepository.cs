using DriveTracker.Contracts.DriveStatusUpdating;

namespace DriveTracker.Contracts;

/// <summary>
/// 
/// </summary>
public interface IDriveStatusRepository
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="driveStatusCancellationToken"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task CreateAsync(DriveStatus driveStatus, CancellationToken cancellationToken = default);
}
