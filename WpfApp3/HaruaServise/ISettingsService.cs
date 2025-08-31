using System;

namespace HaruaConvert.HaruaService
{
    public interface ISettingsService
    {

        string GetIniPath();
    }

    public class SettingsService : ISettingsService
    {
        private readonly string _iniPath;

        public SettingsService(string iniPath)
        {


            _iniPath = iniPath;

            _iniPath = iniPath ?? throw new ArgumentNullException(nameof(iniPath));


        }

        public string GetIniPath()
        {
            return _iniPath;
        }


    }

}
