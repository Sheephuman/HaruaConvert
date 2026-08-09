using HaruaConvert.HaruaInterFace;
using HaruaConvert.Json;
using HaruaConvert.Parameter;
using System;
using System.IO;
using System.Windows.Controls;
using static HaruaConvert.Parameter.ParamField;

namespace HaruaConvert.Methods.Settings
{
    public sealed class MainWindowUiDataLoaderService : IMainWindowUiDataLoaderService
    {
        public bool ApplySelectorInitialValues(MainWindow main, bool firstSet)
        {
            var index = 0;
            if (!firstSet)
            {
                return firstSet;
            }

            var settings = main.paramField.SettingsStore.Current;
            foreach (var selector in main.selectorList)
            {
                var selectorSettings = index < settings.Selectors.Count
                    ? settings.Selectors[index]
                    : new ParamSelectorSettings();

                selector.ArgumentEditor.Text = selectorSettings.Arguments;
                selector.ParamLabel.Text = selectorSettings.ParameterLabel.Replace("\r\n", string.Empty, StringComparison.Ordinal);

                var selectedIndex = settings.SelectedParamSelectorIndex;
                if (selector.Name == ControlField.ParamSelector + selectedIndex)
                {
                    selector.SlectorRadio.IsChecked = true;
                    main.paramField.usedOriginalArgument = selector.ArgumentEditor.Text;
                }

                index++;
            }

            return false;
        }

        public void LoadCommandHistoryItems(ComboBox paramText)
        {
            var jsonreader = new QuerySaver();
            var history = Path.Combine(AppContext.BaseDirectory, "CommandHistory.json");
            if (!File.Exists(history))
            {
                return;
            }

            var tokenList = jsonreader.ReadtoJsonFile<string>(history);
            foreach (string token in tokenList)
            {
                if (!paramText.Items.Contains(token))
                {
                    paramText.Items.Add(token);
                }
            }
        }

        public int LoadPlaceholderIndex(AppSettingsStore store)
        {
            return store.Current.PlaceHolderIndex;
        }
    }
}
