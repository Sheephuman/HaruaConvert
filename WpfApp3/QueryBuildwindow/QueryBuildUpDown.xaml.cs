using System.Globalization;
using System.Windows.Controls;


namespace HaruaConvert.UserControls
{
    /// <summary>qジック
    /// </summary>
    public partial class QueryBuildUpDown : UserControl
    {
        public static int minvalue { get; } = 500;
        public static readonly int maxvalue = 2000;
        public const int startvalue = 10;


        public static WpfNumericUpDown querybox { get; set; }
        MainWindow main;


        public QueryBuildUpDown()
        {
            InitializeComponent();


            NUDTextBox.Text = minvalue.ToString(CultureInfo.CurrentCulture);


            nuManager = new NumericUpDownManager(NUDTextBox, MainWindow.main);
        }
        NumericUpDownManager nuManager;

        private void NUDTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // NumericUpDownManager.NumericUpDownTextChangedProc(NUDTextBox, startvalue, maxvalue, minvalue);
        }

        private void NUDTextBox_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
           // nuManager.NUDTextBox_PreviewKeyUpProc(NUDTextBox, minvalue, maxvalue, e);
        }

        private void NUDTextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
           // nuManager.NUDTextBox_PreviewKeyDownProc(NUDTextBox, minvalue, maxvalue, -10  , e);
        }

        private void NUDButtonUP_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NumericUpDownManager.NUDButtonUP_ClickProc(NUDTextBox, maxvalue, +10);
        }

        private void NUDButtonDown_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            nuManager.NUDButtonDown(NUDTextBox, minvalue,-10);
        }

        private void NUDTextBox_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            nuManager.NUDTextBox_PreviewMouseWheelProc(sender,e);
        }
    }
}
