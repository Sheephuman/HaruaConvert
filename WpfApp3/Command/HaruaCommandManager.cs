using HaruaConvert.mainUI.QueryCreateWindow.LogWindow;
using HaruaConvert.userintarface;
using System.Windows;
using System.Windows.Input;
using WpfApp3.Parameter;

namespace HaruaConvert.Command
{
    public class HaruaCommandManager
    {
        MainWindow _main;
        QueryCreateWindow qi;



        public HaruaCommandManager(MainWindow main)
        {

            _main = main;


            qi = new QueryCreateWindow(main);

            CommandBinding queryBuildCommandBinding = new CommandBinding(
      HaruaButtonCommand.QueryBuildWindow_Open,
      QueryBuildWindow_Open,
      CanExecuteQueryBuildCommand);

            _main.CommandBindings.Add(queryBuildCommandBinding);

        }

        public void AddCommands()
        {
            // コマンドバインディングの追加
            CommandBinding queryBuildWindowOpenBinding = new CommandBinding(
                HaruaButtonCommand.QueryBuildWindow_Open,
                QueryBuildWindow_Open,
                CanExecuteQueryBuildCommand
                );


            CommandBinding defaultQueryBinding = new CommandBinding(
                HaruaButtonCommand.SetDefaultQuery,
                defaultSetQueryBinding,
                CanExecuteSetDefaultQueryCommand);


            CommandBinding ExplorerResterterComandBinding = new CommandBinding(
               HaruaButtonCommand.ExplorerRestarter,
               ExplorerResterterComand,
               CanExecuteSetDefaultQueryCommand);



            _main.CommandBindings.Add(queryBuildWindowOpenBinding);

            _main.CommandBindings.Add(defaultQueryBinding);
            _main.CommandBindings.Add(ExplorerResterterComandBinding);
        }

        private async void ExplorerResterterComand(object sender, ExecutedRoutedEventArgs e)
        {
            ExplorerRestarterClass explorerRestarterClass = new ExplorerRestarterClass();
            Terminate_ProcessClass tpc = null;
            await explorerRestarterClass.ExPlorerRestarter(tpc);

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

                _main.ParamText.Text = ClassShearingMenbers.defaultQuery;
            }

            else
                return;

        }

        private void QueryBuildWindow_Open(object sender, ExecutedRoutedEventArgs e)
        {
            IOpenExplorer openI = new OpernExplorerClass();


            var maintest = new MainWindow(openI);

            maintest._openExplorerTest.OpenExplorer(_main.paramField);


            if (_main.paramField.isClosedQueryBuildWindow)
            {
                qi = new QueryCreateWindow(_main);

            }

            if (!qi.IsVisible || qi.WindowState == WindowState.Minimized)
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
