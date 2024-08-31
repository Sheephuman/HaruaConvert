using HaruaConvert;
using System.Collections.Generic;
using System.Windows.Controls;
using HaruaConvert.Parameter;
using System.Windows.Controls.Primitives;

namespace HaruaConvert.Methods
{
    internal class IniCheckerClass
    {
        


       public class CheckboxGetSetValueClass
       {
        



        public void CheckediniSetVallue<T>(T check, string iniPath)

        {
             var checkControl = check as Control;

                if(checkControl is CheckBox)
                IniDefinition.SetValue(iniPath, "CheckState", checkControl.Name,
                  ((CheckBox)checkControl).IsChecked.Value.ToString());

                else if(checkControl is MenuItem)
                    IniDefinition.SetValue(iniPath, "CheckState", checkControl.Name,
                  ((MenuItem)checkControl).IsChecked.ToString());


            }


            public bool CheckBoxiniGetVallue<T>(T check, string iniPath)
        {
                var checkControl = check as Control;

                var setbool = IniDefinition.GetValueOrDefault(iniPath, "CheckState", checkControl.Name, false);
            return setbool;

        }




    }

    }
}
