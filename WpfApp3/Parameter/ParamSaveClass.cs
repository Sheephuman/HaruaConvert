using HaruaConvert.ini関連;
using HaruaConvert.Methods;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static HaruaConvert.Parameter.ParamField;

namespace HaruaConvert.Parameter
{
    internal class ParamSaveClass
    {
        MainWindow main;
        public ParamSaveClass(MainWindow _main)
        {
            main = _main;

        }

        public void ParamSave_Procedure(ParamField paramField)
        {

            int i = 0;

            //Add Number and Save setting.ini evey selector 
            foreach (var selector in main.selectorList)
            {

                IniDefinition.SetValue(paramField.iniPath, ParamField.ControlField.ParamSelector + "_" + $"{i}", "Arguments_" + $"{i}",
                    selector.ArgumentEditor.Text);

                IniDefinition.SetValue(paramField.iniPath, ParamField.ControlField.ParamSelector + "_" + $"{i}", IniSettingsConst.ParameterLabel + "_" + $"{i}",
                    selector.ParamLabel.Text);

                i++;



                //if Check Selector Radio, Save Check State
                if (selector.SlectorRadio.IsChecked.Value)
                {
                    var radioCount = selector.Name.Remove(0, ParamField.ControlField.ParamSelector.Length);
                    IniDefinition.SetValue(paramField.iniPath, "CheckState", ParamField.ControlField.ParamSelector + "_Check", radioCount);

                }
            }


            {



                var checkedSet = new CheckBoxIniClass.CheckboxGetSetValueClass();

                if (main.childCheckBoxList != null)
                    foreach (CheckBox chk in main.childCheckBoxList)
                    {

                        checkedSet.CheckediniSetVallue(chk, paramField.iniPath);
                    }


                var setWriter = new IniSettings_IOClass();
                setWriter.IniSettingWriter(paramField, main);

                if (!string.IsNullOrEmpty(main.ParamText.Text))
                    IniDefinition.SetValue(paramField.iniPath, QueryNames.ffmpegQuery, QueryNames.BaseQuery, main.ParamText.Text);

                if (!string.IsNullOrEmpty(main. endStringBox.Text))
                    IniDefinition.SetValue(paramField.iniPath, QueryNames.ffmpegQuery, QueryNames.endStrings, main.endStringBox.Text);

                if (!string.IsNullOrEmpty(main.placeHolderList.Text))
                    IniDefinition.SetValue(paramField.iniPath, QueryNames.placeHolder, QueryNames.placeHolderCount, main.placeHolderList.SelectedIndex.ToString(CultureInfo.CurrentCulture));


                IniDefinition.SetValue(paramField.iniPath, QueryNames.userControl, QueryNames.opacitySlider, main.harua_View.MainParams[0].BackImageOpacity.ToString("F1", CultureInfo.CurrentCulture));

            }
        }

    }
}
