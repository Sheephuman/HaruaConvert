using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaruaConvert.Parameter;

namespace HaruaConvert.HaruaService
{
    public interface ISettingsService
    {
    
        string GetValueOrDefault(string section, string key, string defaultValue);
        string GetIniPath();
    }

    public class SettingsService : ISettingsService
    {
        private readonly string _iniPath;
        
        public SettingsService(string iniPath)
        {
            _iniPath =  iniPath ?? throw new ArgumentNullException(nameof(iniPath));


        }

        public string GetIniPath()
        {    
                        
           return _iniPath;
        }

        public string GetValueOrDefault(string section, string key, string defaultValue)
        {
            return IniDefinition.GetValueOrDefault(_iniPath, section, key, defaultValue);
        }
    }

}
