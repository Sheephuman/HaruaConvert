using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HaruaConvert.Command
{
    public class QuerryBuildManager
    {
        MainWindow _main;
       public QuerryBuildManager(MainWindow main) 
        { 
        
            _main = main;




            CommandBinding queryBuildCommandBinding = new CommandBinding(
      QueryBuidCommand.QueryBuild,
      QueryBuildWindow_Open,
      CanExecuteQueryBuildCommand);

            _main.CommandBindings.Add(queryBuildCommandBinding);

        }

        public void AddCommands()
        {
            // コマンドバインディングの追加
            CommandBinding queryBuildCommandBinding = new CommandBinding(
                QueryBuidCommand.QueryBuild,
                QueryBuildWindow_Open,
                CanExecuteQueryBuildCommand);

            _main.CommandBindings.Add(queryBuildCommandBinding);
        }


        private void CanExecuteQueryBuildCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            // コマンドが実行可能かどうかのロジック
            // ここでは常にtrueを返していますが、条件に応じて変更してください
            e.CanExecute = true;
        }
        private void QueryBuildWindow_Open(object sender, ExecutedRoutedEventArgs e)
        {
            // コマンド実行時のロジック
          
        }


    }
}
