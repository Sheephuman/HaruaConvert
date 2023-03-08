using HaruaConvert.InterFace;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
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
using static HaruaConvert.InterFace.IHaruaInterFace;

namespace WpfApp3
{
    /// <summary>
    /// testForm.xaml の相互作用ロジック
    /// </summary>
    public partial class testForm : Window,IHaruaInterFace.IMethods
    {
       public IMethods IHaruaMethods;

        public testForm(IHaruaInterFace.IMethods _IMethods)
        {
            InitializeComponent();


            IHaruaMethods = _IMethods;


        }

        public CommonFileDialogResult CommonOpens()
        {
           return IHaruaMethods.CommonOpens();
        }

        private void testButton_Click(object sender, RoutedEventArgs e)
        {

            IHaruaMethods.CommonOpens();
        }
    }
}
