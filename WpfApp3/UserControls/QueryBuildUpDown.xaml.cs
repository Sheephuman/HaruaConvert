using FFMpegCore;
using HaruaConvert.UserControls;
using System;
using System.DirectoryServices.ActiveDirectory;
using System.Globalization;
using System.Numerics;
using System.Windows.Controls;


namespace HaruaConvert.UserControls
{
    /// <summary>
    /// QueryBuildUpDown.xaml の相互作用ロジック
    /// </summary>
    public partial class QueryBuildUpDown : UserControl
    {
        public static int minvalue { get; } = 500;
        public static readonly int maxvalue = 2000;
        public const int startvalue = 10;
     

        public static WpfNumericUpDown querybox { get; set; }

        public QueryBuildUpDown()
        {
            InitializeComponent();

        //    var textbox = qeryUpDown.TheNUDTextBox;
          //  textbox.Text = minvalue.ToString(CultureInfo.CurrentCulture);
        }
       
    }
}
