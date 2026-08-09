using HaruaConvert.Parameter;
using System;
using System.Globalization;
using WpfApp3.Parameter;
using static HaruaConvert.IniCreate;
using static HaruaConvert.Parameter.ParamField;

namespace HaruaConvert.Json
{
    public sealed class IniSettingsMigrator
    {
        public IniSettingsMigrator(string legacyIniPath)
        {
         ;
        }



        public AppSettings Import(string iniPath)
        {
            var settings = new AppSettings
            {
                Window =
                {
                    Left = IniDefinition.GetValueOrDefault(iniPath, "WindowsLocate", "WindowLeft", 25d),
                    Top = IniDefinition.GetValueOrDefault(iniPath, "WindowsLocate", "WindowTop", 50d),
                },
                Directories =
                {
                    Convert = IniDefinition.GetValueOrDefault(iniPath, "Directory", IniSettingsConst.ConvertDirectory, string.Empty),
                    Output = IniDefinition.GetValueOrDefault(iniPath, "Directory", IniSettingsConst.OutputDirectory, string.Empty),
                    OutputSelector = IniDefinition.GetValueOrDefault(iniPath, "Directory", IniSettingsConst.OutputSelectorDirectory, string.Empty),
                    InputSelector = IniDefinition.GetValueOrDefault(iniPath, "Directory", IniSettingsConst.InputSelectorDirectory, string.Empty),
                },
                SelectorGenerateCount = IniDefinition.GetValueOrDefault(iniPath, IniSettingsConst.Selector_Generate, IniSettingsConst.Selector_Generate, "1"),
                Appearance =
                {
                    BackImageOpacity = double.Parse(
                        IniDefinition.GetValueOrDefault(iniPath, IniSettingsConst.Apperance, IniSettingsConst.BackImageOpacity, "1"),
                        CultureInfo.CurrentCulture),
                    BackgroundColor = new RgbColor
                    {
                        Red = ReadColorComponent(iniPath, "MainUIBackGrounColor_Red"),
                        Green = ReadColorComponent(iniPath, "MainUIBackGrounColor_Green"),
                        Blue = ReadColorComponent(iniPath, "MainUIBackGrounColor_Blue"),
                    },
                },
                FfmpegQuery =
                {
                    BaseQuery = IniDefinition.GetValueOrDefault(iniPath, QueryNames.ffmpegQuery, QueryNames.BaseQuery, ClassShearingMenbers.defaultQuery),
                    EndStrings = IniDefinition.GetValueOrDefault(iniPath, QueryNames.ffmpegQuery, QueryNames.endStrings, "_Harua"),
                },
                PlaceHolderIndex = int.Parse(
                    IniDefinition.GetValueOrDefault(iniPath, QueryNames.placeHolder, QueryNames.placeHolderCount, "0"),
                    CultureInfo.CurrentCulture),
                SelectedParamSelectorIndex = IniDefinition.GetValueOrDefault(
                    iniPath,
                    ClassShearingMenbers.CheckState,
                    ControlField.ParamSelector + "_Check",
                    "0"),
            };

            ImportSelectors(iniPath, settings);
            settings.CheckState["NoAudio"] = IniDefinition.GetValueOrDefault(iniPath, ClassShearingMenbers.CheckState, "NoAudio", false);
            return settings;
        }

        private double ReadColorComponent(string iniPath, string keyName)
        {
            return double.Parse(
                IniDefinition.GetValueOrDefault(iniPath, IniSettingsConst.Apperance, keyName, "0"),
                CultureInfo.CurrentCulture);
        }

        private void ImportSelectors(string iniPath, AppSettings settings)
        {
            var accessor = new AppSettingsSelectorAccessor();
            for (var index = 0; index < 32; index++)
            {
                var section = ControlField.ParamSelector + "_" + index;
                var arguments = IniDefinition.GetValueOrDefault(iniPath, section, IniSettingsConst.Arguments_ + index, string.Empty);
                var label = IniDefinition.GetValueOrDefault(iniPath, section, IniSettingsConst.ParameterLabel + "_" + index, string.Empty);
                if (string.IsNullOrEmpty(arguments) && string.IsNullOrEmpty(label))
                {
                    if (settings.Selectors.Count == 0)
                    {
                        continue;
                    }

                    break;
                }

                var selector = accessor.GetOrCreate(settings, index);
                selector.Arguments = arguments;
                selector.ParameterLabel = string.IsNullOrEmpty(label) ? "パラメータ名" : label;
            }
        }
    }
}
