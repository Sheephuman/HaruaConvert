using HaruaConvert.QueryBuilder;
using HaruaConvert.QueryBuildwindow;
using HaruaConvert.QueryBuildwindow.GetCodecs;
using HaruaConvert.UserControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using static System.Resources.ResXFileRef;


namespace HaruaConvert.userintarface
{
    /// <summary>
    /// QueryCreateWindow.xaml の相互作用ロジック
    /// </summary>
    /// 
    
  

    public partial class QueryCreateWindow : Window
    {
        //  readonly int minValue = 500;
        YourMultiValueConverter converter;
      
            
     //  public static TextBlock queryPreview { get; set; }

        private QueryField qf;
        public static QueryCreateWindow qc { get; set; }


        public QueryCreateWindow(WpfNumericUpDown wp)
        {

            qc = this;
            InitializeComponent();
            converter = new YourMultiValueConverter();

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
            FileNameExtentionBox.Items.Add(".flv:");
            FileNameExtentionBox.Items.Add(".webm");
            FileNameExtentionBox.Items.Add(".mpeg");
            FileNameExtentionBox.Items.Add(".rmvb");

            
            var numanager = new NumericUpDownManager(NUDTextBox);
            var queryBuidUpdown = new QueryCreateUpDown();

            
            
            
            NUDTextBox.PreviewMouseWheel += queryBuidUpdown.NUDTextBox_PreviewMouseWheel;
            NUDTextBox.PreviewKeyUp += queryBuidUpdown.NUDButtonUP_Click;
            NUDTextBox.PreviewKeyDown += queryBuidUpdown.NUDButtonDown_Click;
            
            NUDButtonUP.Click += queryBuidUpdown.NUDButtonUP_Click;
            NUDButtonDown.Click += queryBuidUpdown.NUDButtonDown_Click;
            DataContext = qf;

            MouseLeftButtonDown += (sender, e) => { DragMove(); };
        }

       


      


        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            
         
            if (!converter.isVideoCodec)
            {
                converter.IsBitrateChecked = true;
            }
            else
                converter.IsBitrateChecked = false;

            qf.UpdateAllInput();
        }

      
        private void EnableVideoCodecChecker_Checked(object sender, RoutedEventArgs e)
        {
        
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
            qf.UpdateAllInput();
            EnableVideoCodecChecker.IsEnabled = enablePostTwitterChecker.IsChecked == true ? false : true;

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

            qf.UpdateAllInput();
        }
    }
}