using HaruaConvert.Parameter;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using WpfApp3.Parameter;

namespace HaruaConvert
{
    public partial class MainWindow
    {
        

        

        private void NUD_DownButton_Click(object sender, RoutedEventArgs e)
        {
            //isUPDownClicked = true;

            //Generatednum = SelGenerate - 1;
            //if (SelGenerate != Generatednum)
            //    SelGenerate = int.Parse(NumericUpDown1.NUDTextBox.Text, CultureInfo.CurrentCulture);

        }



        public void NUDUP_Button_Click(object sender, RoutedEventArgs e)
        {
            //isUPDownClicked = true;
            //Generatednum = SelGenerate + 1;
            //if (SelGenerate != Generatednum)
            //    SelGenerate = int.Parse(NumericUpDown1.NUDTextBox.Text, CultureInfo.CurrentCulture);

        }

        /// <summary>
        /// Selectorを列挙して、全ての子コントロールで同じ処理をする
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Tb_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            ////https://balard.sakura.ne.jp/vb/wpf/contentcontrol.php
            var childContent = sender as ContentControl;

            //Find Decendant Element in ContentControl (!!!Caution!!!: Required)
            var labelBlock = VisualTreeHelperWrapperHelpers.FindDescendant<TextBlock>(childContent);


            if (childContent != null)
                foreach (ParamSelector sp in selectorList)
                {
                    if (string.IsNullOrEmpty(labelBlock.Text))
                    {
                        return;
                    }

                    if (labelBlock == sp.ParamLabel)
                    {                    //  sp.SelectorLabel.Margin = new Thickness(100, 100, 100, 100);

                        sp.invisibleText.Text = "";
                        sp.invisibleText.Text = labelBlock.Text;
                        labelBlock.Visibility = Visibility.Hidden;
                        sp.invisibleText.Visibility = Visibility.Visible;





                        if (sp.invisibleText.Visibility == Visibility.Visible)
                        {
                            this.Dispatcher.BeginInvoke((Action)delegate
                            {
                                Keyboard.Focus(sp.invisibleText);
                            }, DispatcherPriority.Render);
                        }



                        return;
                    }
                }
        }


        private async void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ParamSave_Procedure();


                // Closeだけでは確実にプロセスが終了されない
                  if(Lw != null)
                     Lw.Close();
                // Close();
                long threshold = 0;
                if (AllExplorerProcesses != null)
                {


                    // 最小メモリサイズのプロセスを取得
                    Process smallestMemoryProcess = AllExplorerProcesses
                    .OrderBy(process => process.WorkingSet64) // メモリサイズでソート
                    .FirstOrDefault(); // 最初のプロセスを取得（最小サイズ）

                    threshold = smallestMemoryProcess.WorkingSet64 * 2; // 10MB

                }

                    using (var tpc = new Terminate_ProcessClass())
                    {

                        ProcessKill_deligate killProcessDell = tpc.Terminate_Process;

                      
                        // explorer.exeの終了処理。
                        if (AllExplorerProcesses != null)
                        {
                            // メモリサイズの閾値（例: 10MB = 10485760バイト）
                          

                            foreach (Process explorer in main.AllExplorerProcesses)
                            {
                                if (threshold >= explorer.WorkingSet64)
                                    await killProcessDell(explorer.Id);  // 非同期にプロセスを終了
                            }


                            // 終了処理が完了したことを通知する変数
                            var Completed = new TaskCompletionSource<bool>();


                            // ここでタスクの完了を手動で設定
                            Completed.SetResult(true);


                            await Completed.Task;


                            ///タスクバーが復活しなかった
                            //ProcessStartInfo startInfo = new ProcessStartInfo
                            //{
                            //    FileName = "explorer.exe",    // 実行するファイル（explorer.exe）

                            //};



                            // ffmpegの強制終了
                            if (th1 != null)
                            {
                                await killProcessDell(paramField.ffmpeg_pid);  // 非同期にプロセスを終了
                            }
                        }
                    // mainWindow Processが正常に終了されていない場合の対策


                        if (mainProcess != null)
                        {
                            await killProcessDell(mainProcess.Id);  // 非同期にプロセスを終了
                        }

                    }
                  

                


               // Application.Current.Shutdown();  // アプリケーションの終了処理
                                                     // 終了処理が完了するまで待機
                   

                
            }
         
        


            catch (TaskCanceledException ex)
            {
                MessageBox.Show(ex.Message);
            }

            catch (System.ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }

            //When Close() Only is not shutdown
        }



        private void OriginalParamExecButton_Click(object sender, RoutedEventArgs e)
        {
            ClassShearingMenbers.ButtonName = ((Button)sender).Name;

            if (paramField.isExecuteProcessed)
            {
                MessageBox.Show("ffmpwg.exeが実行中ですわ");

                return; 
            }
            //early return

         
            else if (string.IsNullOrEmpty(paramField.usedOriginalArgument))
                    {
                        MessageBox.Show("ユーザーパラメータが空欄です");
                        return;
                    }


            else　if (string.IsNullOrEmpty(InputSelector.FilePathBox.Text))
            {
                MessageBox.Show("入力パスが空欄です");
                return;
            }

            else if (string.IsNullOrEmpty(OutputSelector.FilePathBox.Text))
            {
                MessageBox.Show("出力パスが空欄です");
                return;
            }


            paramField.isExecuteProcessed = FileConvertExec(paramField.setFile, sender, GetLw());
        }


        private void ButtonGenerate_Click(object sender, RoutedEventArgs e)
        {
            // Generatednum = int.Parse(NumericUpDown1.NUDTextBox.Text, CultureInfo.CurrentCulture);



            //if (!isUPDownClicked)
            //{ return; }

            if (paramField.isParam_Edited)
            {

                if (MessageBox.Show("LabelかParameterが変更されているわ。保存するの？", "Information", MessageBoxButton.YesNo,
                       MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    ParamSave_Procedure();
                    paramField.isParam_Edited = false;
                }
                
            }
            Generate_ParamSelector();

            ParamSelector_SetText(sender, true);
            
            

        }
        
    }

    


}
