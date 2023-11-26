using HaruaConvert.Parameter;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaruaConvert.Methods
{
    public class IniSettings_IOClass
    {
        public void IniSettingWriter(ParamField paramField,MainWindow main)
        {




            //WIndow Size
            IniDefinition.SetValue(paramField.iniPath, "WindowsLocate", "WindowLeft", Convert.ToString(main.Left, CultureInfo.CurrentCulture));
            IniDefinition.SetValue(paramField.iniPath, "WindowsLocate", "WindowTop", Convert.ToString(main.Top, CultureInfo.CurrentCulture));

            //FileOpenDialog Init Path
            IniDefinition.SetValue(paramField.iniPath, "Directory", IniSettingsConst.ConvertDirectory, ParamField.Maintab_InputDirectory);
            IniDefinition.SetValue(paramField.iniPath, "Directory", IniSettingsConst.OutputDirectory, ParamField.MainTab_OutputDirectory);
            IniDefinition.SetValue(paramField.iniPath, "Directory", IniSettingsConst.OutputSelectorDirectory, ParamField.ParamTab_OutputSelectorDirectory);
            IniDefinition.SetValue(paramField.iniPath, "Directory", IniSettingsConst.InputSelectorDirectory, ParamField.ParamTab_InputSelectorDirectory);

            //Save Generated Number
            IniDefinition.SetValue(paramField.iniPath, IniSettingsConst.Selector_Generate, IniSettingsConst.Selector_Generate, main. NumericUpDown1.NUDTextBox.Text);



        }


        public void IniSettingReader(ParamField paramField, MainWindow main)
        {
            main.Left = IniDefinition.GetValueOrDefault(paramField.iniPath, "WindowsLocate", "WindowLeft", 25);

            main.Top = Convert.ToInt32(IniDefinition.GetValueOrDefault(paramField.iniPath, "WindowsLocate", "WindowTop", 50));

            ParamField.Maintab_InputDirectory = IniDefinition.GetValueOrDefault(paramField.iniPath, "Directory", IniSettingsConst.ConvertDirectory, "");
            // IniDefinition.SetValue(paramField.iniPath, "Directry", "ConvertDirectory", ParamField.ConvertDirectory);


            ParamField.MainTab_OutputDirectory = IniDefinition.GetValueOrDefault(paramField.iniPath, "Directory", IniSettingsConst.OutputDirectory, "");
            ParamField.ParamTab_OutputSelectorDirectory = IniDefinition.GetValueOrDefault(paramField.iniPath, "Directory", IniSettingsConst.OutputSelectorDirectory, "");
            ParamField.ParamTab_InputSelectorDirectory = IniDefinition.GetValueOrDefault(paramField.iniPath, "Directory", IniSettingsConst.InputSelectorDirectory, "");
            
            
            

            main.NumericUpDown1.NUDTextBox.Text = IniDefinition.GetValueOrDefault(paramField.iniPath, IniSettingsConst.Selector_Generate, IniSettingsConst.Selector_Generate, "1");


        }
    }
}
