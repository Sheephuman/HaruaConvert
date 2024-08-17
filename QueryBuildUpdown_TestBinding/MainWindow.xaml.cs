using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QueryBuildUpdown_TestBinding
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
           
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TextBox? textDisp = testUpDown.FindName("NUDTextBox") as TextBox;

            var qf = new QueryField();
            // QueryBuildUpDownにバインディングの設定
            Binding binding = new Binding("BitRateQuery")
            {
                Source = qf,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                Mode = BindingMode.OneWay
            };

            if (textDisp == null) throw new Exception();
             textDisp.SetBinding(TextBox.TextProperty, binding);
 
        }
    }
}