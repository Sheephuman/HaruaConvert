using FFMpegCore;
using HaruaConvert.Command;
using HaruaConvert.HaruaInterFace;
using HaruaConvert.HaruaServise;
using HaruaConvert.Initilize_Method;
using HaruaConvert.ini関連;
using HaruaConvert.Methods;
using HaruaConvert.Parameter;
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

        mainUIparameter mfc { get; set; }

        CommonOpenDialogClass cod { get; set; }



        /// <summary>
        ///共有箇所：Generate_ParamSelector() :
        ///isUserOriginalParameter :
        ///コンストラクタ 
        /// </summary>
        public List<ParamSelector> selectorList { get; set; }


        public static bool firstShowLog { get; set; }

    public static bool firstSet { get; set; } //初回起動用
        

        public string baseArguments { get; set; }

       public List<CheckBox> childCheckBoxList { get; set; }


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

            // MainWindow自身をIMediaInfoDisplayとしてMediaInfoServiceに渡す
            
            SetUIEvent uiManager = new SetUIEvent(main);
            uiManager.RegisterUIDropEvent();

            uiManager.SetupEventHandlers();

            InitializeParameters();

            Lw = new LogWindow(paramField);
　　　　　

            InitializeViewModels();
            var initail = new InitilizeCheckBox(paramField);

           childCheckBoxList = initail.InitializeChildCheckBox(this,childCheckBoxList);
            if(childCheckBoxList != null)
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




        
        public void ArgumentEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            var ansest = VisualTreeHelperWrapperHelpers.FindAncestor<ParamSelector>((TextBox)sender);
            //var ansest = sender as ParamSelector;
            if (ansest == null)
            {

                return;
            }

            paramField.isParam_Edited = firstSet ? false : true;

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

                    rcount = IniDefinition.GetValueOrDefault(paramField.iniPath, "CheckState", ParamField.ControlField.ParamSelector + "_Check", "0");
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

        public delegate void ExitEvent_delegate(object sender , ExitEventArgs e);

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



    //    public CommonOpenDialogClass ofc { get; set; }
        internal FfmpegQueryClass Ffmpc { get; set; }

        public void FileSelector_MouseDown(object sender, RoutedEventArgs e)
        {



            var ansest = VisualTreeHelperWrapperHelpers.FindAncestor<FileSelector>((DependencyObject)sender);


            ClassShearingMenbers.ButtonName = ansest.Name;

            var selOpens = new Selector_OpenMethodClass(this,paramField);


            CommonFileDialogResult res = ansest.Name == ControlField.InputSelector ?
                 selOpens.Selector_ComonOpenMethod(false, ansest) : selOpens.Selector_ComonOpenMethod(true, ansest);
            //三項演算子





            //var param = new ParamCreateClasss(InputSelector.FilePathBox.Text);

            if (res == CommonFileDialogResult.Ok)
            {
                mfc.ofc = new CommonOpenDialogClass(false, ParamField.ParamTab_OutputSelectorDirectory);
                if (ansest.Name == InputSelector.Name)
                {
                    //  string _fileName = OutputSelector.FilePathBox.Text = param.ConvertFileNameClass(InputSelector.FilePathBox.Text);
                    //  string _fileName = OutputSelector.FilePathBox.Text = param.ConvertFileNameClass(InputSelector.FilePathBox.Text);

                    ParamField.ParamTab_InputSelectorDirectory = Path.GetDirectoryName(mfc.ofc.opFileName);
                }
                if (ansest.Name == OutputSelector.Name)
                {
                   
                    ParamField.ParamTab_OutputSelectorDirectory = mfc.ofc.opFileName;
                    
                }
            }


        }




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

        private void isOpenFolder_Checked(object sender, RoutedEventArgs e)
        {
            paramField.isOpenFolder = (bool)IsOpenForuderChecker.IsChecked ? true : false;


        }

        private string GetDebuggerDisplay()
        {
            return ToString();
        }

        private void opacitySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (OpacityText == null)
                return;
            if (harua_View == null)
                return;

            // スライダーの値をウィンドウの透明度に反映
            harua_View.MainParams[0].BackImageOpacity = opacitySlider.Value;


            // テキストに現在の透明度を表示
            OpacityText.Text = $"{opacitySlider.Value:f1}";
         

        }

        private void OpacitySlider_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // マウスの位置を取得
            Point mousePosition = e.GetPosition(opacitySlider);

            // スライダーの幅を取得
            double sliderWidth = opacitySlider.ActualWidth;

            // スライダーの最小値と最大値を取得
            double minValue = opacitySlider.Minimum;
            double maxValue = opacitySlider.Maximum;

            // クリック位置をスライダーの値に変換
            double value = minValue + (mousePosition.X / sliderWidth) * (maxValue - minValue);

            // 値をスライダーに設定
        
            //Debug.WriteLine(Math.Max(minValue, Math.Min(maxValue, Math.Round(value, 1))));

            var roundValue = Math.Max(minValue, Math.Min(maxValue, Math.Round(value, 1)));
            
            opacitySlider.SetValue(Slider.ValueProperty,roundValue);
        }
    }
    
}



