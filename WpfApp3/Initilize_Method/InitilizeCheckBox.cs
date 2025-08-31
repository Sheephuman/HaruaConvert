using HaruaConvert.Methods;
using HaruaConvert.Parameter;
using System.Collections.Generic;
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
        ParamField paramField { get; set; }

        List<CheckBox> childCheckBoxList;

        public List<CheckBox> InitializeChildCheckBox(Window main)
        {

            childCheckBoxList = new List<CheckBox>();


            //  childCheckBoxList.Capacity = 5; //現在のCheckBoxの数を指定


            // 子要素を列挙し、適切なリストに追加
            main.WalkInChildren(child =>
            {
                if (child is CheckBox checkBox)
                {
                    childCheckBoxList.Add(checkBox);


                }

            });
            foreach (var che in childCheckBoxList)
            {
                che.Checked += (s, e) =>
                {
                    paramField.isCheckerChanged = true;
                };
                che.Unchecked += (s, e) =>
                {
                    paramField.isCheckerChanged = true;
                };
            }

            return childCheckBoxList;
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

        public bool LoadCheckBoxStates(CheckBox checkBox)
        {


            var iniChecker = new IniCheckerClass();
            foreach (var checker in childCheckBoxList)
            {
                // CheckBoxの状態をINIファイルから読み込む
                checker.IsChecked = iniChecker.CheckBoxiniGetVallue(checker, paramField.iniPath);
            }
            return true;
        }

    }
}

