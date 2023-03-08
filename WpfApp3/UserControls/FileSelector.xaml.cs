using HaruaConvert.Parameter;
using System.Windows.Controls;


namespace HaruaConvert
{
    /// <summary>
    /// FileSelector.xaml の相互作用ロジック
    /// </summary>
    public partial class FileSelector : UserControl
    {

        public FileSelector()
        {
            InitializeComponent();
        }


        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        }

        private void FilePathBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //var ansest = VisualTreeHelperWrapperHelpers.FindDescendant<FileSelector>((TextBox)sender);


            //if (ansest?.Name == "InputSelector" && string.IsNullOrEmpty(FilePathBox.Text))
            //{
            //    ParamInterfase.InputFileName = "";
            //}
        }

      
    }
}
