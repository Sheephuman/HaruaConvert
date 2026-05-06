using HaruaConvert.Command;
using HaruaConvert.HaruaInterFace;
using HaruaConvert.Json;
using HaruaConvert.Methods;
using HaruaConvert.Methods.Settings;
using HaruaConvert.Parameter;
using MakizunoSpellChecker;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp3.Parameter;
using static HaruaConvert.IniCreate;
using static HaruaConvert.Parameter.ParamField;

namespace HaruaConvert
{
    public partial class MainWindow : Window
    {
        /// <summary>
        ///共有箇所：Generate_ParamSelector() :
        ///isUserOriginalParameter :
        ///コンストラクタ 
        /// </summary>
        public List<ParamSelector> selectorList { get; set; }






        public bool firstSet { get; set; } //初回起動用
        public bool firstlogWindow { get; set; }

        public string baseArguments { get; set; }

        List<CheckBox> childCheckBoxList;

        /// <summary>
        /// 共有箇所：LogWindow
        /// </summary>
        public ParamField paramField { get; set; }
        private readonly IMainWindowUiDataLoaderService _uiDataLoaderService = new MainWindowUiDataLoaderService();



        CommonOpenDialogClass cod { get; set; }

        //CodeBehindaの責務
        public void InvisibleText_LostFocus(object sender, RoutedEventArgs e)
        {

            foreach (ParamSelector sel in selectorList)
            {
                //var inText = VisualTreeHelperWrapperHelpers.FindDescendant<TextBox_Extend>((ParamSelector)sender);
                var inText = sender as ParamSelector;
                if (inText.invisibleText == sel.invisibleText)
                    sel.invisibleText.Visibility = Visibility.Hidden;
                sel.ParamLabel.Visibility = Visibility.Visible;


            }

        }


        public void ParamSelect_Load(object sender, RoutedEventArgs e)
        {
            /////初回のみ呼ばれるようにする
            firstSet = ParamSelector_SetText(sender, firstSet);


        }



        bool ParamSelector_SetText(object sender, bool _firstSet)
        {
            return _uiDataLoaderService.ApplySelectorInitialValues(this, _firstSet);
        }



        //paramSelectorBox　生成数       
        public int SelGenerate { get; set; }

        //CodeBehindaの責務
        public void ArgumentEditor_TextChanged(object sender, TextChangedEventArgs e)
        {


            var ansest = VisualTreeHelperWrapperHelpers.FindAncestor<ParamSelector>((TextBox)sender);
            //var ansest = sender as ParamSelector;
            if (ansest == null)
            {

                return;
            }
            if (!paramField.isParamEdited)
                paramField.isParamEdited = true;

            paramField.usedOriginalArgument = ansest.ArgumentEditor.Text;


        }

        //ParameterSelevter生成用
        void Generate_ParamSelector()
        {
            if (!firstSet)
            {
               SelectorStack.Children.Clear();

                selectorList.Clear();
            }



            //List<SelectParamerterBox> selList = new List<SelectParamerterBox>();
            selectorList = new List<ParamSelector>();

            SelGenerate = int.Parse(NumericUpDown1.NUDTextBox.Text, CultureInfo.CurrentCulture);



            int count = 0;
            for (; SelGenerate > 0; SelGenerate--)
            {

                Thickness marthick = new Thickness(-40, 0, 0, 0);

                ParamSelector sbx = new (this)
                {
                    Margin = marthick,
                    FontSize = 14,
                    Width = 480,
                    Name = "ParamSelector" + $"{count++}"
                };


                sbx.ParamLabel.Margin = new Thickness(15, 3, 10, 0);
                //Margin = "15,3,10,0"
                sbx.ArgumentEditor.Width = 380;
                sbx.ParamLabel.Width = double.NaN;


                //sbxが既に含まれていない場合にSelectorStackを追加（重複防止）
                if (!SelectorStack.Children.Contains(sbx))
                {
                    SelectorStack.Children.Add(sbx);
                    selectorList.Add(sbx);
                }

            }
            //SelGenerate = count;



            GenerateSelectParaClass gsp = new GenerateSelectParaClass();
            foreach (var selector in selectorList)
            {
                gsp.GenerateParaSelector_setPropaties(selector, this);
            }


        }



        private void IsOpenFolderChecker_Checked(object sender, RoutedEventArgs e)
        {
            paramField.isOpenFolder = IsOpenFolderChecker.IsChecked.Value ? true : false;
        }

        private void isFileOpenChecker_Checked(object sender, RoutedEventArgs e)
        {
            paramField.isOpenFile = isFileOpenChecker.IsChecked.Value ? true : false;
        }

        private void minimzizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }



        private void ParamText_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;

            if (Keyboard.IsKeyDown(Key.Down) || Keyboard.IsKeyDown(Key.Up) && !ParamText.IsDropDownOpen)
                ParamText.IsDropDownOpen = true; // Ensure the dropdown remains open after selection change

        }



        private void ParamText_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _uiDataLoaderService.LoadCommandHistoryItems(ParamText);


                var combo = sender as ComboBox;

                InnerTextBox = combo.Template.FindName("PART_EditableTextBox", combo) as TextBox;



                var contextMenu = new ContextMenu();

                contextMenu.Items.Add(HaruaButtonCommand.SetDefaultQuery);

                contextMenu.Items.Add(HaruaButtonCommand.QueryBuildWindow_Open);


                InnerTextBox.ContextMenu = contextMenu;


                InnerTextBox.KeyDown += (sender, e) =>
                {
                    main.paramField.isParam_Edited = true;
                };


                InnerTextBox.KeyDown += (sender, e) =>
                {
                    if (Keyboard.IsKeyDown(Key.Enter))
                        if (!ParamText.Items.Contains(InnerTextBox.Text))
                            ParamText.Items.Add(InnerTextBox.Text);
                };




            }
            catch (System.IO.FileNotFoundException ex)
            {
                MessageBox.Show("ファイルがみつからないわ" + ex.Message + ex.FileName);

            }
        }




        private void ParamText_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!paramField.isParamEdited)
                paramField.isParamEdited = true;
        }



        private void LinkLabel2_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {

        }

        private void AtacchStringsList_Loaded(object sender, RoutedEventArgs e)
        {
            int index = _uiDataLoaderService.LoadPlaceholderIndex(paramField.iniPath);
            if (index >= 0)
                placeHolderList.SelectedIndex = index;

        }



        private string GetDebuggerDisplay()
        {
            return ToString();
        }

        private void OpacitySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (OpacityText != null && harua_View != null)
            {

                harua_View.MainParams[0].BackImageOpacity = opacitySlider.Value;
                OpacityText.Content = $"{opacitySlider.Value:f1}";


            }
        }

        TextBoxStylingHelper drawhelper;
        TextBox InnerTextBox;
        private void ParamText_InputIdle(object sender, EventArgs e)
        {
            var combo = sender as ComboBox;
            ///ConboBox is Editable = true
            ///
            // ComboBox のテンプレートから TextBox を探す
            InnerTextBox = combo.Template.FindName("PART_EditableTextBox", combo) as TextBox;


            if (!File.Exists("rules.json"))
                return;


            drawhelper.DrawSinWave(InnerTextBox, "rules.json", 3);
        }



        private void InputSelector_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy; // マウスカーソルをコピーにする。
            e.Handled = e.Data.GetDataPresent(DataFormats.FileDrop);
            // ドラッグされてきたものがFileDrop形式の場合だけ、このイベントを処理済みにする。
        }



        internal void ParamSelector_MouseEnter(object sender, MouseEventArgs e)
        {
            //var ansest = sender as ParamSelector;
            var ansest = VisualTreeHelperWrapperHelpers.FindAncestor<ParamSelector>((ContentControl)sender);
            if (ansest == null)
                return;


            if (!string.IsNullOrEmpty(ansest.ParamLabel.Text))
                ansest.ParamLabel.ToolTip = ansest.ParamLabel.Text;

      }

        private static Process OpenUrl(string url)
        {
            ProcessStartInfo pi = new ProcessStartInfo()
            {
                FileName = url,
                UseShellExecute = true,
            };

            return Process.Start(pi);
        }

        RadioButton radioSel;

        public void SlectorRadio_Checked(object sender, RoutedEventArgs e)

        {
            radioSel = sender as RadioButton;


            foreach (ParamSelector rd in selectorList)
            {
                if (rd.SlectorRadio == radioSel)
                {
                    //baseArguments = rd.ArgumentEditor.Text;
                    if (isUserParameter.IsChecked.Value)
                    {
                        paramField.usedOriginalArgument = rd.ArgumentEditor.Text;
                        _arguments = rd.ArgumentEditor.Text;
                    }

                    return;
                }
            }
        }
        private void LinkLabel_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {


            OpenUrl("https://twitter.com/shiyokatadragon");
        }


    }
}

