using HaruaConvert.HaruaInterFace;
using HaruaConvert.Json;
using HaruaConvert.Parameter;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Controls;
using WpfApp3.Parameter;
using static HaruaConvert.IniCreate;
using static HaruaConvert.Parameter.ParamField;

namespace HaruaConvert.Methods.Settings
{
    public sealed class MainWindowUiDataLoaderService : IMainWindowUiDataLoaderService
    {
        public bool ApplySelectorInitialValues(MainWindow main, bool firstSet)
        {
            int i = 0;
            if (!firstSet)
            {
                return firstSet;
            }

            foreach (var selector in main.selectorList)
            {
                selector.ArgumentEditor.Text = IniDefinition.GetValueOrDefault(
                    main.paramField.iniPath,
                    ParamField.ControlField.ParamSelector + "_" + $"{i}",
                    IniSettingsConst.Arguments_ + $"{i}",
                    string.Empty);

                selector.ParamLabel.Text = IniDefinition.GetValueOrDefault(
                    main.paramField.iniPath,
                    ParamField.ControlField.ParamSelector + "_" + $"{i}",
                    IniSettingsConst.ParameterLabel + "_" + $"{i}",
                    "パラメータ名").Replace("\r\n", string.Empty, StringComparison.Ordinal);

                string rcount = IniDefinition.GetValueOrDefault(
                    main.paramField.iniPath,
                    ClassShearingMenbers.CheckState,
                    ParamField.ControlField.ParamSelector + "_Check",
                    "0");

                if (selector.Name == ParamField.ControlField.ParamSelector + rcount)
                {
                    selector.SlectorRadio.IsChecked = true;
                    main.paramField.usedOriginalArgument = selector.ArgumentEditor.Text;
                }

                i++;
            }

            return false;
        }

        public void LoadCommandHistoryItems(ComboBox paramText)
        {
            var jsonreader = new CommandHistoryIO();
            string history = Path.Combine(AppContext.BaseDirectory, "CommandHistory.json");
            if (!File.Exists(history))
            {
                return;
            }

            var tokenList = jsonreader.ReadtoJsonFile<string>("CommandHistory.json");
            foreach (string token in tokenList)
            {
                if (!paramText.Items.Contains(token))
                {
                    paramText.Items.Add(token);
                }
            }
        }

        public int LoadPlaceholderIndex(string iniPath)
        {
            return int.Parse(
                IniDefinition.GetValueOrDefault(iniPath, QueryNames.placeHolder, QueryNames.placeHolderCount, "0"),
                CultureInfo.CurrentCulture);
        }
    }
}
