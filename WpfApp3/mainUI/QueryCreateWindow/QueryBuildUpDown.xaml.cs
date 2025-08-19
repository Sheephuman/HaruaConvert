using HaruaConvert.userintarface;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;




namespace HaruaConvert.UserControls
{
    /// <summary>qジック
    /// </summary>
    public partial class QueryCreateUpDown : UserControl
    {
        public static int minvalue { get; } = 100;
        public static readonly int maxvalue = 2000;
        public const int startvalue = 10;




        public static WpfNumericUpDown querybox { get; set; }


        public QueryCreateUpDown()
        {
            InitializeComponent();

            NUDTextBox = QueryCreateWindow.qc.BitRateNumBox;

            // NUDTextBox.Text = minvalue.ToString(CultureInfo.CurrentCulture);


            nuManager = new NumericUpDownManager(NUDTextBox);
        }
        NumericUpDownManager nuManager;

        private void NUDTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //NumericUpDownManager.NumericUpDownTextChangedProc(NUDTextBox, startvalue, maxvalue, minvalue);
        }

        private void NUDTextBox_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            nuManager.NUDTextBox_PreviewKeyUpProc(NUDTextBox, minvalue, maxvalue, e);
        }

        private void NUDTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            nuManager.NUDTextBox_PreviewKeyDownProc(NUDTextBox, minvalue, maxvalue, -10, e);
        }

        public void NUDButtonUP_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is RepeatButton)
                nuManager.NUDButtonUP_ClickProc(NUDTextBox, maxvalue, +10);
        }

        public void NUDButtonDown_Click(object sender, RoutedEventArgs e)
        {



            if (Keyboard.IsKeyDown(Key.Up) && sender is TextBox)
                nuManager.NUDButtonDown(NUDTextBox, minvalue, +10);



            else
                nuManager.NUDButtonDown(NUDTextBox, minvalue, -10);

        }

        public void NUDTextBox_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            nuManager.NUDTextBox_PreviewMouseWheelProc(sender, e);
        }
    }
}
