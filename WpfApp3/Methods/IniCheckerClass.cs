using System.Diagnostics;
using System.Windows.Controls;
using WpfApp3.Parameter;
using static HaruaConvert.IniCreate;

namespace HaruaConvert.Methods
{
    internal class IniCheckerClass
    {

        public void CheckediniSetVallue<T>(T check, string iniPath)

        {
            var checkControl = check as Control;

            if (checkControl is CheckBox chk)
            {
                string isChecked = chk.IsChecked.Value.ToString();

                IniDefinition.SetValue(iniPath, ClassShearingMenbers.CheckState, checkControl.Name, isChecked);
                ;

                Debug.WriteLineIf(true, isChecked);
            }
            else if (checkControl is MenuItem menucheck)
            {
                IniDefinition.SetValue(iniPath, ClassShearingMenbers.CheckState, checkControl.Name,
                menucheck.IsChecked.ToString());


                //  Debug.WriteLine(((MenuItem)checkControl).Name + $"{((MenuItem)checkControl).IsChecked}:Saved");


            }



        }


        public bool CheckBoxiniGetVallue<T>(T check, string iniPath)
        {
            var checkControl = check as Control;



            bool setbool = IniDefinition.GetValueOrDefault(iniPath, ClassShearingMenbers.CheckState, checkControl.Name, false);



            //    Debug.WriteLine(checkControl.Name + ":" + ((CheckBox)checkControl).IsChecked.Value.ToString());



            return setbool;

        }

    }


}
