namespace DriveTracker.Contracts.DriveStatusUpdating;

/// <summary>
/// Shows the exact status of the Exhaust Gas Recirculation (EGR) valve.
/// </summary>
/// <param name="PercentageOpen">0 means the EGR is completely closed.
/// 100 means the EGR is fully open</param>
public record EgrPosition(int PercentageOpen);
