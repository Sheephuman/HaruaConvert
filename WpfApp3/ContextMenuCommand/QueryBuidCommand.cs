using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HaruaConvert.Command
{
    static class QueryBuidCommand
    {
        public static readonly RoutedUICommand QueryBuildWindow_Open = new RoutedUICommand(
     "ffmpegコマンド組み立て", // コマンドの名前
     "QueryBuildOpen", // コマンドの識別名
     typeof(QueryBuidCommand)); // コマンドが定義されているクラス


        public static readonly RoutedUICommand SetDefaultQuery = new RoutedUICommand(
       "Default Queryをセット", // コマンドの名前
       "defaultSetQueryBinding", // コマンドの識別名
       typeof(QueryBuidCommand)); // コマンドが定義されているクラス
    }


}
