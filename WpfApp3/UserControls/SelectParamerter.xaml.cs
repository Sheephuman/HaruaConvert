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
        public ParamSelector()
        {
            InitializeComponent();
            ArgumentEditor.Text = "";
          
        }



        /// <summary>
        /// 【WPF備忘録】TextBoxで、IME変換確定のEnterキーでは反応しないようにする
        /// http://www.madeinclinic.jp/c/20180421/
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPreviewTextInputUpdate(object sender, TextCompositionEventArgs e)
        {


            if (e.TextComposition.CompositionText.Length == 0)
            {
                isImeOnConv = false;
            }
            else
            {
                isImeOnConv = true;
            }
        }
        private bool isImeOnConv = false; //IME利用中かどうか判定するフラグ
        private int EnterKeyBuffer { get; set; } //IMEでの変換決定のEnterキーに反応させないためのバッファ

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            if (isImeOnConv)
            {

                EnterKeyBuffer = 1;
            }
            else
            {
                EnterKeyBuffer = 0;
            }
            isImeOnConv = false;
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
            if (isImeOnConv == false && e.Key == Key.Enter && EnterKeyBuffer == 1)
            {
                EnterKeyBuffer = 0;
                return;
            }
            else if (isImeOnConv == false && e.Key == Key.Enter && EnterKeyBuffer == 0)
            {
                EnterKeyBuffer = 1;
            }


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
