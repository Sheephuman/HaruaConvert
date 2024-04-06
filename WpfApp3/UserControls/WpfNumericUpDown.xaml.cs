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
        /// 
       

            public WpfNumericUpDown()
        {
            InitializeComponent();
            
                //
                //
                //
                //TextBox.Text = minvalue.ToString(CultureInfo.CurrentCulture);

                isfirst = true;
            
            nuManager = new NumericUpDownManager( NUDTextBox);
        }

      public static MainWindow main { get; set; }

        NumericUpDownManager nuManager;

        public WpfNumericUpDown(int _selG)
        {
            InitializeComponent();
            selGenerate = _selG;
            if(selGenerate >= 500)
                TheNUDTextBox.Text = _selG.ToString(CultureInfo.CurrentCulture);
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
         
        }

        public void NUDButtonDown_Click(object sender, RoutedEventArgs e)
        {
            
           nuManager.NUDButtonDown(NUDTextBox,minvalue, -1);
        }

        public void NUDTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
           nuManager.NUDTextBox_PreviewKeyDownProc(NUDTextBox, minvalue, maxvalue,-1, e);
        }

        public void NUDTextBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
             nuManager.NUDTextBox_PreviewKeyDownProc(NUDTextBox, minvalue, maxvalue,-1,  e);
        }

        public void NUDButtonUP_Click(object sender, RoutedEventArgs e)
        {
            
            NumericUpDownManager.NUDButtonUP_ClickProc(NUDTextBox, maxvalue,1);
        }

        public static explicit operator WpfNumericUpDown(QueryBuildUpDown v)
        {
            throw new NotImplementedException();
        }

        private void NUDTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
           NumericUpDownManager.ValidateAndCorrectInput(NUDTextBox,minvalue,maxvalue);
            // フォーカスが外れた時に入力を検証し、必要に応じて修正
        }
    }
}
