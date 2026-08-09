using HaruaConvert.Json;
using HaruaConvert.Parameter;
using System;
using System.Globalization;
using System.Windows;

namespace HaruaConvert.Methods
{
    public class Settings_IOClass
    {
        private readonly AppearanceSettingsApplier _appearanceApplier = new();

        public void JsonSettingWriter(ParamField paramField, MainWindow main, AppSettings settings)
        {
            try
            {
                settings.Window.Left = main.Left;
                settings.Window.Top = main.Top;

                settings.Directories.Convert = ParamField.Maintab_InputDirectory;
                settings.Directories.Output = ParamField.MainTab_OutputDirectory;
                settings.Directories.OutputSelector = ParamField.ParamTab_OutputSelectorDirectory;
                settings.Directories.InputSelector = ParamField.ParamTab_InputSelectorDirectory;

                settings.SelectorGenerateCount = main.NumericUpDown1.NUDTextBox.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void JsonSettingReader(ParamField paramField, MainWindow main)
        {
            try
            {
                var settings = paramField.SettingsStore.Current;

                main.Left = settings.Window.Left;
                main.Top = settings.Window.Top;

                ParamField.Maintab_InputDirectory = settings.Directories.Convert;
                ParamField.MainTab_OutputDirectory = settings.Directories.Output;
                ParamField.ParamTab_OutputSelectorDirectory = settings.Directories.OutputSelector;
                ParamField.ParamTab_InputSelectorDirectory = settings.Directories.InputSelector;

                main.NumericUpDown1.NUDTextBox.Text = settings.SelectorGenerateCount;

                _appearanceApplier.ApplyToMainWindow(main, settings.Appearance);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
