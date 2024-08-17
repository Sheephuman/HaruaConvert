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
    public partial class QueryCreateWindow : Window
    {
        //  readonly int minValue = 500;
        YourMultiValueConverter converter;


        public static TextBlock queryPreview { get; set; }

        private QueryField qf;

        public QueryCreateWindow(WpfNumericUpDown wp)
        {


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
            var queryBuidUpdown = new QueryBuildUpDown();

            
            
            
            NUDTextBox.PreviewMouseWheel += queryBuidUpdown.NUDTextBox_PreviewMouseWheel;
            NUDTextBox.PreviewKeyUp += queryBuidUpdown.NUDButtonUP_Click;
            NUDTextBox.PreviewKeyDown += queryBuidUpdown.NUDButtonDown_Click;
            
            NUDButtonUP.Click += queryBuidUpdown.NUDButtonUP_Click;
            NUDButtonDown.Click += queryBuidUpdown.NUDButtonDown_Click;
            DataContext = qf;

            MouseLeftButtonDown += (sender, e) => { DragMove(); };
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            


            ////個別にBindingを設定する
            //Binding VideoCodecBinding = new Binding(nameof(FfmpegVideoCodecDic))
            //{
            //    Source = this,

            //    Mode = BindingMode.OneWay
            //};

            //VideoCodecBox.SetBinding(ItemsControl.ItemsSourceProperty, VideoCodecBinding);
            //VideoCodecBox.DisplayMemberPath = "Key";  // 辞書のキーを表示する
            //VideoCodecBox.SelectedValuePath = "Value";


            //Binding audioCodecBinding = new Binding(nameof(FfmpegAudioCodecDic))
            //{
            //    Source = this,

            //    Mode = BindingMode.OneWay
            //};

            //AudioCodecBox.SetBinding(ItemsControl.ItemsSourceProperty, audioCodecBinding);
            //AudioCodecBox.DisplayMemberPath = "Key";  // 辞書のキーを表示する
            //AudioCodecBox.SelectedValuePath = "Value";



            //var qf = new QueryField();


            //MessageBox.Show(nameof(qf.BitRateQuery));

            //// QueryBuildUpDownにバインディングの設定
            //Binding binding = new Binding(nameof(qf.BitRateQuery))
            //{
            //  Source = qf,
            //    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
            //    Mode = BindingMode.OneWay
            //};


            //textDisp.SetBinding(TextBox.TextProperty, binding);



            // queryPreview = PreviewBlock;
        }

        private void testLabel_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
           
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            
         
            if (!converter.isVideoCodec)
            {
                converter.IsBitrateChecked = true;
            }
            else
                converter.IsBitrateChecked = false;
        }

      
        private void EnableVideoCodecChecker_Checked(object sender, RoutedEventArgs e)
        {
           
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

        }
    }
}