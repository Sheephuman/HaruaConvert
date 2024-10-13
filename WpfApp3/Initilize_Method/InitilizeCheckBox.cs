using HaruaConvert.ini関連;
using HaruaConvert.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HaruaConvert.Initilize_Method
{
    class InitilizeCheckBox
    {
        public InitilizeCheckBox(ParamField _paramfield)
        {
            paramField = _paramfield;
        }
        ParamField paramField {  get; set; }


        public List<CheckBox> InitializeChildCheckBox(Window main, List<CheckBox> CheckBoxList)
        {
         
            CheckBoxList = new List<CheckBox>();


            //  childCheckBoxList.Capacity = 5; //現在のCheckBoxの数を指定


            // 子要素を列挙し、適切なリストに追加
            main.WalkInChildren(child =>
            {
                if (child is CheckBox checkBox)
                {
                    CheckBoxList.Add(checkBox);
                }

               

            });
            return CheckBoxList;
        }

        public List<MenuItem> InitializeChildCheckBox(Window main, List<MenuItem> menuList)
        {
            menuList = new List<MenuItem>();

            main.WalkInChildren(child =>
            {
                if (child is RichTextBox rich)
                {

                    var contextMenu = rich.ContextMenu;
                    foreach (MenuItem item in contextMenu.Items)
                    {

                        menuList.Add(item);
                    }
                }
            });

            return menuList;
        }

        public void LoadCheckBoxStates(List<CheckBox> childCheckBoxList)
        {
            var iniChecker = new 
                CheckBoxIniClass.CheckboxGetSetValueClass();
            foreach (var checkBox in childCheckBoxList)
            {
                // CheckBoxの状態をINIファイルから読み込む
                checkBox.IsChecked = iniChecker.CheckBoxiniGetVallue(checkBox, paramField.iniPath);
            }

        }

    }
}
