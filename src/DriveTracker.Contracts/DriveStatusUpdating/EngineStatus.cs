namespace DriveTracker.Contracts.DriveStatusUpdating;

public record EngineStatus(
    decimal EngineLoadPercentage,
    decimal ManifoldAbsolutePressure,
    decimal FuelPressure,
    decimal OilTemperatureInCelsius, 
    decimal CoolantTemperatureInCelsius,
    decimal IntakeAirTemperatureInCelsius,
    CatalyticConverterStatus CatalyticConverterStatus,
    EgrPosition? EgrPosition = null);
