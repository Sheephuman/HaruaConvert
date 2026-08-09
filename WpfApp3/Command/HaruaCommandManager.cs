using HaruaConvert.Json;
using HaruaConvert.userintarface;
using Newtonsoft.Json;
using System;
using System.IO;
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


            CommandBinding AddRuleQueryBinding = new CommandBinding(
               HaruaButtonCommand.AddRuleQuery,
               addRuleQueryBinding,
               CanExecuteAddRuleQueryCommand);



            CommandBinding ExplorerResterterComandBinding = new CommandBinding(
               HaruaButtonCommand.ExplorerRestarter,
               ExplorerResterterComand,
               CanExecuteSetDefaultQueryCommand);



            _main.CommandBindings.Add(queryBuildWindowOpenBinding);

            _main.CommandBindings.Add(defaultQueryBinding);
            _main.CommandBindings.Add(ExplorerResterterComandBinding);
        }

        private void CanExecuteAddRuleQueryCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
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

          
        }

        private void addRuleQueryBinding(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBoxResult msbr = MessageBox.Show("rule.Jsonにqueryを追加しますか？\r\n",
               "メッセージボックス", MessageBoxButton.YesNo,
               MessageBoxImage.Asterisk);
            if (msbr == MessageBoxResult.Yes)
            {
                var command = new QuerySaver();
                command.SaveToJsonFile(_main.ParamText.Text, "rules.json");
            }

            else
                return;
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
            //IOpenExplorer openI = new OpernExplorerClass();


            //var maintest = new MainWindow(openI);

            //maintest._openExplorerTest.OpenExplorer(_main.paramField);


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
