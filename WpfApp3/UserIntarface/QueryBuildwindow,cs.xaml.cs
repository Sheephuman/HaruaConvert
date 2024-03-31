using HaruaConvert.UserControls;
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
    
  readonly int maxValue = 2000;
        
        public QueryCreateWindow(WpfNumericUpDown wp)
        {


            InitializeComponent();
            

            wp.TheNUDTextBox.Text = minValue.ToString(CultureInfo.CurrentCulture);
            //wp.TheNUDTextBox.TextChanged += QueryBuildChanged;
          // wp.NUDTextBox.LostFocus += NUDTextBox_LostFocus;
//            textbox.TextChanged += QueryBuildChanged; 
        }

       

        private void QueryBuildChanged(object sender, TextChangedEventArgs e)
        {
            
        }
    }
}