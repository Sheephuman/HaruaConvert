using HaruaConvert.mainUI.QueryCreateWindow.GetCodecs;
using HaruaConvert.mainUI.QueryCreateWindow.ViewModel;
using HaruaConvert.QueryBuilder;
using HaruaConvert.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static HaruaConvert.IniCreate;
using static HaruaConvert.Parameter.ParamField;


namespace HaruaConvert.userintarface
{
    /// <summary>
    /// QueryCreateWindow.xaml の相互作用ロジック
    /// </summary>
    /// 



    public partial class QueryCreateWindow : Window
    {
        //readonly int minValue = 100;
        MultiValueConverter converter;

       public QueryWindowViewModel _queryviewModel { get; set; }
        //  public static TextBlock queryPreview { get; set; }

        private QueryField qf;
        public static QueryCreateWindow qc { get; set; }

        MainWindow main;


        public QueryCreateWindow(MainWindow _main)
        {


            main = _main;

            qc = this;
            InitializeComponent();
            converter = new MultiValueConverter();

            var getCoudecs = new GetCodecsName();

            var codec = new CodecTypeManager();
            qf = new QueryField();
            _queryviewModel = new QueryWindowViewModel(qf);
         

            var codecsToFindVideo = new List<string>
{
    "av1",
    "libsvtav1",
    "mpeg4",
"hevc",
"libx265",
"libx264",
"dvvideo",
"h264",
"jpeg2000",
"vp9",
"libvpx-vp9",
"prores",
"dnxhd",
"xvid",
"divx",
"h263",
"mjpeg",
"cineform",
"avc-intra",
"ffv1",
"huffyuv",
"rawvideo",
"Indeo",
"msvideo"
};

            var codecsToFindAudio = new List<string>
{

 //追加された圧縮率の高いコーデック
"flac",      // 可逆音声コーデック
"aac",       // 高効率音声コーデック
"mp3",       // 一般的な音声コーデック
"wavpack",    // 可逆音声コーデック
"opus"      // 高効率音声コーデック
};


            //Read ffmpeg Codec Dictionary


            qf.FfmpegVideoCodecDic = getCoudecs.GetCodecNameExecute(codec.typeVideo, codecsToFindVideo);


            qf.FfmpegAudioCodecDic = getCoudecs.GetCodecNameExecute(codec.typeAudio, codecsToFindAudio);

            //foreach (var cocec in FfmpegAudioCodecDic)
            //{ Debug.WriteLine(cocec); }


            FileNameExtentionBox.Items.Add(".mp4");
            FileNameExtentionBox.Items.Add(".avi");
            FileNameExtentionBox.Items.Add(".gif");
            FileNameExtentionBox.Items.Add(".wmv");
            FileNameExtentionBox.Items.Add(".mov");
            FileNameExtentionBox.Items.Add(".mkv");
            FileNameExtentionBox.Items.Add(".flv");
            FileNameExtentionBox.Items.Add(".webm");
            FileNameExtentionBox.Items.Add(".mpeg");
            FileNameExtentionBox.Items.Add(".rmvb");


            var numanager = new NumericUpDownManager(BitRateNumBox);
            var queryBuidUpdown = new QueryCreateUpDown();




            BitRateNumBox.PreviewMouseWheel += queryBuidUpdown.NUDTextBox_PreviewMouseWheel;
            BitRateNumBox.PreviewKeyUp += queryBuidUpdown.NUDButtonUP_Click;
            BitRateNumBox.PreviewKeyDown += queryBuidUpdown.NUDButtonDown_Click;

            NUDButtonUP.Click += queryBuidUpdown.NUDButtonUP_Click;
            NUDButtonDown.Click += queryBuidUpdown.NUDButtonDown_Click;

            DataContext = qf;


            MouseLeftButtonDown += (sender, e) => { DragMove(); };


            qf.BitRateInput = "100";
        }







        private void isBitrateCheckBox_Checked(object sender, RoutedEventArgs e)
        {


            if (!converter.isVideoCodec)
            {
                converter.IsBitrateChecked = true;
            }
            else
                converter.IsBitrateChecked = false;

            if (!string.IsNullOrEmpty(BitRateNumBox.Text))
                qf.UpdateAllInput(FileNameExtentionBox.Text);
        }


        private void EnableVideoCodecChecker_Checked(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(VideoCodecBox.Text))
                qf.UpdateAllInput(FileNameExtentionBox.Text);

            if (EnableVideoCodecChecker.IsChecked == true)
            {
                converter.isVideoCodec = true;
                enablePostTwitterChecker.IsEnabled = false;



            }
            else
            {
                converter.isVideoCodec = false;
                enablePostTwitterChecker.IsEnabled = true;
            }


        }

        private void enablePostTwitterChecker_Checked(object sender, RoutedEventArgs e)
        {
            //  qf.VideoCodecStrings = VideoCodecBox.SelectedValue.ToString();

            EnableVideoCodecChecker.IsEnabled = enablePostTwitterChecker.IsChecked == true ? false : true;
            qf.UpdateAllInput(FileNameExtentionBox.Text);


        }

      

      
        private void AudioCodecBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            if (AudioCodecBox.SelectedItem != null)
            {

                qf.AudioCodecStrings = AudioCodecBox.SelectedValue.ToString();

                qf.UpdateAllInput(FileNameExtentionBox.Text);
            }


        }


        private void testLabel_datacontextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }


        private void AudioCodecChecker_Checked(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(AudioCodecBox.Text))
                qf.UpdateAllInput(FileNameExtentionBox.Text);
        }

        private void OtherFileNameChecker_Checked(object sender, RoutedEventArgs e)
        {
            
            if (FileNameExtentionBox.SelectedValue != null)
                qf.UpdateAllInput(FileNameExtentionBox.Text);
        }

        private void FileNameExtentionBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            qf.UpdateAllInput(FileNameExtentionBox.Text);
            if (FileNameExtentionBox.SelectedItem is null)
                return;


            if (FileNameExtentionBox.SelectedItem.ToString() == ".mp4")
            {

                qf.FfmpegVideoCacheDic = new Dictionary<string, string>();
                //FileNameExtentionBox.ItemsSource =

              //  VideoCodecBox.ItemsSource = null;

                List<string> videoBindingList = new()
                {
                    "libx264",
                    "libx265",
                    "mpeg4",
                    "vp9",
                    "av1",
                    "webm"
                };


                Dictionary<string, string> AudioBinding = new();
                foreach (string token in videoBindingList)
                {
                    var filtered = qf.FfmpegVideoCodecDic
                        .Where(x => x.Key.Contains(token))
                        .ToDictionary(ktoken => ktoken.Key, ktoken => ktoken.Value);

                    foreach (var kv in filtered)
                    {
                        if (kv.Key.Contains("librav1e")) //.mkv
                            continue;

                        if (kv.Key.Contains("msmpeg4")) //.wmv
                            continue;

                        AudioBinding[kv.Key] = kv.Value; // 重複キーは上書き
                    }
                }


                VideoCodecBox.ItemsSource = AudioBinding;


            }
            else
            {

                VideoCodecBox.ItemsSource = qf.FfmpegVideoCodecDic;
            }
        }

        private void QueryApplayButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(PreviewBlock.Text))
            {
                main.ParamText.Text = PreviewBlock.Text;

                var combo = main.ParamText;
                if (!combo.Items.Contains(combo.Text))
                    combo.Items.Add(combo.Text);

                MessageBox.Show("Queryが正常に適用されましたましたわ");

                main.Activate();
            }
            //メインウィンドウのBinding先がMainParamクラスであるため、
            //その中の変数に直接アクセスしてもBindingに反映されない
        }

        private void MakeProfile_Click(object sender, RoutedEventArgs e)
        {

            if (!string.IsNullOrEmpty(profileNameBox.Text))
            {
                IniDefinition.SetValue(main.paramField.profileQueryIni, QueryNames.profileName,
                    profileNameBox.Text, PreviewBlock.Text);
                MessageBox.Show("プロファイルが正常に作成されました");
            }

            else
                MessageBox.Show("プロファイル名を設定してくださいね");



            //IniDefinition.SetValue(paramField.iniPath, QueryNames.ffmpegQuery, QueryNames.BaseQuery, ParamText.Text);
        }

        private void SendClipBoard_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(PreviewBlock.Text))
            {
                Clipboard.SetText(PreviewBlock.Text);

                MessageBox.Show("queryをコピーしました");
            }
            else
                MessageBox.Show("queryが空です");
        }


        private void QueryCreateWindow_Closed(object sender, EventArgs e)
        {
            main.paramField.isClosedQueryBuildWindow = true;
        }


        private void QueryCreateWindowForm_Loaded(object sender, RoutedEventArgs e)
        {
            main.paramField.isClosedQueryBuildWindow = false;
        }

        private void FileNameExtentionBox_Loaded(object sender, RoutedEventArgs e)
        {
            FileNameExtentionBox.SelectedIndex = 1;
        }
    }
}