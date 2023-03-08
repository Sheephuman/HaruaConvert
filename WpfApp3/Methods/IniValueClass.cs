using HaruaConvert;
using System.Collections.Generic;
using System.Windows.Controls;
using HaruaConvert.Parameter;

namespace HaruaConvert.Methods
{
    internal class IniGetSetValueClass
    {
        //MainWindow main;
        //public IniValueClass(MainWindow _main) 
        //{ main = _main; }

      


       public class CheckboxGetSetValueClass
       {
        List<CheckBox> ChecBoxList;
        public List<CheckBox> AddCheckeBoxControls(CheckBox chk)
        {
            ChecBoxList = new List<CheckBox>();
            ChecBoxList.Add(chk);

            return ChecBoxList;
        }


#pragma warning disable CA1822 // メンバーを static に設定します
        public void CheckediniSetVallue(CheckBox chek, string iniPath)
#pragma warning restore CA1822 // メンバーを static に設定します
        {
            IniDefinition.SetValue(iniPath, "CheckState", chek.Name,
              chek.IsChecked.Value.ToString());

        }

#pragma warning disable CA1822 // メンバーを static に設定します
        public bool CheckBoxiniGetVallue(CheckBox chek, string iniPath)
#pragma warning restore CA1822 // メンバーを static に設定します
        {
            var setbool = IniDefinition.GetValueOrDefault(iniPath, "CheckState", chek.Name, false);
            return setbool;

        }


    }

    }
}
