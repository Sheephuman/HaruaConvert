
using FFMpegCore;
using HaruaConvert.HaruaInterFace;
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
using System.Windows.Threading;
using Windows.Storage.Streams;
using WpfApp3.Parameter;
using static HaruaConvert.Parameter.ParamField;

namespace HaruaConvert
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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
        ///isUserOriginalParameter_Method :
        ///コンストラクタ 
        /// </summary>
        List<ParamSelector> selectorList;




        bool firstSet { get; set; } //初回起動用
        string baseArguments;

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
        bool isUPDownClicked;

        //     bool isLabelEited { get; set; }


        /// <summary>
        /// https://rksoftware.wordpress.com/2016/05/30/001-5/
        /// </summary>



        private readonly ObservableCollection<string> FileList;

        Process mainProcess;


        //private const uint WM_DROPFILES = 0x233;
        //private const uint WM_COPYDATA = 0x4a;

        //private const uint WM_COPYGLOBALDATA = 0x49;



        //paramSelectorBox　生成数       
        public int SelGenerate { get; set; }



        public MainWindow()
        {
            InitializeComponent();



            paramField = new ParamField();
            paramField.isParam_Edited = false;

            Ffmpc = new FfmpegQueryClass(this);

            isUPDownClicked = false;
            paramField.isAutoScroll = true;


            paramField.iniPath = Path.Combine(Environment.CurrentDirectory, "Settings.ini");


            {
                /////////
                ////https://csharp.hotexamples.com/jp/examples/-/CHANGEFILTERSTRUCT/-/php-changefilterstruct-class-examples.html
                /////　UACが有効でもAllowDrop 出来るようにする
                ///////
                //var handle = Process.GetCurrentProcess().MainWindowHandle;
                //CHANGEFILTERSTRUCT filterStatus = new CHANGEFILTERSTRUCT();


                ////filterStatus.size = (uint)Marshal.SizeOf(filterStatus);
                //filterStatus.size = Convert.ToUInt32(Marshal.SizeOf(typeof(CHANGEFILTERSTRUCT)));
                //filterStatus.info = 0;

                //Allowdoep_UAC = ChangeWindowMessageFilterEx(handle, WM_DROPFILES, ChangeWindowMessageFilterExAction.Allow, ref filterStatus);
                //ChangeWindowMessageFilterEx(handle, WM_COPYDATA, ChangeWindowMessageFilterExAction.Allow, ref filterStatus);
                //ChangeWindowMessageFilterEx(handle, WM_COPYGLOBALDATA, ChangeWindowMessageFilterExAction.Allow, ref filterStatus);
            }

            firstSet = true;


            {
                ///
                /////Load Setting ini
                ///









                isUseOriginalCheckProc(isUserParameter.IsChecked.Value);


                var iniCon = new IniSettingsConst();

                var setiniReader = new IniSettings_IOClass();
                setiniReader.IniSettingReader(paramField,this);

                #region SelectParameterBox Generate
                {
                    ////
                    /// SelectParameterBox Generate
                    ///
                    SelGenerate = SelGenerate = int.Parse(NumericUpDown1.NUDTextBox.Text, CultureInfo.CurrentCulture);





                    Generate_ParamSelector();
                    #endregion
                }
            }



            if (firstSet)
                paramField.isExitProcessed = true;

            mainProcess = Process.GetCurrentProcess();


            //isLabelEited = false;

            Lw = new LogWindow(this);

            {
                FileList = new ObservableCollection<string>();


                ClassShearingMenbers.endFileNameStrings = IniDefinition.GetValueOrDefault(paramField.iniPath, QueryNames.ffmpegQuery , QueryNames.endStrings, "_Harua");
                //  Set Default Parameter on FfmpegQueryClass
                harua_View = new Harua_ViewModel(this);




                DataContext = harua_View._Main_Param;


                //_arguments = Harua_ViewModel.StartQuery;
            }


            #region Register Events

            {
                NumericUpDown1.NUDButtonUP.Click += NUDUP_Button_Click;
                NumericUpDown1.NUDButtonDown.Click += NUD_DownButton_Click;
            }


            {


                InputSelector.AllowDrop = true;
                InputSelector.FilePathBox.AllowDrop = true;

                //InputSelector.openDialogButton.Name = InputSelector.Name + "_openButton";



                InputSelector.FilePathBox.Drop += FileDrop;
                InputSelector.openDialogButton.Drop += FileDrop;
            }


            MouseLeftButtonDown += (sender, e) => { DragMove(); };

            //No Frame Window Enable Moving
            //http://getbget.seesaa.net/article/436398354.html

            InputSelector.openDialogButton.PreviewMouseDown
                 += FileSelector_MouseDown;

            OutputSelector.openDialogButton.PreviewMouseDown
                 += FileSelector_MouseDown;



            ///////
            ////https://qiita.com/tricogimmick/items/4347214669a99cd2c775
            /////

            Loaded += (o, e) =>
            {

                selectorList = new List<ParamSelector>();
                childCheckBoxList = new List<CheckBox>();

                childCheckBoxList.Capacity = 4;


                //子要素を列挙するDelegate
                this.WalkInChildren(child =>
                {
                    var checkedCheckbox = child.GetType().Equals(typeof(CheckBox));

                    if (checkedCheckbox)
                    {
                        childCheckBoxList.Add((CheckBox)child);
                    }




                    var isControl = child.GetType().Equals(typeof(ParamSelector));

                    if (isControl)
                    {
                        selectorList.Add((ParamSelector)child);
                    }



                    Debug.WriteLine(child);
                });

                var init = new IniGetSetValueClass.CheckboxGetSetValueClass();


                foreach (CheckBox chk in childCheckBoxList)
                {

                    chk.IsChecked = init.CheckBoxiniGetVallue(chk, paramField.iniPath);
                }


                GenerateSelectParaClass gsp = new GenerateSelectParaClass();
                

                selectorList.Capacity = selectorList.Count;

                foreach (var selector in selectorList)
                {
                    gsp.GenerateParaSelector_setPropaties(selector, this);



                    //SelGenerate = count;
                }


            };

            #endregion





        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:識別子はアンダースコアを含むことはできません", Justification = "<保留中>")]
        public void ArgumentEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            var ansest = VisualTreeHelperWrapperHelpers.FindAncestor<ParamSelector>((TextBox)sender);
            //var ansest = sender as ParamSelector;
            if (ansest == null)
                return;


            paramField.usedOriginalArgument = ansest.ArgumentEditor.Text;

        }






#pragma warning disable CA1707 // 識別子はアンダースコアを含むことはできません
        public void InvisibleText_LostFocus(object sender, RoutedEventArgs e)
#pragma warning restore CA1707 // 識別子はアンダースコアを含むことはできません
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

            //var iniSet = new IniGetSetValueClass.CheckboxGetSetValueClass();

        }



        public void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {


        }




        public void ParamSelect_Load(object sender, RoutedEventArgs e)
        {
            /////初回のみ呼ばれるようにする
            firstSet = ParamSelector_SetText(sender, firstSet);
        }





#pragma warning disable CA1707 // 識別子はアンダースコアを含むことはできません
        public void InvisibleText_KeyDown(object sender, KeyEventArgs e)
#pragma warning restore CA1707 // 識別子はアンダースコアを含むことはできません
        {
            var selecter = sender as ParamSelector;


            if (selecter == null)
            {
                return;
            }


            if (e.Key == Key.Escape)
            {
                foreach (ParamSelector sp in selectorList)
                {
                    if (!string.IsNullOrEmpty(selecter.invisibleText.Text) && selecter.invisibleText == sp.invisibleText)
                    {

                        selecter.invisibleText.Visibility = Visibility.Hidden;
                        selecter.ParamLabel.Visibility = Visibility.Visible;
                        return;
                    }
                    else if (selecter.invisibleText.Text == "\r\n")
                    {

                        selecter.invisibleText.Text = selecter.ParamLabel.Text;


                        sender = null;
                        return;
                    }
                }

            }




            ///http://www.madeinclinic.jp/c/20180421/
            else if (e.KeyboardDevice.Modifiers == ModifierKeys.Shift && e.Key == Key.Enter)
            {
                paramField.isParam_Edited = true;



                foreach (ParamSelector sp in selectorList)
                {
                    //ImeEnabled = InputMethod.GetIsInputMethodEnabled(sp.invisibleText);

                    // if (ImeEnabled)
                    if (string.IsNullOrEmpty(selecter.invisibleText.Text))
                    {
                        return;
                    }




                    //何故かNUll文字ではなく改行コードが必ず入っているため　原因不明
                    if (selecter.invisibleText.Text != Environment.NewLine)
                    {
                        selecter.invisibleText.Visibility = Visibility.Hidden;

                        //     selecter.ParamLabel.Text = selecter.invisibleText.Text.Replace("\r\n", "", StringComparison.Ordinal);


                        selecter.ParamLabel.Visibility = Visibility.Visible;

                        selecter.ParamLabel.Text = selecter.invisibleText.Text;
                        //isLabelEited = false;


                    }

                    else
                    {
                        MessageBox.Show("名前を設定してください");
                        selecter.invisibleText.Text = selecter.ParamLabel.Text;
                        selecter.ParamLabel.Visibility = Visibility.Hidden;
                        selecter.invisibleText.Visibility = Visibility.Visible;

                        this.Dispatcher.BeginInvoke((Action)delegate
                        {
                            Keyboard.Focus(sp.invisibleText);
                        }, DispatcherPriority.Render);


                        return;
                    }

                }
            }
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

#pragma warning disable CA1707 // 識別子はアンダースコアを含むことはできません
        public void SlectorRadio_Checked(object sender, RoutedEventArgs e)
#pragma warning restore CA1707 // 識別子はアンダースコアを含むことはできません
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

        //[DllImport("user32
        //
        //
        //
        //
        //
        //
        //
        //", SetLastError = true)]
        //public static extern bool ChangeWindowMessageFilterEx(IntPtr hWnd, uint msg, ChangeWindowMessageFilterExAction action, ref CHANGEFILTERSTRUCT changeInfo);

        //public enum ChangeWindowMessageFilterExAction : uint
        //{
        //    Reset = 0, Allow = 1, DisAllow = 2
        //};

        //[StructLayout(LayoutKind.Sequential)]
        //public struct CHANGEFILTERSTRUCT
        //{
        //    public uint size;
        //    public MessageFilterInfo info;
        //}
        //public enum MessageFilterInfo : uint
        //{
        //    None = 0, AlreadyAllowed = 1, AlreadyDisAllowed = 2, AllowedHigher = 3
        //};


        // bool Allowdoep_UAC = false;
        private void FileDrop(object sender, DragEventArgs e)
        {

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                FileList.Clear();
                var fileNames = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (var name in fileNames)
                {
                    FileList.Add(name);
                }

                paramField.setFile = FileList[0];
                filePathOutput = paramField.setFile;

                Drop_Label.Content = "Convert";

                var ClickedControl = sender as Control;

                if (ClickedControl.Name == Directory_DropButon.Name)
                    SourcePathLabel.Text = paramField.setFile;




                if (ClickedControl.Name == InputSelector.FilePathBox.Name)
                    InputSelector.FilePathBox.Text = paramField.setFile;



                if (ClickedControl.Name == OutputSelector.Name)
                    InputSelector.FilePathBox.Text = paramField.setFile;


                if (ClickedControl.Name == InputSelector.Name + ParamField.ButtonNameField._openButton)
                    InputSelector.FilePathBox.Text = paramField.setFile;



                harua_View.SourcePathText = paramField.setFile;


                displayMediaInfo(paramField.setFile);

            }

        }


        /// <summary>
        ///  Reflect ffmpg command line Log to logWindows
        /// </summary>
        /// <param name="executeName"></param>




        private void Button_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.All;

            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;
        }








        private void TabItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (((TabItem)sender).Name == "ParameterTab")
            {
                Height = 450;
            }
            else
            {
                Height = 330;
            }

        }



        private void mainTub_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

            if (mainTub.ActualHeight > 0 && mainTub.ActualWidth > 0)
            {
                Height = 330;
                Width = 550;
            }
        }


        public void displayMediaInfo(string setFile)
        {

            try
            {
                if (string.IsNullOrEmpty(paramField.setFile))
                { return; }

                ////InstanceProcessAlreadyExitedException 対策　効果不明
                var ffprove_Process = Process.GetProcessesByName("ffprobe.exe");

                if (ffprove_Process.Length > 0)
                {
                    ffprove_Process[0].Kill();
                }


                

                SorceFileDataBox.Document.Blocks.Clear();
                
                
                FFOptions probe = new FFOptions();
                probe.BinaryFolder = "dll";


                var mediaInfo = FFProbe.Analyse(paramField.setFile,probe);




                var resultBitRate = Math.Truncate(mediaInfo.PrimaryVideoStream.BitRate * 0.001);
                var resultAudioBitRate = Math.Truncate(mediaInfo.PrimaryAudioStream.BitRate * 0.001);
                var resultCodec = mediaInfo.PrimaryVideoStream.CodecLongName;
                var resultAudioCodec = mediaInfo.PrimaryAudioStream.CodecLongName;
                var resultCannels = mediaInfo.PrimaryAudioStream.Channels;

                SorceFileDataBox.AppendText("BitRate:" + $"{resultBitRate}" + "Kbps");
                SorceFileDataBox.AppendText(Environment.NewLine);
                SorceFileDataBox.AppendText("AudioBitRate:" + $"{resultAudioBitRate}" + "Kbps");
                SorceFileDataBox.AppendText(Environment.NewLine);
                SorceFileDataBox.AppendText("Codec:" + $"{resultCodec}");
                SorceFileDataBox.AppendText(Environment.NewLine);
                SorceFileDataBox.AppendText("AudioCodec:" + $"{resultAudioCodec}");
                SorceFileDataBox.AppendText(Environment.NewLine);
                SorceFileDataBox.AppendText("Cannels:" + $"{resultCannels}");




                //明示的GC呼び出し
                //Call explicit GC
                GC.Collect();
            }
            catch(FFMpegCore.Exceptions.FFMpegException ex)
            {
                MessageBox.Show(ex.Message);    

            }

            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + "Stream infomation Empty");
                SorceFileDataBox.Document.Blocks.Clear();

            }
            catch (FFMpegCore.Exceptions.FormatNullException ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + "Fomata Data Empty");
                SorceFileDataBox.Document.Blocks.Clear();
            }
            catch (Instances.Exceptions.InstanceProcessAlreadyExitedException ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine);
            }

            catch (Win32Exception ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + "ffprobe.exeがないわよ");
            }
        }


        private void Num_5_Initialized(object sender, EventArgs e)
        {
            //     Num_5.Height = Num_3.Height;
        }



        //string destinationPath { get; set; }

        private void OutputButton_Checked(object sender, RoutedEventArgs e)
        {

            ClassShearingMenbers.ButtonName = ((RadioButton)sender).Name;

            cod = new CommonOpenDialogClass(true, ParamField.MainTab_OutputDirectory);

            var result = cod.CommonOpens();



            if (result == CommonFileDialogResult.Ok && !string.IsNullOrEmpty(cod.opFileName))
            {
                OutputPathText.Text = cod.opFileName;

                harua_View.OutputPath = cod.opFileName;


                //Update MainTab_OutputDirectory
                ParamField.MainTab_OutputDirectory = cod.opFileName;

            }
            mainGrid.Height += 30;
        }

        private delegate void ProcessKill_deligate(int targetProcess);



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

                //ParaSelectGroup.Background.Opacity = 0.2;

                #region
                //var highLightColor = ConfigManager.Instance.HighlightColor;
                //var mediaColor = System.Windows.Media.Color.


                // var mediaColor = System.Windows.Media.Color.FromRgb(255, 238, 236);

                //itemsRectangle.Items.Add(mediaColor);

                ////mainSelectGroup.IsEnabled = false;
                ////xStack.IsEnabled = false;
                ////ConvertStuck.IsEnabled = false;

                ////xGruoup.IsEnabled = true;


                // xGroup.Background.Opacity = 0.2;


                //
                //ImageBrush transformImageBrush = new ImageBrush();
                //transformImageBrush.ImageSource =
                //    new BitmapImage(new Uri(@"m", UriKind.Relative));

                //myBrush.Opacity = 0.2;

                //  mainTub.Background = transformImageBrush;
                //mainTub.Background = Color.Transparent;
                //mainSelectGroup.Background = new SolidColorBrush(mediaColor);

                //xStack.Background = new SolidColorBrush(mediaColor);
                //ConvertStuck.Background = new SolidColorBrush(mediaColor);


                #endregion

            }
            else
            {
                #region
                //ConvertStuck.IsEnabled = true;
                //mainSelectGroup.IsEnabled = true;
                //xStack.IsEnabled = true;
                //xGruoup.IsEnabled = true;

                //ConbertStack.Background.Opacity = 0.2;
                //mainSelectGroup.Background.Opacity = 0.2;
                #endregion



                // ParaSelectGroup.Background.Opacity = 1;

                ParaSelectGroup.IsEnabled = false;
                ExecButton.IsEnabled = false;


                ParaSelectGroup.Background = new SolidColorBrush(mediaColor);

            }            //    if (mainSelectGroup.Background != null)
                         //    {
                         //        mainSelectGroup.Background.Opacity = 0.2;
                         //        xStack.Background.Opacity = 0.2;
                         //        ConvertStuck.Background.Opacity = 0.2;
                         //    }
                         //}
        }


        private void isUserOriginalParameter_Checked(object sender, RoutedEventArgs e)
        {
            isUseOriginalCheckProc(isUserParameter.IsChecked.Value);

            if (ParaSelectGroup.Background != null)
                ParaSelectGroup.Background.Opacity =
                isUserParameter.IsChecked.Value ? 0 : 1;
        }


        void ParamSave_Procedure()
        {

            int i = 0;

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
                    IniDefinition.SetValue(paramField.iniPath, "CheckState", ParamField.ControlField.ParamSelector + "_Check", radioCount);

                }
            }


            {



                var checkedSet = new IniGetSetValueClass.CheckboxGetSetValueClass();
                foreach (CheckBox chk in childCheckBoxList)
                {

                    checkedSet.CheckediniSetVallue(chk, paramField.iniPath);
                }


                var setWriter = new IniSettings_IOClass();
                setWriter.IniSettingWriter(paramField, this);

                if (!string.IsNullOrEmpty(ParamText.Text))
                    IniDefinition.SetValue(paramField.iniPath, QueryNames.ffmpegQuery, QueryNames.BaseQuery, ParamText.Text);

                if (!string.IsNullOrEmpty(endStringBox.Text))
                    IniDefinition.SetValue(paramField.iniPath, QueryNames.ffmpegQuery, QueryNames.endStrings　, endStringBox.Text);



            }
        }


        private void Window_Closed(object sender, EventArgs e)
        {

            ParamSave_Procedure();

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
                  //  string _fileName = OutputSelector.FilePathBox.Text = param.ConvertFileName(InputSelector.FilePathBox.Text);

                    ParamField.ParamTab_InputSelectorDirectory = Path.GetDirectoryName(ofc.opFileName);
                }
                if (ansest.Name == OutputSelector.Name)
                {
                    ParamField.ParamTab_OutputSelectorDirectory = ofc.opFileName;
                }
            }


        }

     
        public string outputFileName { get; set; }








        private void RotateOption_Checked(object sender, RoutedEventArgs e)
        {
            var radio = sender as RadioButton;



            //RadioButtonをクリックしたときにfalseを再設定する
            switch (radio.Name)
            {
                case "NoRotate":
                    ChekOptionStruct.isNoRotate = true;
                    ChekOptionStruct.isRightRotate = false;
                    ChekOptionStruct.isLeftRotate = false;
                    ChekOptionStruct.isHorizontalRotate = false;
                    break;

                case "Right_Rotate":
                    ChekOptionStruct.isNoRotate = false;
                    ChekOptionStruct.isRightRotate = true;
                    ChekOptionStruct.isLeftRotate = false;
                    ChekOptionStruct.isHorizontalRotate = false;
                    break;

                case "Left_Rotate":
                    ChekOptionStruct.isNoRotate = false;
                    ChekOptionStruct.isRightRotate = false;
                    ChekOptionStruct.isLeftRotate = true;
                    ChekOptionStruct.isHorizontalRotate = false;
                    break;

                case "Horizon_Rotate":
                    ChekOptionStruct.isNoRotate = false;
                    ChekOptionStruct.isRightRotate = false;
                    ChekOptionStruct.isLeftRotate = false;
                    ChekOptionStruct.isHorizontalRotate = true;
                    break;
            }


            if (!ChekOptionStruct.isNoRotate)
            { _arguments = _arguments.Replace(" -metadata:s:v:0 rotate=0 ", ""); }

            else if (!ChekOptionStruct.isRightRotate)
            {
                _arguments = _arguments.Replace(" -vf transpose=1 ", "");
            }

            else if (!ChekOptionStruct.isLeftRotate)
            {
                _arguments = _arguments.Replace(" -vf transpose=2 ", "");
            }
            else if (!ChekOptionStruct.isHorizontalRotate)
            {
                _arguments = _arguments.Replace(" -vf transpose=3 ", "");
            }

        }

        bool isForceExec;
        private void isForceExec_Checked(object sender, RoutedEventArgs e)
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



        IMainTabEvents[] mainTabEvents;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            //get Interface instance
           mainTabEvents = new IMainTabEvents[]
            {

                new Directory_ClickProcedure(this)
            };                ;
            //var dclicks = new Directory_ClickProcedure(this);

            
            //get Button name from label template
            Button dropbutton = (Button)Drop_Label.Template.FindName(ButtonNameField.Convert_DropButton, Drop_Label);

            dropbutton.Click += DropButton_ClickHandle;

            


            Directory_DropButon.Click += DropButton_ClickHandle;
               
                

            
           

            AtacchStringsList.Items.Add("[]");
            AtacchStringsList.Items.Add("{}");
            AtacchStringsList.Items.Add("<>");
        }

        private void DropButton_ClickHandle(object sender, RoutedEventArgs e)
        {
            
            foreach (var button in mainTabEvents)
            {

                if (((Button)sender).Name == nameof(Directory_DropButon))
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

        private void Param_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void LinkLabel2_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {

        }

        private void AtacchStringsList_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void isOpenFolder_Checked(object sender, RoutedEventArgs e)
        {
            paramField.isOpenFolder = (bool)IsOpenForuderChecker.IsChecked ? true : false;
            
        
        }
    }

}



