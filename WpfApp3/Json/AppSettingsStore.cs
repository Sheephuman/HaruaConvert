using Newtonsoft.Json;
using System;
using System.IO;

namespace HaruaConvert.Json
{
    public sealed class AppSettingsStore
    {
        private readonly string _jsonPath;
        
        private AppSettings _cached;

        public AppSettingsStore(string jsonPath)
        {
            _jsonPath = jsonPath ?? throw new ArgumentNullException(nameof(jsonPath));
            
        }

        public string JsonPath => _jsonPath;

        public AppSettings Current
        {
            get
            {
                _cached ??= Load();
                return _cached;
            }
        }


        public AppSettings Load()
        {
            if (File.Exists(_jsonPath))
            {
                var json = File.ReadAllText(_jsonPath);
                _cached = JsonConvert.DeserializeObject<AppSettings>(json) ?? new AppSettings();
                return _cached;
            }

           
            _cached = new AppSettings();
            return _cached;
        }

        public void Save()
        {
            if (_cached != null)
            {
                Save(_cached);
            }
        }

        public void Save(AppSettings settings)
        {
            _cached = settings ?? throw new ArgumentNullException(nameof(settings));
            var directory = Path.GetDirectoryName(_jsonPath);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var json = JsonConvert.SerializeObject(_cached, Formatting.Indented);
            File.WriteAllText(_jsonPath, json);
        }
    }
}
