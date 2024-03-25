using System.Globalization;
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

          
          NUDTextBox.Text = minvalue.ToString(CultureInfo.CurrentCulture);
        }

        private void NUDTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
             NumericUpDownManager.NumericUpDownTextChangedProc(NUDTextBox, startvalue, maxvalue, minvalue);
        }

        private void NUDTextBox_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            NumericUpDownManager.NUDTextBox_PreviewKeyUpProc(NUDButtonUP, NUDButtonDown, e);
        }

        private void NUDTextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            NumericUpDownManager.NUDTextBox_PreviewKeyDownProc(NUDButtonUP, NUDButtonDown, e);
        }

        private void NUDButtonUP_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NumericUpDownManager.NUDButtonUP_ClickProc(NUDTextBox, maxvalue);
        }

        private void NUDButtonDown_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NumericUpDownManager.NUDButtonDown(NUDTextBox,minvalue);
        }
    }
}
