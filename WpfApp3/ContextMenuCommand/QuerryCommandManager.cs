using HaruaConvert.UserControls;
using HaruaConvert.userintarface;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HaruaConvert.Command
{
    public class QuerryCommandManager
    {
        MainWindow _main;
        QueryCreateWindow qi;

        

        public QuerryCommandManager(MainWindow main)
        {

            _main = main;


            qi = new QueryCreateWindow(main);

            CommandBinding queryBuildCommandBinding = new CommandBinding(
      QueryBuidCommand.QueryBuildWindow_Open,
      QueryBuildWindow_Open,
      CanExecuteQueryBuildCommand);

            _main.CommandBindings.Add(queryBuildCommandBinding);

        }

        public void AddCommands()
        {
            // コマンドバインディングの追加
            CommandBinding queryBuildWindowOpenBinding = new CommandBinding(
                QueryBuidCommand.QueryBuildWindow_Open,
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
            MessageBoxResult msbr = MessageBox.Show("ffmpegにdefaultクエリを設定しますか？\r\n",
               "メッセージボックス", MessageBoxButton.YesNo,
               MessageBoxImage.Asterisk);
            if (msbr == MessageBoxResult.Yes)
            {

                _main.ParamText.Text = "-b:v 700k -codec:v " +
                "libx265 -vf yadif=0:-1:1 -pix_fmt yuv420p -acodec aac -y -threads 2 ";
            }

            else
                return;
            
        }
        
        private void QueryBuildWindow_Open(object sender, ExecutedRoutedEventArgs e)
        {

            if (_main.paramField.isClosedQueryBuildWindow)
            {
                qi = new QueryCreateWindow(_main);
                
            }
           
            if (!qi.IsVisible || qi.WindowState == WindowState.Minimized )
            {
                qi.WindowState = WindowState.Normal;

                qi.Show();
            }
            else
            {
                qi.Activate();
                qi.Focus();
            }  



        }
    }
}
