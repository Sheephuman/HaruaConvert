using HaruaConvert.Command;
using HaruaConvert.HaruaInterFace;
using HaruaConvert.Initilize_Method;
using HaruaConvert.Json;
using HaruaConvert.mainUI.mainWindow;
using HaruaConvert.Methods;
using HaruaConvert.Parameter;
using MakizunoSpellChecker;
using Microsoft.WindowsAPICodePack.Dialogs;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WpfApp3.Parameter;
using static HaruaConvert.Parameter.ParamField;


namespace HaruaConvert
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public partial class MainWindow : Window
    {
        public Harua_ViewModel harua_View { get; set; }

        /// <summary>
        /// 共有箇所：LogWindow
        /// </summary>
        public ParamField paramField { get; set; }



        CommonOpenDialogClass cod { get; set; }



        /// <summary>
        ///共有箇所：Generate_ParamSelector() :
        ///isUserOriginalParameter :
        ///コンストラクタ 
        /// </summary>
        public List<ParamSelector> selectorList { get; set; }




        public static bool firstSet { get; set; } //初回起動用
        public static bool firstlogWindow { get; set; }

        public string baseArguments { get; set; }

        List<CheckBox> childCheckBoxList;


        /// <summary>
        /// パラメータチェック用構造体
        /// Callクラス側で参照するためStatic
        /// </summary>
        public struct ChekOptionStruct
        {
            static public bool isAudio { get; set; }
            static public bool isForceFPS { get; set; }
            static public bool isLeftRotate { get; set; }
            static public bool isRightRotate { get; set; }
            static public bool isHorizontalRotate { get; set; }
            static public bool isNoRotate { get; set; }
        }

        //     bool isLabelEited { get; set; }


        /// <summary>
        /// https://rksoftware.wordpress.com/2016/05/30/001-5/
        /// </summary>



        public ObservableCollection<string> FileList { get; set; }

        Process mainProcess;


        //paramSelectorBox　生成数       
        public int SelGenerate { get; set; }

        public static MainWindow main { get; set; }


        public MainWindow()
        {
            InitializeComponent();




            main = this;
            drawhelper = new TextBoxStylingHelper();
            // MainWindow自身をIMediaInfoDisplayとしてMediaInfoServiceに渡す

            SetUIEvent uiManager = new SetUIEvent(main);
            uiManager.RegisterUIDropEvent();

            uiManager.SetupEventHandlers();

            InitializeParameters();



            InitializeViewModels();
            var initail = new InitilizeCheckBox(paramField);

            childCheckBoxList = initail.InitializeChildCheckBox(this);
            if (childCheckBoxList != null)
            {
                foreach (var chk in childCheckBoxList)
                {
                    chk.IsChecked = initail.LoadCheckBoxStates(chk);

                    Debug.WriteLine($"[{chk.Name}] load value = {chk.IsChecked.Value}");


                }
            }




            SelectorEventHandlers();

            placeHolderList.ItemsSource = new Dictionary<string, List<string>>
                {
                                                             { "{}",
                        new List<string> { "{", "}" } },

                      { "<>",
                        new List<string> { "<", ">" } },

                 };







            SetupUIEvents();


            LoadSettings();

            mainProcess = Process.GetCurrentProcess();




            FileList = new ObservableCollection<string>();
            Generate_ParamSelector();
            var cm = new HaruaCommandManager(main);
            cm.AddCommands();




        }


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

        private void chk_Loaded(object sender, RoutedEventArgs e)
        {

            //var iniSet = new Checker_IniIOClass.CheckboxGetSetValueClass();

        }



        public void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {


        }




        public void ParamSelect_Load(object sender, RoutedEventArgs e)
        {
            /////初回のみ呼ばれるようにする
            firstSet = ParamSelector_SetText(sender, firstSet);


        }




        string rcount { get; set; }

        bool ParamSelector_SetText(object sender, bool _firstSet)
        {
            int i = 0;
            if (_firstSet)
            {


                foreach (var selector in selectorList)
                {
                    selector.ArgumentEditor.Text = IniDefinition.GetValueOrDefault
                        (paramField.iniPath, ParamField.ControlField.ParamSelector + "_" + $"{i}", IniSettingsConst.Arguments_ + $"{i}",
                        "");


                    //selector.ArgumentEditor.Text);

                    selector.ParamLabel.Text = IniDefinition.GetValueOrDefault(paramField.iniPath, ParamField.ControlField.ParamSelector + "_" + $"{i}",
                    IniSettingsConst.ParameterLabel + "_" + $"{i}",
                 "パラメータ名").Replace("\r\n", "", StringComparison.Ordinal);

                    rcount = IniDefinition.GetValueOrDefault(paramField.iniPath, ClassShearingMenbers.CheckState, ParamField.ControlField.ParamSelector + "_Check", "0");
                    int rcountInt = int.Parse(rcount, CultureInfo.CurrentCulture);




                    i++;

                    if (selector.Name == ParamField.ControlField.ParamSelector + rcount)
                    {
                        selector.SlectorRadio.IsChecked = true;
                        paramField.usedOriginalArgument = selector.ArgumentEditor.Text;
                    }
                }


                _firstSet = false;
                return _firstSet;
            }

            return _firstSet;
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


        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// OutputfilePathのData Bind関係　
        /// </summary>
        /// <param name="propertyName"></param>
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
              => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));




        string _OutputfilePath;


        public string filePathOutput
        {
            get { return _OutputfilePath; }
            // 値をセットしたあと、画面側でも値が反映されるように通知を行います。
            set { if (_OutputfilePath != paramField.setFile) { _OutputfilePath = paramField.setFile; RaisePropertyChanged(); } }
        }




        /// <summary>
        ///  Reflect ffmpg command line Log to logWindows
        /// </summary>
        /// <param name="executeName"></param>









        private void TabItem_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }



        private void mainTub_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {


        }




        public void HandleMediaAnalysisException(Exception ex)
        {

            string message = ex.Message;

            // 特定の例外タイプに基づいてカスタマイズされたメッセージを追加
            if (ex is NullReferenceException)
            {
                message += "\nfforobeの呼び出しに失敗したみたい...";
            }
            else if (ex is Win32Exception)
            {
                message += "\nffprobe.exeがないわよ";
            }
            // その他の特定の例外に対する処理...

            MessageBox.Show(message);
            ClearSourceFileData();


        }



        public void ClearSourceFileData()
        {
            SorceFileDataBox.Document.Blocks.Clear();
        }


        private void Num_5_Initialized(object sender, EventArgs e)
        {
            //     Num_5.Height = Num_3.Height;
        }



        //string destinationPath { get; set; }

        private void OutputButton_Checked(object sender, RoutedEventArgs e)
        {

            ClassShearingMenbers.ButtonName = ((RadioButton)sender).Name;



            cod = new CommonOpenDialogClass(true, MainTab_OutputDirectory);

            var result = cod.CommonOpens();



            if (result == CommonFileDialogResult.Ok && !string.IsNullOrEmpty(cod.opFileName))
            {
                OutputPathText.Text = cod.opFileName;

                harua_View.OutputPath = cod.opFileName;
                main.paramField.isOutputButtonChecked = true;

                //Update MainTab_OutputDirectory
                //StaticParamField.MainTab_OutputDirectory = cod.opFileName;




            }
            HaruaGrid.Height += 30;
        }

        public delegate Task ProcessKill_deligate(int targetProcess);

        public delegate void ExitEvent_delegate(object sender, ExitEventArgs e);

        private void NoAudio_Checked(object sender, RoutedEventArgs e)
        {

            ChekOptionStruct.isAudio = NoAudio.IsChecked.Value;

            if (!ChekOptionStruct.isAudio)
            {
                _arguments = _arguments.Replace("-an", "");
            }
            else if (ChekOptionStruct.isAudio)
            {
                _arguments += (" -an ");
            }

        }

        private void Force30FPS_Checked(object sender, RoutedEventArgs e)
        {
            ChekOptionStruct.isForceFPS = Force30FPS.IsChecked.Value;

            if (!ChekOptionStruct.isForceFPS)
            {
                _arguments = _arguments.Replace(" -r 30 ", "");
            }
            else if (ChekOptionStruct.isForceFPS)
            {
                _arguments += (" -r 30 ");
            }

        }

        private void AutoLocateButton_Checked(object sender, RoutedEventArgs e)
        {

            harua_View.OutputPath = "";
            OutputPathText.Text = "";
            ParamField.MainTab_OutputDirectory = "";
        }

        private void isUseOriginalCheckProc(bool _checkState)
        {

            baseArguments = "";
            var mediaColor = Color.FromRgb(222, 222, 222);

            if (!HasContent)
                return;

            if (_checkState)
            {

                ParaSelectGroup.IsEnabled = true;

                ExecButton.IsEnabled = true;



            }
            else
            {




                // ParaSelectGroup.Background.Opacity = 1;

                ParaSelectGroup.IsEnabled = false;
                ExecButton.IsEnabled = false;


                ParaSelectGroup.Background = new SolidColorBrush(mediaColor);

            }

        }


        private void isUserOriginalParameter_Checked(object sender, RoutedEventArgs e)
        {
            isUseOriginalCheckProc(isUserParameter.IsChecked.Value);

            if (ParaSelectGroup.Background != null)
                ParaSelectGroup.Background.Opacity =
                isUserParameter.IsChecked.Value ? 0 : 1;
        }


        public void ParamSave_Procedure(bool isEdit, bool isChecked)
        {

            int i = 0;

            if (isEdit)
                //Add Number and Save setting.ini evey selector 
                foreach (var selector in selectorList)
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
                        IniDefinition.SetValue(paramField.iniPath, ClassShearingMenbers.CheckState, ParamField.ControlField.ParamSelector + "_Check", radioCount);

                }
            }

            {



            if (isChecked)
            {
                var checkedSet = new IniCheckerClass.CheckboxGetSetValueClass();

                if (childCheckBoxList != null)
                    foreach (CheckBox chk in childCheckBoxList)
                    {

                        checkedSet.CheckediniSetVallue(chk, paramField.iniPath);
                    }
            }

            var setWriter = new IniSettings_IOClass();
            setWriter.IniSettingWriter(paramField, this);

                    if (!string.IsNullOrEmpty(ParamText.Text))
                        IniDefinition.SetValue(paramField.iniPath, QueryNames.ffmpegQuery, QueryNames.BaseQuery, ParamText.Text);

                    if (!string.IsNullOrEmpty(endStringBox.Text))
                        IniDefinition.SetValue(paramField.iniPath, QueryNames.ffmpegQuery, QueryNames.endStrings, endStringBox.Text);

                    if (!string.IsNullOrEmpty(placeHolderList.Text))
                        IniDefinition.SetValue(paramField.iniPath, QueryNames.placeHolder, QueryNames.placeHolderCount, placeHolderList.SelectedIndex.ToString(CultureInfo.CurrentCulture));
                }


                //背景画像のOpacity書き込み
                IniDefinition.SetValue(paramField.iniPath, IniSettingsConst.Apperance, IniSettingsConst.BackImageOpacity, main.harua_View.MainParams[0].BackImageOpacity.ToString(CultureInfo.CurrentCulture));


                var ffjQuery = new CommandHistory();

                CommandHistoryIO qHistory = new();

                foreach (var item in ParamText.Items)
                {

                    ffjQuery.ffQueryToken.Add(item.ToString());


                }


                qHistory.SaveToJsonFile(ffjQuery, "CommandHistory.json");
            }
        }



        private void Window_Closed(object sender, EventArgs e)
        {


        }

        private void Window_StateChanged(object sender, EventArgs e)
        {

            if (this.WindowState == WindowState.Maximized)
                this.WindowState = WindowState.Normal;
        }



        static bool isEmptyInputter;
        static bool _isEmptyInputter
        {
            get { return isEmptyInputter; }
            set { isEmptyInputter = true; }
        }



        public CommonOpenDialogClass ofc { get; set; }
        internal FfmpegQueryClass Ffmpc { get; set; }

        public void FileSelector_MouseDown(object sender, RoutedEventArgs e)
        {



            var ansest = VisualTreeHelperWrapperHelpers.FindAncestor<FileSelector>((DependencyObject)sender);


            ClassShearingMenbers.ButtonName = ansest.Name;

            var selOpens = new Selector_OpenMethodClass(this);


            CommonFileDialogResult res = ansest.Name == ControlField.InputSelector ?
                 selOpens.Selector_ComonOpenMethod(false, ansest) : selOpens.Selector_ComonOpenMethod(true, ansest);
            //三項演算子





            //var param = new ParamCreateClasss(InputSelector.FilePathBox.Text);

            if (res == CommonFileDialogResult.Ok)
            {
                if (ansest.Name == InputSelector.Name)
                {
                    //  string _fileName = OutputSelector.FilePathBox.Text = param.ConvertFileNameClass(InputSelector.FilePathBox.Text);
                    //  string _fileName = OutputSelector.FilePathBox.Text = param.ConvertFileNameClass(InputSelector.FilePathBox.Text);

                    ParamField.ParamTab_InputSelectorDirectory = Path.GetDirectoryName(ofc.opFileName);
                }
                if (ansest.Name == OutputSelector.Name)
                {
                    ParamField.ParamTab_OutputSelectorDirectory = ofc.opFileName;
                }
            }


        }


        public string outputFileName { get; set; }





        bool isForceExec;
        private void isForceExec_






            (object sender, RoutedEventArgs e)
        {
            isForceExec = isForceExecCheckBox.IsChecked.Value ? true : false;
        }


        private void LinkLabel_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {


            OpenUrl("https://twitter.com/shiyokatadragon");
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

                ParamSelector sbx = new ParamSelector(this)
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

        int Generatednum { get; set; }



        public IMainTabEvents[] mainTabEvents { get; set; }


        public void DropButton_ClickHandle(object sender, RoutedEventArgs e)
        {


            foreach (var button in mainTabEvents)
            {

                if (((Button)sender).Name == ButtonNameField.Directory_DropButon)
                {
                    button.Directory_DropButon_Click(sender, e);

                    return;
                }

                else if (((Button)sender).Name == ButtonNameField.Convert_DropButton)
                {
                    button.Convert_DropButton_Click(sender, e);

                    return;
                }



            }

        }



        public string AddParam(string locatePath)
        {
            throw new NotImplementedException();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }


        private void InputSelector_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy; // マウスカーソルをコピーにする。
            e.Handled = e.Data.GetDataPresent(DataFormats.FileDrop);
            // ドラッグされてきたものがFileDrop形式の場合だけ、このイベントを処理済みにする。
        }




        /// <summary>
        /// 

        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void ParamSelect_MouseEnter(object sender, MouseEventArgs e)
        {

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



        private void LinkLabel2_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {

        }

        private void AtacchStringsList_Loaded(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(IniDefinition.GetValueOrDefault(paramField.iniPath, QueryNames.placeHolder, QueryNames.placeHolderCount, "0"), CultureInfo.CurrentCulture);
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

            drawhelper.DrawSinWave(InnerTextBox, "rules.json", 3);
        }




        private void ParamText_Loaded(object sender, RoutedEventArgs e)
        {
            var Jsonreader = new CommandHistoryIO();


            var tokenList = Jsonreader.ReadtoJsonFile<string>("CommandHistory.json");
            foreach (string token in tokenList)
            {
                if (!ParamText.Items.Contains(token))
                    ParamText.Items.Add(token);
            }


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

        private void ParamText_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!paramField.isParamEdited)
                paramField.isParamEdited = true;
        }

        private void ParamText_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;

            if (Keyboard.IsKeyDown(Key.Down) || Keyboard.IsKeyDown(Key.Up) && !ParamText.IsDropDownOpen)
                ParamText.IsDropDownOpen = true; // Ensure the dropdown remains open after selection change

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
    }

}