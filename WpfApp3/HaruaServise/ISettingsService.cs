using HaruaConvert.Json;
using System;

namespace HaruaConvert.HaruaService
{
    public interface ISettingsService
    {
        string GetSettingsPath();
        AppSettingsStore Store { get; }
    }

    public class SettingsService : ISettingsService
    {
        public SettingsService(AppSettingsStore store)
        {
            Store = store ?? throw new ArgumentNullException(nameof(store));
            _settingsPath = store.JsonPath;
        }

        private readonly string _settingsPath;

        public AppSettingsStore Store { get; }

        public string GetSettingsPath()
        {
            return _settingsPath;
        }
    }
}
