using HaruaConvert.Parameter;
using System.Windows;

namespace HaruaConvert
{
    public partial class MainWindow : Window
    {
       public void InitializeViewModels()
        {

            //  Set Default Parameter on FfmpegQueryClass
            harua_View = new Harua_ViewModel(this);

            ///MainWindowのパラメータボックスで読み込むquery
            DataContext = harua_View._Main_Param;
        }


    }
}
