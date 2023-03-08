using FFMpegCore;
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
using HaruaConvert.InterFace;
using HaruaConvert.Methods;
using HaruaConvert.UserControls;
using static HaruaConvert.Items.Items;
using System.Windows.Forms.Design;
using WpfApp3;

namespace HaruaConvert
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IHaruaInterFace.IMainWindwEvents,
        IHaruaInterFace.IMouseEvents
    {
        DataContextClass_HaruaConvert DCmenber;



        string setFile { get; set; }

        List<ParamSelector> selectorList;

        bool firstSet { get; set; } //初回起動用
        string baseArguments;
        List<CheckBox> childCheckBoxList;

        string iniPath { get; set; }
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


            Ffmpc = new FfmpegQueryClass(this);

            isUPDownClicked = false;
            ParamInterfase.isAutoScroll = true;


            iniPath = Path.Combine(Environment.CurrentDirectory, "Settings.ini");


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



                Left = IniDefinition.GetValueOrDefault(iniPath, "WindowsLocate", "WindowLeft", 25);

                Top = Convert.ToInt32(IniDefinition.GetValueOrDefault(iniPath, "WindowsLocate", "WindowTop", 50));







                isUseOriginalCheckProc(isUserOriginalParameter.IsChecked.Value);


                var iniCon = new IniSettingsConst();

                ParamInterfase.InputDirectory = IniDefinition.GetValueOrDefault(iniPath, "Directory", IniSettingsConst.ConvertDirectory, "");
                // IniDefinition.SetValue(iniPath, "Directry", "ConvertDirectory", ParamInterfase.ConvertDirectory);


                ParamInterfase.OutputDirectory = IniDefinition.GetValueOrDefault(iniPath, "Directory", IniSettingsConst.OutputDirectory, "");
                ParamInterfase.OutputSelectorDirectory = IniDefinition.GetValueOrDefault(iniPath, "Directory", IniSettingsConst.OutputSelectorDirectory, "");
                ParamInterfase.InputSelectorDirectory = IniDefinition.GetValueOrDefault(iniPath, "Directory", IniSettingsConst.InputSelectorDirectory, "");
                NumericUpDown1.NUDTextBox.Text = IniDefinition.GetValueOrDefault(iniPath, IniSettingsConst.Selector_Generate, IniSettingsConst.Selector_Generate, "1");



                #region SelectParameterBox Generate
                {
                    ////
                    /// SelectParameterBox Generate
                    ///
                    SelGenerate = SelGenerate = int.Parse(NumericUpDown1.NUDTextBox.Text);





                    Generate_ParamSelector();
                    #endregion
                }
            }



            if (firstSet)
                ParamInterfase.isExitProcessed = true;

            mainProcess = Process.GetCurrentProcess();


            //isLabelEited = false;

            Lw = new LogWindow(this);

            {
                FileList = new ObservableCollection<string>();


                ParamInterfase.endFileNameStrings = IniDefinition.GetValueOrDefault(iniPath, "ffmpegQuery", "endStrings", "_Harua");
                //  Set Default Parameter on FfmpegQueryClass
                DCmenber = new DataContextClass_HaruaConvert()
                {
                    StartQuery = IniDefinition.GetValueOrDefault
                                    (iniPath, "ffmpegQuery", "BaseQuery", "  -b:v 1200k -pix_fmt yuv420p -acodec aac -y -threads 2"),

                    OutputPath = ParamInterfase.OutputDirectory,
                    endString = ParamInterfase.endFileNameStrings,
                    SourcePathText = "フォルダ:" + IniDefinition.GetValueOrDefault
                                    (iniPath, "Directory", IniSettingsConst.ConvertDirectory, "Source File")
 ,

                };

                _arguments = DCmenber.StartQuery;

                DataContext = DCmenber;
            }


            #region Register Events
            
            {
                NumericUpDown1.NUDButtonUP.Click += NUDUP_Button_Click;
                NumericUpDown1.NUDButtonDown.Click += NUD_DownButton_Click;
            }


            {

                
                InputSelector.AllowDrop = true;
                InputSelector.FilePathBox.AllowDrop = true;

                InputSelector.openDialogButton.Name = InputSelector.Name  + "_openButton";        
                
             

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

            //AutoButton.Width = xGroup.Width+10;
            //ReffrenceButton.Width = xGroup.Width+10;

            
            ///////
            ////https://qiita.com/tricogimmick/items/4347214669a99cd2c775
            /////
            Loaded += (o, e) =>
            {

                selectorList = new List<ParamSelector>();
                childCheckBoxList = new List<CheckBox>();


                this.WalkInChildren(child =>
                {
                    var cachedCheckbox = child.GetType().Equals(typeof(CheckBox));

                    if (cachedCheckbox)
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

                    chk.IsChecked = init.CheckBoxiniGetVallue(chk, iniPath);
                }


                GenerateSelectParaClass gsp = new GenerateSelectParaClass();
                int count = 0;
                foreach (var selector in selectorList)
                {
                    gsp.GenerateParaSelector_setPropaties(selector, this);

                    //var selchild = selector;

                    //selchild.Name = "ParamSelector" + $"{count}";
                    //count++;
                    //rehgist Event
                    //selchild.Loaded += ParamSelect_Load;
                    //selchild.SlectorRadio.Checked += SlectorRadio_Checked;
                    //selchild.ArgumentEditor.TextChanged += ArgumentEditor_TextChanged;

                    //tb.ArgumentEditor.Loaded += ArgumentEditor_Loaded;

                    //selchild.KeyUp += InvisibleText_KeyDown;
                    //selchild.LostFocus += InvisibleText_LostFocus;
                    //selchild.SelectorLabelCon.MouseDoubleClick += Tb_MouseDoubleClick;
                    //TextCompositionManager.AddPreviewTextInputHandler(selchild.invisibleText, OnPreviewTextInput);
                    //TextCompositionManager.AddPreviewTextInputUpdateHandler(selchild.invisibleText, OnPreviewTextInputUpdate);


                    // tb.invisibleText.TextChanged += invisibleText_DataContextChanged3;

                    SelGenerate = count;
                }


            };

            #endregion





        }


        public void NUDUP_Button_Click(object sender, RoutedEventArgs e)
        {
            isUPDownClicked = true;
            Generatednum = SelGenerate + 1;
            if (SelGenerate != Generatednum)
                SelGenerate = int.Parse(NumericUpDown1.NUDTextBox.Text, CultureInfo.CurrentCulture);

        }



        private void NUD_DownButton_Click(object sender, RoutedEventArgs e)
        {
            isUPDownClicked = true;

            Generatednum = SelGenerate - 1;
            if (SelGenerate != Generatednum)
                SelGenerate = int.Parse(NumericUpDown1.NUDTextBox.Text, CultureInfo.CurrentCulture);

        }


#pragma warning disable CA1707 // 識別子はアンダースコアを含むことはできません
        public void InvisibleText_LostFocus(object sender, RoutedEventArgs e)
#pragma warning restore CA1707 // 識別子はアンダースコアを含むことはできません
        {

            foreach (ParamSelector sel in selectorList)
            {
                var inText = VisualTreeHelperWrapperHelpers.FindDescendant<TextBox_Extend>((ParamSelector)sender);
                if (inText == sel.invisibleText)
                    sel.invisibleText.Visibility = Visibility.Hidden;
                sel.ParamLabel.Visibility = Visibility.Visible;


            }
        }

        private void chk_Loaded(object sender, RoutedEventArgs e)
        {

            var iniSet = new IniGetSetValueClass.CheckboxGetSetValueClass();

        }

        public void OnPreviewTextInputUpdate(object sender, TextCompositionEventArgs e)
        {


            if (e.TextComposition.CompositionText.Length == 0)
            {
                isImeOnConv = false;
            }
            else
            {
                isImeOnConv = true;
            }
        }

        public void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (isImeOnConv)
            {

                EnterKeyBuffer = 1;
            }
            else
            {
                EnterKeyBuffer = 0;
            }
            isImeOnConv = false;
        }



        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:識別子はアンダースコアを含むことはできません", Justification = "<保留中>")]
        public void ArgumentEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            var ansest = VisualTreeHelperWrapperHelpers.FindAncestor<ParamSelector>((TextBox)sender);
            if (ansest == null)
                return;

            else if (ansest.Name.Contains(rcount, StringComparison.Ordinal))
            {
                ParamInterfase.isUsedOriginalArgument = ansest.ArgumentEditor.Text;
            }

        }

        public void ParamSelect_Load(object sender, RoutedEventArgs e)
        {
            /////初回のみ呼ばれるようにする
            firstSet = ParamSelector_SetText(sender, firstSet);
        }




        public void Tb_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var sel = sender as ContentControl;


            var labelBlock = VisualTreeHelperWrapperHelpers.FindDescendant<TextBlock>(sel);

            if (sel != null)
                foreach (ParamSelector sp in selectorList)
                {
                    if (string.IsNullOrEmpty(labelBlock.Text))
                    {
                        return;
                    }

                    if (labelBlock == sp.ParamLabel)
                    {                    //  sp.SelectorLabel.Margin = new Thickness(100, 100, 100, 100);

                        sp.invisibleText.Text = "";
                        sp.invisibleText.Text = labelBlock.Text;
                        labelBlock.Visibility = Visibility.Hidden;
                        sp.invisibleText.Visibility = Visibility.Visible;

                        if (sp.invisibleText.Visibility == Visibility.Visible)
                        {
                            this.Dispatcher.BeginInvoke((Action)delegate
                            {
                                Keyboard.Focus(sp.invisibleText);
                            }, DispatcherPriority.Render);
                        }

                        return;
                    }
                }
        }
        private bool isImeOnConv = false; //IME利用中かどうか判定するフラグ
        private int EnterKeyBuffer { get; set; } //IMEでの変換決定のEnterキーに反応させないためのバッファ


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
            else if (e.Key == Key.Enter && isImeOnConv == false && EnterKeyBuffer == 0)
            {
                //  System.Windows.Forms.ImeMode ime = System.Windows.Forms.ImeMode.On;


                EnterKeyBuffer = 1;

                //if (!isLabelEited)
                //{

                //    isLabelEited = true;
                //    return;

                //}

                foreach (ParamSelector sp in selectorList)
                {
                    //ImeEnabled = InputMethod.GetIsInputMethodEnabled(sp.invisibleText);

                    // if (ImeEnabled)

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


        string rcount;

        bool ParamSelector_SetText(object sender, bool _firstSet)
        {
            int i = 0;
            if (_firstSet)
            {


                foreach (var selector in selectorList)
                {
                    selector.ArgumentEditor.Text = IniDefinition.GetValueOrDefault
                        (iniPath, ParamInterfase.ControlField.ParamSelector + "_" + $"{i}", IniSettingsConst.Arguments_ + $"{i}",
                        "");


                    //selector.ArgumentEditor.Text);

                    selector.ParamLabel.Text = IniDefinition.GetValueOrDefault(iniPath, ParamInterfase.ControlField.ParamSelector + "_" + $"{i}",
                        IniSettingsConst.ParameterLabel+ "_" + $"{i}",
                     "パラメータ名").Replace("\r\n", "", StringComparison.Ordinal);

                    rcount = IniDefinition.GetValueOrDefault(iniPath, "CheckState", ParamInterfase.ControlField.ParamSelector + "_Check", "0");
                   int rcountInt = int.Parse(rcount);




                    i++;
                    
                    if (selector.Name == ParamInterfase.ControlField.ParamSelector + rcount)
                    {
                        selector.SlectorRadio.IsChecked = true;
                        ParamInterfase.isUsedOriginalArgument = selector.ArgumentEditor.Text;
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
                    if (isUserOriginalParameter.IsChecked.Value)
                    {
                        ParamInterfase.isUsedOriginalArgument = rd.ArgumentEditor.Text;
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
            set { if (_OutputfilePath != setFile) { _OutputfilePath = setFile; RaisePropertyChanged(); } }
        }

        //[DllImport("user32.dll", SetLastError = true)]
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

                setFile = FileList[0];
                filePathOutput = setFile;

                Drop_Label.Content = "Convert";

                var ClickedControl = sender as Control; 
                
                if(ClickedControl.Name == Directory_DropButon.Name)
                  SourcePathLabel.Text = setFile;
                
                
              
                
                if (ClickedControl.Name == InputSelector.FilePathBox.Name)
                    InputSelector.FilePathBox.Text = setFile;



                if (ClickedControl.Name == OutputSelector.Name)
                    InputSelector.FilePathBox.Text = setFile;


                if (ClickedControl.Name == InputSelector.Name +  ParamInterfase.ButtonNameField._openButton)
                    InputSelector.FilePathBox.Text = setFile;

               

                DCmenber.SourcePathText = setFile;


                displayMediaInfo(setFile);

            }

        }


        /// <summary>
        ///  Reflect ffmpg command line Log to logWindows
        /// </summary>
        /// <param name="executeName"></param>
        ///// <param name="argument"></param>
        //static void AsyncProcTest(string executeName, string argument)
        //{



        //    var si = new ProcessStartInfo(executeName, argument);
        //    si.RedirectStandardError = true;
        //    si.RedirectStandardOutput = true;
        //    si.UseShellExecute = false;
        //    si.CreateNoWindow = false;

        //    using (var proc = new Process())
        //    using (var ctoken = new CancellationTokenSource())
        //    {
        //        {
        //            proc.EnableRaisingEvents = true;
        //            proc.StartInfo = si;
        //            // コールバックの設定
        //            proc.OutputDataReceived += (sender, ev) =>
        //            {
        //                Console.WriteLine($"stdout={ev.Data}");
        //            };
        //            proc.ErrorDataReceived += (sender, ev) =>
        //            {
        //                Console.WriteLine($"stderr={ev.Data}");
        //            };
        //            proc.Exited += (sender, ev) =>
        //            {
        //                Console.WriteLine($"exited");
        //                // プロセスが終了すると呼ばれる



        //                ctoken.Cancel();
        //            };
        //            // プロセスの開始
        //            proc.Start();
        //            // 非同期出力読出し開始
        //            proc.BeginErrorReadLine();
        //            proc.BeginOutputReadLine();
        //            // 終了まで待つ
        //            ctoken.Token.WaitHandle.WaitOne(30, true);
        //            MessageBox.Show("Exit");
        //        };
        //    };


        //}




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


        private void displayMediaInfo(string setFile)
        {

            try
            {
                if (string.IsNullOrEmpty(setFile))
                { return; }

                ////InstanceProcessAlreadyExitedException 対策　効果不明
                var ffprove_Process = Process.GetProcessesByName("ffprobe.exe");

                if (ffprove_Process.Length > 0)
                {
                    ffprove_Process[0].Kill();
                }



                SorceFileDataBox.Document.Blocks.Clear();

                var mediaInfo = FFProbe.Analyse(setFile);
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
        }

        public void Directory_DropButon_Click(object sender, RoutedEventArgs e)
        {

            //  ParamInterfase.identification_Obj = sender;
            ParamInterfase.ButtonName = ((Button)sender).Name;




            if (!ParamInterfase.isExitProcessed && !isForceExec)
            {
                MessageBox.Show("ffmpeg.exeが実行中です");

                return;
            }



            using (CommonOpenDialogClass ofc = new CommonOpenDialogClass(false, ParamInterfase.InputDirectory))
            {

                var result = ofc.CommonOpens();






                if (result == CommonFileDialogResult.Ok)  //Selected OK
                {


                    setFile = ofc.opFileName;
                    DCmenber.SourcePathText = setFile;
                    SourcePathLabel.Text = setFile;


                    // ParamInterfase.ConvertDirectory = setFile;

                    Drop_Label.Content = "Convert";

                    ParamInterfase.InputDirectory = Path.GetDirectoryName(setFile);


                    //Update InputDirectory
                    ParamInterfase.InputDirectory = Path.GetDirectoryName(ofc.opFileName);


                }
                displayMediaInfo(setFile);
                //  ParamInterfase.ConvertDirectory = ofc.opFileName;
            }

        }
        private void Convert_DropButton_Click(object sender, RoutedEventArgs e)
        {

            if (!ParamInterfase.isExitProcessed && !isForceExec)
            {
                MessageBox.Show("ffmpwg.exeが実行中ですわ");
                return;
            }



            ParamInterfase.ButtonName = ((Button)sender).Name;
            //var runinng = Process.GetProcessesByName("ffmpeg.exe");
            if (!string.IsNullOrEmpty(setFile))
            {
                //Convert Process Improvement Part
                ParamInterfase.isExitProcessed = FileConvertExec(setFile);



                Lw.Activate();

            }
            else
            {

                Directory_DropButon_Click(sender, e);
                DCmenber.SourcePathText = setFile;

            }




        }


        private void Num_5_Initialized(object sender, EventArgs e)
        {
            //     Num_5.Height = Num_3.Height;
        }



        //string destinationPath { get; set; }

        private void OutputButton_Checked(object sender, RoutedEventArgs e)
        {

            ParamInterfase.ButtonName = ((RadioButton)sender).Name;

            var cod = new CommonOpenDialogClass(true, ParamInterfase.OutputDirectory);

            var result = cod.CommonOpens();



            if (result == CommonFileDialogResult.Ok && !string.IsNullOrEmpty(cod.opFileName))
            {
                OutputPathText.Text = cod.opFileName;

                DCmenber.OutputPath = cod.opFileName;






                //Update OutputDirectory
                ParamInterfase.OutputDirectory = Path.GetDirectoryName(cod.opFileName);

            }
            mainGrid.Height += 30;
        }

        private delegate void ProcessKill_deligate(int targetProcess);


        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Closeだけでは確実にプロセスが終了されない
                Lw.Close();
                Close();

                using (var tpc = new Terminate_ProcessClass())
                {
                    ProcessKill_deligate killProcessDell = tpc.Terminate_Process;
                    GC.Collect();


                    //ffmpegの強制終了
                    if (th1 != null)
                    {
                        killProcessDell(ParamInterfase.ffmpeg_pid);
                    }

                    //main Processが正常に終了されていない場合の対策
                    if (mainProcess != null)
                    {
                        killProcessDell(mainProcess.Id);
                    }
                }
                Environment.Exit(0);




            }

            catch (TaskCanceledException ex)
            {
                MessageBox.Show(ex.Message);
            }

            catch (System.ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }

            //When Close() Only is not shutdown
        }

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

            DCmenber.OutputPath = "";
            OutputPathText.Text = "";
            ParamInterfase.OutputDirectory = "";
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
                //    new BitmapImage(new Uri(@"harua.jpg", UriKind.Relative));

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
            isUseOriginalCheckProc(isUserOriginalParameter.IsChecked.Value);

            if (ParaSelectGroup.Background != null)
                ParaSelectGroup.Background.Opacity =
                isUserOriginalParameter.IsChecked.Value ? 0 : 1;
        }





        private void Window_Closed(object sender, EventArgs e)
        {



            int i = 0;

            //Add Number and Save setting.ini evey selector 
            foreach (var selector in selectorList)
            {

                IniDefinition.SetValue(iniPath, ParamInterfase.ControlField.ParamSelector +"_"+ $"{i}" , "Arguments_" + $"{i}",
                    selector.ArgumentEditor.Text);

                IniDefinition.SetValue(iniPath, ParamInterfase.ControlField.ParamSelector + "_" + $"{i}", IniSettingsConst.ParameterLabel +"_" + $"{i}",
                    selector.ParamLabel.Text);

                i++;



                //if Check Selector Radio, Save Check State
                if (selector.SlectorRadio.IsChecked.Value)
                {
                    var radioCount = selector.Name.Remove(0,ParamInterfase.ControlField.ParamSelector.Length);
                    IniDefinition.SetValue(iniPath, "CheckState", ParamInterfase.ControlField.ParamSelector + "_Check", radioCount);

                }
            }


            {



                var checkedSet = new IniGetSetValueClass.CheckboxGetSetValueClass();
                foreach (CheckBox chk in childCheckBoxList)
                {

                    checkedSet.CheckediniSetVallue(chk, iniPath);
                }



                //WIndow Size
                IniDefinition.SetValue(iniPath, "WindowsLocate", "WindowLeft", Convert.ToString(Left));
                IniDefinition.SetValue(iniPath, "WindowsLocate", "WindowTop", Convert.ToString(Top));

                //FileOpenDialog Init Path
                IniDefinition.SetValue(iniPath, "Directory", IniSettingsConst.ConvertDirectory , ParamInterfase.InputDirectory);
                IniDefinition.SetValue(iniPath, "Directory", IniSettingsConst.OutputDirectory, ParamInterfase.OutputDirectory);
                IniDefinition.SetValue(iniPath, "Directory", IniSettingsConst.OutputSelectorDirectory, ParamInterfase.OutputSelectorDirectory);
                IniDefinition.SetValue(iniPath, "Directory", IniSettingsConst.InputSelectorDirectory, ParamInterfase.InputSelectorDirectory);

                //Save Generated Number
                IniDefinition.SetValue(iniPath, IniSettingsConst.Selector_Generate, IniSettingsConst.Selector_Generate, NumericUpDown1.NUDTextBox.Text);



                if (!string.IsNullOrEmpty(ParamText.Text))
                    IniDefinition.SetValue(iniPath, "ffmpegQuery", "BaseQuery", ParamText.Text);
                
                if (!string.IsNullOrEmpty(endStringBox.Text))
                    IniDefinition.SetValue(iniPath, "ffmpegQuery", "endStrings", endStringBox.Text);


            }


        }
        private void Window_StateChanged(object sender, EventArgs e)
        {

            if (this.WindowState == WindowState.Maximized)
                this.WindowState = WindowState.Normal;
        }

        private void OriginalParamExecButton_Click(object sender, RoutedEventArgs e)
        {
            ParamInterfase.ButtonName = ((Button)sender).Name;

            if (!ParamInterfase.isExitProcessed && !isForceExec)
            {
                MessageBox.Show("ffmpwg.exeが実行中ですわ");

                return;
            }
            //early return
            else if (string.IsNullOrEmpty(ParamInterfase.isUsedOriginalArgument))
            {
                MessageBox.Show("ユーザーパラメータが空ですわ");
                return;
            }
            FileConvertExec(setFile);
        }


        static bool isEmptyInputter;
        static bool _isEmptyInputter
        {
            get { return isEmptyInputter; }
            set { isEmptyInputter = true; }
        }


        

        internal FfmpegQueryClass Ffmpc { get; set; }

        public void FileSelector_MouseDown(object sender, RoutedEventArgs e)
        {

            //if (((FileSelector)sender).Name == "InputSelector")
            //((Button)sender).FindAncestor<DependencyObject>() ;



            FileSelector ansest;
            //Fined Parent Element 
            ansest = VisualTreeHelperWrapperHelpers.FindAncestor<FileSelector>((Button)sender);

            ParamInterfase.ButtonName = ansest.Name;
            



            CommonFileDialogResult res = ansest.Name == "InputSelector" ?
                 Selector_ComonOpenMethod(false, ansest) : Selector_ComonOpenMethod(true, ansest);
            //三項演算子




            var param = new ParamCreateClasss(InputSelector.FilePathBox.Text);

            if (res == CommonFileDialogResult.Ok)
            {
                if (ansest.Name == InputSelector.Name)
                {
                    string _fileName = OutputSelector.FilePathBox.Text = param.ConvertFileName(InputSelector.FilePathBox.Text);

                    ParamInterfase.InputSelectorDirectory =  InputSelector.FilePathBox.Text;
                }
                if (ansest.Name == OutputSelector.Name)
                {
                    ParamInterfase.OutputSelectorDirectory = OutputSelector.FilePathBox.Text;
                }
            }

            
        }


#pragma warning disable CA1822 // メンバーを static に設定します
        CommonFileDialogResult Selector_ComonOpenMethod(bool isFolder, FileSelector selector)
#pragma warning restore CA1822 // メンバーを static に設定します
        {

            ParamInterfase.ButtonName = selector.Name;


            if(selector.Name == ParamInterfase.ControlField.InputSelector)
            {
                ParamInterfase.InitialDirectory = string.Empty;
                ParamInterfase.InitialDirectory = Path.GetDirectoryName(ParamInterfase.InputSelectorDirectory);
            }
            else if(selector.Name == ParamInterfase.ControlField.OutputSelector)
            {
                ParamInterfase.InitialDirectory = string.Empty;
                ParamInterfase.InitialDirectory = Path.GetDirectoryName(ParamInterfase.OutputSelectorDirectory);
            }

            var ofc = new CommonOpenDialogClass(isFolder, ParamInterfase.InitialDirectory);
            
            var commons = ofc.CommonOpens();



            if (commons == CommonFileDialogResult.Cancel)
                return commons;




            if (isFolder) //Clicked OutputSelector 
            {


                //Update OutputSelectorDirectory
                ParamInterfase.OutputSelectorDirectory = Path.GetDirectoryName(ofc.opFileName);




                selector.FilePathBox.Text = ofc.opFileName+ "\\" + DateTime.Now + ".mp4";
            }
            else if(!isFolder) //Clicked InputSelector 
            {
                //Update inputSelectorDirectory
                ParamInterfase.InputFileName = Path.GetDirectoryName(ofc.opFileName);


                selector.FilePathBox.Text = ofc.opFileName;
             
                return commons;
            }





            return commons;
        }




        private void Param_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (!string.IsNullOrEmpty(ParamText.Text))
            //    FfmpegQueryClass.ffmpegQuery = ParamText.Text;
        }



        //static async void ExpandDelay()
        //{         
        //    await Task.Delay(6000).ConfigureAwait(true);

        //}

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
            { _arguments.Replace(" -metadata:s:v:0 rotate=0 ", ""); }

            if (!ChekOptionStruct.isRightRotate)
            {
                _arguments.Replace(" -vf transpose=1 ", "");
            }

            if (!ChekOptionStruct.isLeftRotate)
            {
                _arguments.Replace(" -vf transpose=2 ", "");
            }
            if (!ChekOptionStruct.isHorizontalRotate)
            {
                _arguments.Replace(" -vf transpose=3 ", "");
            }

        }

        private void isUserOriginalParameter_Checked_1(object sender, RoutedEventArgs e)
        {

        }

        private void isUserOriginalParameter_Unchecked(object sender, RoutedEventArgs e)
        {

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

        private void LinkLabel_RequestNavigate_1(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {

        }

        private void LinkLabel2_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {

        }


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

                ParamSelector sbx = new ParamSelector()
                {
                    Margin = marthick,
                    FontSize = 14,
                    Width = 480,
                    Name = "ParamSelector" + $"{count++}"
                };

                sbx.ParamLabel.Margin = new Thickness(15, 0, 30, 0);
                sbx.ArgumentEditor.Width = 380;
                sbx.ParamLabel.Width = 110;



                if (!SelectorStack.Children.Contains(sbx))
                {
                    SelectorStack.Children.Add(sbx);
                    selectorList.Add(sbx);
                }

            }
            SelGenerate = count;



            GenerateSelectParaClass gsp = new GenerateSelectParaClass();
            foreach (var selector in selectorList)
            {
                gsp.GenerateParaSelector_setPropaties(selector, this);
            }


        }

        int Generatednum { get; set; }
        private void ButtonGenerate_Click(object sender, RoutedEventArgs e)
        {
            // Generatednum = int.Parse(NumericUpDown1.NUDTextBox.Text, CultureInfo.CurrentCulture);

            if (!isUPDownClicked)
            { return;  }


            Generate_ParamSelector();
           
                ParamSelector_SetText(sender,true);
            isUPDownClicked = false;
            }

        
        private void AtacchStringsList_Loaded(object sender, RoutedEventArgs e)
        {


        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            AtacchStringsList.Items.Add("[]");
            AtacchStringsList.Items.Add("{}");
            AtacchStringsList.Items.Add("<>");
        }

        
        public string AddParam(string locatePath)
        {
            throw new NotImplementedException();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        IHaruaInterFace.IMethods IMainWindwEvents;
        private void testButton_Click(object sender, RoutedEventArgs e)
        {
            IMainWindwEvents = new CommonOpenDialogClass(false, ParamInterfase.InitialDirectory);
            var test = new testForm(IMainWindwEvents);
            test.Show();
        }

        private void InputSelector_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy; // マウスカーソルをコピーにする。
            e.Handled = e.Data.GetDataPresent(DataFormats.FileDrop);
            // ドラッグされてきたものがFileDrop形式の場合だけ、このイベントを処理済みにする。
        }
    }

}



