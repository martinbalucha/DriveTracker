using DriveTracker.Contracts.DriveStatusUpdating;

namespace DriveTracker.Contracts;

/// <summary>
/// 
/// </summary>
public interface IDriveStatusRepository
{
    /// <summary>
    /// Stores a new drive status update.
    /// </summary>
    /// <param name="driveStatus">A new drive status update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns></returns>
    Task StoreAsync(DriveStatus driveStatus, CancellationToken cancellationToken = default);
}
