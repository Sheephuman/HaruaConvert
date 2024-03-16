using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaruaConvert.Parameter;

namespace HaruaConvert.HaruaServise
{
    public interface ISettingsService
    {
        string GetIniPath();
        string GetValueOrDefault(string section, string key, string defaultValue);
    }

    public class SettingsService : ISettingsService
    {
        private readonly string _iniPath;
        ParamField param { get; set; }
        public SettingsService(string iniPath)
        {
            _iniPath =  iniPath ?? throw new ArgumentNullException(nameof(iniPath));


        }

        public string GetIniPath()
        {
      
            //_iniPath = param.iniPath;

            return _iniPath;
        }

        public string GetValueOrDefault(string section, string key, string defaultValue)
        {
            return IniDefinition.GetValueOrDefault(_iniPath, section, key, defaultValue);
        }
    }

}
