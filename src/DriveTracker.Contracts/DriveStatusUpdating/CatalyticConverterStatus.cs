namespace DriveTracker.Contracts.DriveStatusUpdating;

/// <summary>
/// Status of the catalytic converter
/// </summary>
/// <param name="Bank1TemperatureInCelsius">Temperature in the catalytic converter for bank 1.</param>
/// <param name="Bank2TemperatureInCelsius">Temperature in the catalytic converter for bank 2. Is nullable
/// and default set to null because of the prevalence of inline cylinder distribution - bank 2 is not present.</param>
public record CatalyticConverterStatus(
    decimal Bank1TemperatureInCelsius,
    decimal? Bank2TemperatureInCelsius = null);
