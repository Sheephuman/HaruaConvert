using HaruaConvert.QueryBuilder.GetCodecs;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;


namespace HaruaConvert.QueryBuilder
{
    /// <summary>
    /// QueryCreateWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class QueryCreateWindow : Window
    {
       

       // readonly int minValue = 500;


        public Dictionary<string, string> FfmpegVideoCodecDic { get; set;}
        public Dictionary<string, string> FfmpegAudioCodecDic { get; set; }

        QueryField qf;
       public static TextBlock queryPreview { get; set; }

        public QueryCreateWindow(QueryField _qf)
        {


            InitializeComponent();

            qf = _qf;

            var getCoudecs = new GetCodecsName();

            var codec = new CodecTypeManager();


            FfmpegVideoCodecDic = getCoudecs.GetCodecNameExecute(codec.typeVideo);

             FfmpegAudioCodecDic = getCoudecs.GetCodecNameExecute(codec.typeAudio);

            //foreach (var cocec in FfmpegAudioCodecDic)
            //{ Debug.WriteLine(cocec); }
          

           // DataContext = this;
            //wp.PublicNUDTextBox.Text = minValue.ToString(CultureInfo.CurrentCulture);

            // TextBox textDisp = (TextBox)queryUpDown1.ContentTemplate.FindName("PublicNUDTextBox", queryUpDown1);

         

            //textDisp.Text = minValue.ToString(CultureInfo.CurrentCulture);

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

            //個別にBindingを設定する
            Binding VideoCodecBinding = new Binding(nameof(FfmpegVideoCodecDic))
            {
                Source = this,

                Mode = BindingMode.OneWay
            };

            VideoCodecBox.SetBinding(ItemsControl.ItemsSourceProperty, VideoCodecBinding);
            VideoCodecBox.DisplayMemberPath = "Key";  // 辞書のキーを表示する
            VideoCodecBox.SelectedValuePath = "Value";


            Binding audioCodecBinding = new Binding(nameof(FfmpegAudioCodecDic))
            {
                Source = this,

                Mode = BindingMode.OneWay
            };

            AudioCodecBox.SetBinding(ItemsControl.ItemsSourceProperty, audioCodecBinding);
            AudioCodecBox.DisplayMemberPath = "Key";  // 辞書のキーを表示する
            AudioCodecBox.SelectedValuePath = "Value";


            TextBox textDisp = queryUpDown1.FindName("NUDTextBox") as TextBox;


            // QueryBuildUpDownにバインディングの設定
            Binding binding = new Binding(nameof(qf.BuildQueryes))
            {
                Source = this,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                Mode = BindingMode.OneWay
            };


            textDisp.SetBinding(TextBox.TextProperty, binding);



            textDisp.Text = "500";

            queryPreview = PreviewBlock;
        }
    }
}