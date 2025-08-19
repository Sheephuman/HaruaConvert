using System.Windows.Input;

namespace HaruaConvert.Command
{
    public static class HaruaButtonCommand
    {
        public static readonly RoutedUICommand QueryBuildWindow_Open = new RoutedUICommand(
     "ffmpegコマンド組み立て", // コマンドの名前
     "QueryBuildWindow_Open", // コマンドの識別名
     typeof(HaruaButtonCommand)); // コマンドが定義されているクラス


        public static readonly RoutedUICommand SetDefaultQuery = new RoutedUICommand(
       "Default Queryをセット", // コマンドの名前
       "defaultSetQueryBinding", // コマンドの識別名
       typeof(HaruaButtonCommand)); // コマンドが定義されているクラス


        public static readonly RoutedUICommand ExplorerRestarter = new RoutedUICommand(
    nameof(ExplorerRestarter), // コマンドの名前
     nameof(ExplorerRestarter) + "_Binding", // コマンドの識別名
    typeof(HaruaButtonCommand)); // コマンドが定義されているクラス

    }


}
