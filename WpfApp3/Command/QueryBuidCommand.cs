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
        public static readonly RoutedUICommand QueryBuild = new RoutedUICommand(
     "ffmpegコマンド組み立て", // コマンドの名前
     "QueryBuild", // コマンドの識別名
     typeof(QueryBuidCommand)); // コマンドが定義されているクラス
    }
}
