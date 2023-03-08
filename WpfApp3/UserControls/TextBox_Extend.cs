using System.Windows;
using System.Windows.Controls;

namespace HaruaConvert.UserControls
{
    internal class TextBox_Extend : TextBox
    {
        public event RoutedEventHandler InputTextChanged;
        public TextBox_Extend()
        {
            var oldVal = string.Empty;
            this.GotFocus += (sender, e) => { oldVal = this.Text; };
            this.LostFocus += (sender, e) =>
            { if (oldVal != this.Text && InputTextChanged != null) { InputTextChanged(sender, e); } };
        }


    }

}
