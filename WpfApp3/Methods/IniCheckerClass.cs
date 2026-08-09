using HaruaConvert.Json;
using System.Windows.Controls;

namespace HaruaConvert.Methods
{
    internal class IniCheckBoxSetClass
    {
        private readonly CheckBoxSettingsPersistence _persistence = new();

        public void CheckediniSetVallue<T>(T check, AppSettingsStore store)
        {
            _persistence.SaveCheckState(check, store);
        }

        public bool CheckBoxiniGetVallue<T>(T check, AppSettingsStore store)
        {
            return _persistence.LoadCheckState(check, store);
        }
    }
}
