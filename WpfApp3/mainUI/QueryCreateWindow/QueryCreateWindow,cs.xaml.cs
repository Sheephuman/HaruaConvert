using HaruaConvert.Parameter;
using HaruaConvert.QueryBuilder;
using HaruaConvert.mainUI.QueryCreateWindow;
using HaruaConvert.mainUI.QueryCreateWindow.GetCodecs;
using HaruaConvert.UserControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using static HaruaConvert.Parameter.ParamField;
using static System.Resources.ResXFileRef;
using System.DirectoryServices.ActiveDirectory;


namespace HaruaConvert.userintarface
{
    /// <summary>
    /// QueryCreateWindow.xaml の相互作用ロジック
    /// </summary>
    /// 
    
  

    public partial class QueryCreateWindow : Window
    {
        //  readonly int minValue = 500;
        MultiValueConverter converter;
      
            
     //  public static TextBlock queryPreview { get; set; }

        private QueryField qf;
        public static QueryCreateWindow qc { get; set; }

        MainWindow main;
        QueryCreateWindow qi;
        public QueryCreateWindow(MainWindow _main)
        {
           


            main = _main;
            
            qc = this;
            InitializeComponent();
            converter = new MultiValueConverter();

            var getCoudecs = new GetCodecsName();

            var codec = new QueryBuilder.CodecTypeManager();

            qf = new QueryField();

            qf.FfmpegVideoCodecDic = getCoudecs.GetCodecNameExecute(codec.typeVideo);

             qf.FfmpegAudioCodecDic = getCoudecs.GetCodecNameExecute(codec.typeAudio);

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
        }

       


      


        private void isBitrateCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            
         
            if (!converter.isVideoCodec)
            {
                converter.IsBitrateChecked = true;
            }
            else
                converter.IsBitrateChecked = false;

            if(!string.IsNullOrEmpty(BitRateNumBox.Text))
                 qf.UpdateAllInput();
        }

      
        private void EnableVideoCodecChecker_Checked(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(VideoCodecBox.Text))
                qf.UpdateAllInput();

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
            qf.UpdateAllInput();


            //-codec:v h264 -vf yadif=0:-1:1
            //if (enablePostTwitterChecker.IsChecked == true)
            //{
            //    converter.isVideoCodec = true;


            //}
            //else
            //{

            //    EnableVideoCodecChecker.IsEnabled = true;
            //}
        }

        private void VideoCodecBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

           
            if (VideoCodecBox.SelectedItem != null)
            {             
              
                qf.VideoCodecStrings = indexMake(VideoCodecBox);

                qf.UpdateAllInput();
            }
                          
            
        }


        string indexMake(ComboBox CodecBox) {
            string removeText = string.Empty;
            
            
            removeText = CodecBox.SelectedItem.ToString();

            

            var result = removeText.Replace("[", "").Replace("]", "");


            //if (!string.IsNullOrEmpty(result))
            //   qf.VideoCodecStrings = result;
         

            return result;
        
        }

        //private void SelectItemBySubstring(ComboBox comboBox, string substring)
        //{
        //    foreach (var item in comboBox.Items)
        //    {
        //        if (item.ToString().Contains(substring))
        //        {
        //            comboBox.SelectedItem = item;
        //            break; // 目的のアイテムが見つかったらループを終了
        //        }
        //    }
        //}

        private void AudioCodecBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (AudioCodecBox.SelectedItem != null)
            {

                qf.AudioCodecStrings = indexMake(AudioCodecBox);

                qf.UpdateAllInput();
            }

            
        }

        private void VideoCodecBox_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void testLabel_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

    
        private void AudioCodecChecker_Checked(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(AudioCodecBox.Text))
                qf.UpdateAllInput();
        }

        private void OtherFileNameChecker_Checked(object sender, RoutedEventArgs e)
        {
            if (FileNameExtentionBox.SelectedValue != null)
                qf.UpdateAllInput();
        }

        private void FileNameExtentionBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            qf.UpdateAllInput();
        }

        private void QueryBuidButton_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(PreviewBlock.Text))
               main.ParamText.Text = PreviewBlock.Text;

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

        private void Window_Closed(object sender, EventArgs e)
        {

        }

        private void QueryCreateWindow_Closed(object sender, EventArgs e)
        {
            main.paramField.isClosedQueryBuildWindow = true;
        }

        private void QueryCreateWindow_Loaded(object sender, RoutedEventArgs e)
        {
            main.paramField.isClosedQueryBuildWindow = false;
        }

        
    }
}