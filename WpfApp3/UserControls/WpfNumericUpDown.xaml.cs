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
      static bool isfirst { get; set; }
        /// <summary>
        /// https://stackoverflow.com/questions/841293/where-is-the-wpf-numeric-updown-control
        /// からの流用
        /// https://github.com/Torabi/WPFNumericUpDown　これも似たようなもの？
        /// </summary>
        public WpfNumericUpDown()
        {
            InitializeComponent();
            
                //NUDTextBox.Text = minvalue.ToString(CultureInfo.CurrentCulture);

                isfirst = true;
            
        }

        public WpfNumericUpDown(int _selG)
        {
            InitializeComponent();
            selGenerate = _selG;
            if(selGenerate >= 500)
                TheNUDTextBox.Text = _selG.ToString(CultureInfo.CurrentCulture);
        }

        public WpfNumericUpDown(QueryBuildUpDown qb)
        {
            InitializeComponent();
            
          //   qb.nudTextBox.Text = minValue.ToString(CultureInfo.CurrentCulture);
        }

        private readonly int minvalue = 1;
        private readonly int maxvalue = 100;
        private int startvalue = 10;
        public int selGenerate { get; set; }


        // NUDTextBoxへの公開プロパティ
        public TextBox TheNUDTextBox
        {
            get { return this.NUDTextBox; }
            set { this.NUDTextBox = value; }
        }

        /// <summary>
        /// NumberCount UpDown procedure
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void NUDTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int number = 0;
            if (TheNUDTextBox.Text != "")
                if (!int.TryParse(TheNUDTextBox.Text, out number)) TheNUDTextBox.Text = startvalue.ToString(CultureInfo.CurrentCulture);
            if (number > maxvalue) TheNUDTextBox.Text = maxvalue.ToString(CultureInfo.CurrentCulture);
            if (number < minvalue) TheNUDTextBox.Text = minvalue.ToString(CultureInfo.CurrentCulture);
            TheNUDTextBox.SelectionStart = TheNUDTextBox.Text.Length;
        }

        public void NUDButtonDown_Click(object sender, RoutedEventArgs e)
        {
            int number;
            if (NUDTextBox.Text != "") number = Convert.ToInt32(NUDTextBox.Text,CultureInfo.CurrentCulture);
            else number = 0;
            if (number > minvalue)
                NUDTextBox.Text = Convert.ToString(number - 1, CultureInfo.CurrentCulture);
        }

        public void NUDTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
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

        public void NUDTextBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Up)
                typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(NUDButtonUP, new object[] { false });

            if (e.Key == Key.Down)
                typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(NUDButtonDown, new object[] { false });


        }

        public void NUDButtonUP_Click(object sender, RoutedEventArgs e)
        {
            int number;
            if (NUDTextBox.Text != "") number = Convert.ToInt32(NUDTextBox.Text,CultureInfo.CurrentCulture);
            else number = 0;
            if (number < maxvalue)
                NUDTextBox.Text = Convert.ToString(number + 1,CultureInfo.CurrentCulture);
        }

        public static explicit operator WpfNumericUpDown(QueryBuildUpDown v)
        {
            throw new NotImplementedException();
        }
    }
}
