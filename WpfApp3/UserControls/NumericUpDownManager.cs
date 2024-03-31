using System;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace HaruaConvert.UserControls
{
    public class NumericUpDownManager
    {
        public NumericUpDownManager(TextBox _NUDTextBox)
        {
            NUDTextBox = _NUDTextBox;


        }
        TextBox NUDTextBox;
        int startvalue;

        int maxvalue;
        public void NumericSetupEventHandlers()
        {



        }


        public static void NumericUpDownTextChangedProc(TextBox NUDTextBox, int startvalue,int maxvalue, int minvalue)
        {
            int number = 0;
            if (NUDTextBox.Text != "")
                if (!int.TryParse(NUDTextBox.Text, out number)) NUDTextBox.Text = startvalue.ToString(CultureInfo.CurrentCulture);
            if (number > maxvalue) NUDTextBox.Text = maxvalue.ToString(CultureInfo.CurrentCulture);
            if (number < minvalue) NUDTextBox.Text = minvalue.ToString(CultureInfo.CurrentCulture);
            NUDTextBox.SelectionStart = NUDTextBox.Text.Length;

        }

        public static void NUDButtonDown(TextBox NUDTextBox,int minvalue)
        {
            int number;
            if (NUDTextBox.Text != "") number = Convert.ToInt32(NUDTextBox.Text, CultureInfo.CurrentCulture);
            else number = 0;
            if (number > minvalue)
                NUDTextBox.Text = Convert.ToString(number - 1, CultureInfo.CurrentCulture);

        }


        public static void NUDTextBox_PreviewKeyDownProc(RepeatButton NUDButtonUP, RepeatButton NUDButtonDown, KeyEventArgs e)
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


        public static void NUDTextBox_PreviewKeyUpProc(RepeatButton NUDButtonUP, RepeatButton NUDButtonDown, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
                typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(NUDButtonUP, new object[] { false });

            if (e.Key == Key.Down)
                typeof(Button).GetMethod("set_IsPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(NUDButtonDown, new object[] { false });
        }


        public static void NUDButtonUP_ClickProc(TextBox NUDTextBox,int maxvalue)
        {
            int number;
            if (NUDTextBox.Text != "") number = Convert.ToInt32(NUDTextBox.Text, CultureInfo.CurrentCulture);
            else number = 0;
            if (number < maxvalue)
                NUDTextBox.Text = Convert.ToString(number + 1, CultureInfo.CurrentCulture);

        }

    }
        internal void NUDTextBox_PreviewMouseWheelProc(object sender, System.Windows.Input.MouseWheelEventArgs e)
}
