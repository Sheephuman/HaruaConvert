using HaruaConvert.UserControls;
using HaruaConvert.userintarface;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace HaruaConvert.Command
{
    public class QuerryCommandManager
    {
        MainWindow _main;
        public QuerryCommandManager(MainWindow main)
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
            CommandBinding queryBuildWindowOpenBinding = new CommandBinding(
                QueryBuidCommand.QueryBuild,
                QueryBuildWindow_Open,
                CanExecuteQueryBuildCommand
                );


            CommandBinding defaultQueryBinding = new CommandBinding(
                QueryBuidCommand.SetDefaultQuery,
                defaultSetQueryBinding,
                CanExecuteSetDefaultQueryCommand);


            _main.CommandBindings.Add(queryBuildWindowOpenBinding);

            _main.CommandBindings.Add(defaultQueryBinding);

        }

        private void CanExecuteSetDefaultQueryCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true; //
        }

        private void CanExecuteQueryBuildCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;

            //MenuItem menueItem = sender as MenuItem;


            //if (_main.SetDefaultCommandItem == menueItem)
            //{
            //    e.CanExecute = true;

            //}
            //else 
            //{

            //    e.CanExecute = false; // その他の場合は実行不可
            //}

            // e.Handled = true; // 他のバインディングに伝播しないようにする
        }


        private void defaultSetQueryBinding(object sender, ExecutedRoutedEventArgs e)
        {
            // どのMenuItemがクリックされたかを判定する
       
          _main.ParamText.Text = "-b:v 700k -codec:v h264 -vf yadif=0:-1:1 -pix_fmt yuv420p -acodec aac -y -threads 2 ";
                
            
        }

        private void QueryBuildWindow_Open(object sender, ExecutedRoutedEventArgs e)
        {
             
                    QueryCreateWindow qi = new QueryCreateWindow(_main);
                    qi.Show();
                



        }
    }
}
