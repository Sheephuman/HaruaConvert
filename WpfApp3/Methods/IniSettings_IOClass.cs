using HaruaConvert.Parameter;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Resolvers;

namespace HaruaConvert.Methods
{
    public class IniSettings_IOClass
    {
        public void IniSettingWriter(StaticParamField paramField,MainWindow main)
        {
            try
            {



                //WIndow Size
                IniDefinition.SetValue(paramField.iniPath, "WindowsLocate", "WindowLeft", Convert.ToString(main.Left, CultureInfo.CurrentCulture));
                IniDefinition.SetValue(paramField.iniPath, "WindowsLocate", "WindowTop", Convert.ToString(main.Top, CultureInfo.CurrentCulture));

                //FileOpenDialog Init Path
                IniDefinition.SetValue(paramField.iniPath, "Directory", IniSettingsConst.ConvertDirectory, StaticParamField.Maintab_InputDirectory);
                IniDefinition.SetValue(paramField.iniPath, "Directory", IniSettingsConst.OutputDirectory, StaticParamField.MainTab_OutputDirectory);
                IniDefinition.SetValue(paramField.iniPath, "Directory", IniSettingsConst.OutputSelectorDirectory, StaticParamField.ParamTab_OutputSelectorDirectory);
                IniDefinition.SetValue(paramField.iniPath, "Directory", IniSettingsConst.InputSelectorDirectory, StaticParamField.ParamTab_InputSelectorDirectory);

                //Save Generated Number
                IniDefinition.SetValue(paramField.iniPath, IniSettingsConst.Selector_Generate, IniSettingsConst.Selector_Generate, main.NumericUpDown1.NUDTextBox.Text);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        public void IniSettingReader(StaticParamField paramField, MainWindow main)
        {

            try
            {
                main.Left = Convert.ToDouble(IniDefinition.GetValueOrDefault(paramField.iniPath, "WindowsLocate", "WindowLeft", 25));

                main.Top = Convert.ToDouble(IniDefinition.GetValueOrDefault(paramField.iniPath, "WindowsLocate", "WindowTop", 50));

                StaticParamField.Maintab_InputDirectory = IniDefinition.GetValueOrDefault(paramField.iniPath, "Directory", IniSettingsConst.ConvertDirectory, "");
                //IniDefinition.SetValue(paramField.iniPath, "Directry", "ConvertDirectory", ParamField.ConvertDirectory);


                StaticParamField.MainTab_OutputDirectory = IniDefinition.GetValueOrDefault(paramField.iniPath, "Directory", IniSettingsConst.OutputDirectory, "");
                StaticParamField.ParamTab_OutputSelectorDirectory = IniDefinition.GetValueOrDefault(paramField.iniPath, "Directory", IniSettingsConst.OutputSelectorDirectory, "");
                StaticParamField.ParamTab_InputSelectorDirectory = IniDefinition.GetValueOrDefault(paramField.iniPath, "Directory", IniSettingsConst.InputSelectorDirectory, "");




                main.NumericUpDown1.NUDTextBox.Text = IniDefinition.GetValueOrDefault(paramField.iniPath, IniSettingsConst.Selector_Generate, IniSettingsConst.Selector_Generate, "1");

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);

            
            }
        }
    }
}
