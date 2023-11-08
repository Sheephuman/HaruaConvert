using System.DirectoryServices.ActiveDirectory;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HaruaConvert
{
    /// <summary>
    /// SelectParamerter.xaml の相互作用ロジック
    /// </summary>
    public partial class ParamSelector : UserControl
    {
        public ParamSelector(MainWindow main)
        {
            InitializeComponent();
            ArgumentEditor.Text = "";
            _main = main;
        }

        MainWindow _main;

        /// <summary>
        /// 【WPF備忘録】TextBoxで、IME変換確定のEnterキーでは反応しないようにする
        /// http://www.madeinclinic.jp/c/20180421/
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPreviewTextInputUpdate(object sender, TextCompositionEventArgs e)
        {


           
        }
        
        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void SelectorLabel_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void invisibleText_IsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (!this.invisibleText.IsFocused)
                Focus();
        }

        private void invisibleText_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Focus();
        }

        private void invisibleText_InputTextChanged(object sender, System.EventArgs e)
        {

        }


        private void invisibleText_InputTextChanged(object sender, System.Windows.Input.KeyEventArgs e)
        {

        }

        private void invisibleText_InputTextChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            e.Handled = false;
        }

        private void invisibleText_KeyUp(object sender, KeyEventArgs e)
        {
          


        }


        //EventHandler PreDrawEvent;
        //public object objectLock { get; private set; }

        //// イベントを定義
        //public event EventHandler Changed;


        //event EventHandler Checked
        //{
        //    add
        //    {
        //        lock (objectLock)
        //        {

        //            PreDrawEvent += value;
        //        }
        //    }
        //    remove
        //    {
        //        lock (objectLock)
        //        {
        //            PreDrawEvent -= value;
        //        }
        //    }
        //}



    }
}