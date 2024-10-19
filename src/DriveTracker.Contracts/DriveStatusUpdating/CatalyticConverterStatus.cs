namespace DriveTracker.Contracts.DriveStatusUpdating;

public record CatalyticConverterStatus(
    decimal Bank1TemperatureInCelsius,
    decimal Bank2TemperatureInCelsius);
