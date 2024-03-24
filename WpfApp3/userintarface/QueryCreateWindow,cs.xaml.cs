using HaruaConvert.UserControls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            wp.TheNUDTextBox.TextChanged += QueryBuildChanged;

//            textbox.TextChanged += QueryBuildChanged; 
        }

        private void QueryBuildChanged(object sender, TextChangedEventArgs e)
        {
            var judgeedContent = sender as WpfNumericUpDown; 
            if (judgeedContent != null)
            {
                var textbox = judgeedContent.TheNUDTextBox;
                textbox.Text = minValue.ToString(CultureInfo.CurrentCulture);

                int number = 0;
                if (textbox.Text != "")
                    if (!int.TryParse(textbox.Text, out number)) textbox.Text = minValue.ToString(CultureInfo.CurrentCulture);
                if (number >  maxValue) textbox.Text = maxValue.ToString(CultureInfo.CurrentCulture);
                if (number < maxValue) textbox.Text = maxValue.ToString(CultureInfo.CurrentCulture);
                textbox.SelectionStart = textbox.Text.Length;
            }
            
        }
    }
}