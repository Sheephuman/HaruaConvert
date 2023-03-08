using System;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HaruaConvert.UserControls
{
    /// <summary>
    /// WpfNumericUpDown.xaml の相互作用ロジック
    /// </summary>
    public partial class WpfNumericUpDown : UserControl
    {
        public WpfNumericUpDown()
        {
            InitializeComponent();
            NUDTextBox.Text = startvalue.ToString(CultureInfo.CurrentCulture);

           
        }

        public WpfNumericUpDown(int _selG)
        {
            InitializeComponent();
            selGenerate = _selG;
        }



        private readonly int minvalue = 1;
        private readonly int maxvalue = 100;
        private int startvalue = 10;
        public int selGenerate { get; set; }



        private void NUDTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int number = 0;
            if (NUDTextBox.Text != "")
                if (!int.TryParse(NUDTextBox.Text, out number)) NUDTextBox.Text = startvalue.ToString(CultureInfo.CurrentCulture);
            if (number > maxvalue) NUDTextBox.Text = maxvalue.ToString(CultureInfo.CurrentCulture);
            if (number < minvalue) NUDTextBox.Text = minvalue.ToString(CultureInfo.CurrentCulture);
            NUDTextBox.SelectionStart = NUDTextBox.Text.Length;
        }

        private void NUDButtonDown_Click(object sender, RoutedEventArgs e)
        {
            int number;
            if (NUDTextBox.Text != "") number = Convert.ToInt32(NUDTextBox.Text,CultureInfo.CurrentCulture);
            else number = 0;
            if (number > minvalue)
                NUDTextBox.Text = Convert.ToString(number - 1, CultureInfo.CurrentCulture);
        }

        private void NUDTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {


            if (e.Key == Key.Up)
            {
                NUDButtonUP.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(NUDButtonUP, new object[] { true });
            }


            if (e.Key == Key.Down)
            {
                NUDButtonDown.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(NUDButtonDown, new object[] { true });
            }
        }

        private void NUDTextBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Up)
                typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(NUDButtonUP, new object[] { false });

            if (e.Key == Key.Down)
                typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(NUDButtonDown, new object[] { false });


        }

        private void NUDButtonUP_Click(object sender, RoutedEventArgs e)
        {
            int number;
            if (NUDTextBox.Text != "") number = Convert.ToInt32(NUDTextBox.Text,CultureInfo.CurrentCulture);
            else number = 0;
            if (number < maxvalue)
                NUDTextBox.Text = Convert.ToString(number + 1,CultureInfo.CurrentCulture);
        }
    }
}
