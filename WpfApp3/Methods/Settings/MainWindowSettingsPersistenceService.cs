using HaruaConvert.HaruaInterFace;
using HaruaConvert.Json;
using HaruaConvert.Parameter;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Controls;
using WpfApp3.Parameter;
using static HaruaConvert.IniCreate;
using static HaruaConvert.Parameter.ParamField;

namespace HaruaConvert.Methods.Settings
{
    public sealed class MainWindowSettingsPersistenceService : IMainWindowSettingsPersistenceService
    {
        public void Save(MainWindow main, bool isEdit, bool isChecked, IEnumerable<CheckBox> childCheckBoxList)
        {
            int i = 0;

            {
                // Add Number and Save setting.ini every selector
                foreach (var selector in main.selectorList)
                {
                    IniDefinition.SetValue(main.paramField.iniPath, ParamField.ControlField.ParamSelector + "_" + $"{i}", "Arguments_" + $"{i}",
                        selector.ArgumentEditor.Text);

                    IniDefinition.SetValue(main.paramField.iniPath, ParamField.ControlField.ParamSelector + "_" + $"{i}", IniSettingsConst.ParameterLabel + "_" + $"{i}",
                        selector.ParamLabel.Text);

                    ++i;

                    // if Check Selector Radio, Save Check State
                    if (selector.SlectorRadio.IsChecked == true)
                    {
                        var radioCount = selector.Name.Remove(0, ParamField.ControlField.ParamSelector.Length);
                        IniDefinition.SetValue(main.paramField.iniPath, ClassShearingMenbers.CheckState, ParamField.ControlField.ParamSelector + "_Check", radioCount);
                    }
                }
            }

            if (isChecked)
            {
                var checkedSet = new IniCheckBoxSetClass();
                if (childCheckBoxList != null)
                {
                    foreach (CheckBox chk in childCheckBoxList)
                    {
                        checkedSet.CheckediniSetVallue(chk, main.paramField.iniPath);
                    }
                }
            }

            var setWriter = new IniSettings_IOClass();
            setWriter.IniSettingWriter(main.paramField, main);

            if (!string.IsNullOrEmpty(main.ParamText.Text))
            {
                IniDefinition.SetValue(main.paramField.iniPath, QueryNames.ffmpegQuery, QueryNames.BaseQuery, main.ParamText.Text);
            }

            if (!string.IsNullOrEmpty(main.endStringBox.Text))
            {
                IniDefinition.SetValue(main.paramField.iniPath, QueryNames.ffmpegQuery, QueryNames.endStrings, main.endStringBox.Text);
            }

            if (!string.IsNullOrEmpty(main.placeHolderList.Text))
            {
                IniDefinition.SetValue(main.paramField.iniPath, QueryNames.placeHolder, QueryNames.placeHolderCount, main.placeHolderList.SelectedIndex.ToString(CultureInfo.CurrentCulture));
            }

            // Background image opacity
            IniDefinition.SetValue(main.paramField.iniPath, IniSettingsConst.Apperance, IniSettingsConst.BackImageOpacity, main.harua_View.MainParams[0].BackImageOpacity.ToString(CultureInfo.CurrentCulture));

            var ffjQuery = new CommandHistory();
            CommandHistoryIO qHistory = new();

            foreach (var item in main.ParamText.Items)
            {
                ffjQuery.ffQueryToken.Add(item.ToString());
            }

            string history = Path.Combine(AppContext.BaseDirectory, "CommandHistory.json");
            if (!File.Exists(history))
            {
                return;
            }

            qHistory.SaveToJsonFile(ffjQuery, "CommandHistory.json");
        }
    }
}
