using HaruaConvert.QueryBuildwindow;
using HaruaConvert.QueryBuildwindow.GetCodecs;
using HaruaConvert.UserControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;


namespace HaruaConvert.userintarface
{
    /// <summary>
    /// QueryCreateWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class QueryCreateWindow : Window
    {
        readonly int minValue = 500;


        public Dictionary<string, string> FfmpegVideoCodecDic { get; set;}
        public Dictionary<string, string> FfmpegAudioCodecDic { get; set; }

        readonly int maxValue = 2000;
        
        public QueryCreateWindow(WpfNumericUpDown wp)
        {


            InitializeComponent();

            var getCoudecs = new GetCodecsName();

            var codec = new CodecTypeManager();


            FfmpegVideoCodecDic = getCoudecs.GetCodecNameExecute(codec.TypeVideo);

             FfmpegAudioCodecDic = getCoudecs.GetCodecNameExecute(codec.TypeAudio);

            //foreach (var cocec in FfmpegAudioCodecDic)
            //{ Debug.WriteLine(cocec); }

              DataContext = this;
            wp.TheNUDTextBox.Text = minValue.ToString(CultureInfo.CurrentCulture);


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
            

        }

       

        private void QueryBuildChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void GetVideoCodecsButton_Click(object sender, RoutedEventArgs e)
        {
            //foreach (var colist in resultList )
            //    if(!VideoCodecBox.Items.Contains(colist))
            //         VideoCodecBox.Items.Add( colist );

               
        }
    }
}