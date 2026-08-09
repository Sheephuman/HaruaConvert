using HaruaConvert.Json;
using System.Windows.Controls;

namespace HaruaConvert.Methods
{
    internal sealed class CheckBoxSettingsPersistence
    {
        public void SaveCheckState<T>(T check, AppSettingsStore store)
        {
            var checkControl = check as Control;
            if (checkControl == null)
            {
                return;
            }

            var settings = store.Current;
            if (checkControl is CheckBox chk)
            {
                settings.CheckState[checkControl.Name] = chk.IsChecked == true;
            }
            else if (checkControl is MenuItem menuCheck)
            {
                settings.CheckState[checkControl.Name] = menuCheck.IsChecked == true;
            }

            store.Save(settings);
        }

        public bool LoadCheckState<T>(T check, AppSettingsStore store)
        {
            var checkControl = check as Control;
            if (checkControl == null)
            {
                return false;
            }

            var settings = store.Current;
            if (settings.CheckState.TryGetValue(checkControl.Name, out var value))
            {
                return value;
            }

            return false;
        }
    }
}
