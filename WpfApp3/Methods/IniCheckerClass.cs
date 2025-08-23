using System.Windows.Controls;

namespace HaruaConvert.Methods
{
    internal class IniCheckerClass
    {



        public class CheckboxGetSetValueClass
        {




            public void CheckediniSetVallue<T>(T check, string iniPath)

            {
                var checkControl = check as Control;

                if (checkControl is CheckBox)
                    IniDefinition.SetValue(iniPath, "CheckState", checkControl.Name,
                      ((CheckBox)checkControl).IsChecked.Value.ToString());

                else if (checkControl is MenuItem)
                {
                    IniDefinition.SetValue(iniPath, "CheckState", checkControl.Name,
                    ((MenuItem)checkControl).IsChecked.ToString());
                    //  Debug.WriteLine(((MenuItem)checkControl).Name + $"{((MenuItem)checkControl).IsChecked}:Saved");


                }



            }


            public bool CheckBoxiniGetVallue<T>(T check, string iniPath)
            {
                var checkControl = check as Control;
                // Debug.WriteLine(checkControl.Name);

                var setbool = IniDefinition.GetValueOrDefault(iniPath, "CheckState", checkControl.Name, false);
                return setbool;

            }




        }

    }
}
