namespace Months18.Services;

public interface ISettingsService
{
    Task<T> GetValue<T>(string key, T defaultValue);
    Task SetValue<T>(string key, T value);
}
