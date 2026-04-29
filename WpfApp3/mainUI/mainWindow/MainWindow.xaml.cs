using Castle.Components.DictionaryAdapter.Xml;
using HaruaConvert.Command;
using HaruaConvert.HaruaInterFace;
using HaruaConvert.Initilize_Method;
using HaruaConvert.Json;
using HaruaConvert.mainUI.mainWindow;
using HaruaConvert.Methods;
using HaruaConvert.Parameter;
using HaruaConvert.ViewModel.ffmpegOptions.CheckBox;
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
using static HaruaConvert.IniCreate;
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
        /// パラメータチェック用構造体
        /// Callクラス側で参照するためStatic
        /// </summary>
        public struct ChekOptionStruct
        {

            static public bool isForceFPS { get; set; }
            static public bool isLeftRotate { get; set; }
            static public bool isRightRotate { get; set; }
            static public bool isHorizontalRotate { get; set; }
            static public bool isNoRotate { get; set; }
            public static bool isAudio { get; internal set; }
        }

        //     bool isLabelEited { get; set; }


        /// <summary>
        /// https://rksoftware.wordpress.com/2016/05/30/001-5/
        /// </summary>



        public ObservableCollection<string> FileList { get; set; }


        Process mainProcess;



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

            initail.LoadCheckBoxStates(childCheckBoxList);





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





        public event
            PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// OutputfilePathのData Bind関係　
        /// </summary>
        /// <param name="propertyName"></param>
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
              => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));




        string _filePathOutput;


        public string filePathOutput
        {
            get { return _filePathOutput; }
            // 値をセットしたあと、画面側でも値が反映されるように通知を行います。
            set
            {
                if (_filePathOutput != paramField.setFile)
                {

                    _filePathOutput = paramField.setFile;

                    RaisePropertyChanged();
                }
            }
        }




        /// <summary>
        ///  Reflect ffmpg command line Log to logWindows
        /// </summary>
        /// <param name="executeName"></param>



        public void ClearSourceFileData()
        {
            SorceFileDataBox.Document.Blocks.Clear();
        }




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

            ChekOptionStruct.isAudio = NoAudio.IsChecked == true;

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
            ChekOptionStruct.isForceFPS = Force30FPS.IsChecked == true;

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
            isUseOriginalCheckProc(isUserParameter.IsChecked == true);

            if (ParaSelectGroup.Background != null)
                ParaSelectGroup.Background.Opacity =
                isUserParameter.IsChecked == true ? 0 : 1;
        }


        public void ParamSave_Procedure(bool isEdit, bool isChecked)
        {
            try
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



                        ++i;



                        //if Check Selector Radio, Save Check State
                        if (selector.SlectorRadio.IsChecked == true)
                        {
                            var radioCount = selector.Name.Remove(0, ParamField.ControlField.ParamSelector.Length);
                            IniDefinition.SetValue(paramField.iniPath, ClassShearingMenbers.CheckState, ParamField.ControlField.ParamSelector + "_Check", radioCount);

                        }
                    }

                {



                    if (isChecked)
                    {
                        var initail = new InitilizeCheckBox(paramField);



                        var checkedSet = new IniCheckBoxSetClass();

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

                string history = Path.Combine(AppContext.BaseDirectory, "CommandHistory.json");

                if (!File.Exists(history))
                    return;
                qHistory.SaveToJsonFile(ffjQuery, "CommandHistory.json");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

        }


        private void Window_StateChanged(object sender, EventArgs e)
        {

            if (this.WindowState == WindowState.Maximized)
                this.WindowState = WindowState.Normal;
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
            isForceExec = isForceExecCheckBox.IsChecked == true ? true : false;
        }





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


      
    }
}