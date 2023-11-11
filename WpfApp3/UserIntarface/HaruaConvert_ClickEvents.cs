using HaruaConvert.Parameter;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
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
            isUPDownClicked = true;

            Generatednum = SelGenerate - 1;
            if (SelGenerate != Generatednum)
                SelGenerate = int.Parse(NumericUpDown1.NUDTextBox.Text, CultureInfo.CurrentCulture);

        }


        public void Directory_DropButon_Click(object sender, RoutedEventArgs e)
        {

            //  ParamFields.identification_Obj = sender;
            ClassShearingMenbers.ButtonName = ((Button)sender).Name;




            if (!paramField.isExitProcessed && !isForceExec)
            {
                MessageBox.Show("ffmpeg.exeが実行中です");

                return;
            }



            using (CommonOpenDialogClass ofc = new CommonOpenDialogClass(false, ParamFields.InputDirectory))
            {

                var result = ofc.CommonOpens();






                if (result == CommonFileDialogResult.Ok)  //Selected OK
                {


                    paramField.setFile = ofc.opFileName;
                    harua_View.SourcePathText = paramField.setFile;
                    SourcePathLabel.Text = harua_View.SourcePathText;
                    SourcePathLabel.ToolTip = harua_View.SourcePathText;


                    // ParamFields.ConvertDirectory = paramField.setFile;

                    Drop_Label.Content = "Convert";

                    ParamFields.InputDirectory = Path.GetDirectoryName(paramField.setFile);


                    //Update InputDirectory
                    ParamFields.InputDirectory = Path.GetDirectoryName(ofc.opFileName);


                }
                displayMediaInfo(paramField.setFile);
                //  ParamFields.ConvertDirectory = ofc.opFileName;
            }

        }




        public void NUDUP_Button_Click(object sender, RoutedEventArgs e)
        {
            isUPDownClicked = true;
            Generatednum = SelGenerate + 1;
            if (SelGenerate != Generatednum)
                SelGenerate = int.Parse(NumericUpDown1.NUDTextBox.Text, CultureInfo.CurrentCulture);

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



        private void Convert_DropButton_Click(object sender, RoutedEventArgs e)
        {

            if (!paramField.isExitProcessed && !isForceExec)
            {
                MessageBox.Show("ffmpwg.exeが実行中ですよ");
                return;
            }



            ClassShearingMenbers.ButtonName = ((Button)sender).Name;
            //var runinng = Process.GetProcessesByName("ffmpeg.exe");
            if (!string.IsNullOrEmpty(paramField.setFile))
            {
                //Convert Process Improvement Part
                paramField.isExitProcessed = FileConvertExec(paramField.setFile, sender);



                Lw.Activate();

            }
            else
            {

                Directory_DropButon_Click(sender, e);
                harua_View.SourcePathText = paramField.setFile;

            }




        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Closeだけでは確実にプロセスが終了されない
                Lw.Close();
                Close();

                using (var tpc = new Terminate_ProcessClass())
                {
                    ProcessKill_deligate killProcessDell = tpc.Terminate_Process;



                    //ffmpegの強制終了
                    if (th1 != null)
                    {
                        killProcessDell(paramField.ffmpeg_pid);

                    }

                    //main Processが正常に終了されていない場合の対策
                    if (mainProcess != null)
                    {
                        killProcessDell(mainProcess.Id);
                    }
                }
                Environment.Exit(0);




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

            if (!paramField.isExitProcessed && !isForceExec)
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


            paramField.isExitProcessed = FileConvertExec(paramField.setFile, sender);
        }


        private void ButtonGenerate_Click(object sender, RoutedEventArgs e)
        {
            // Generatednum = int.Parse(NumericUpDown1.NUDTextBox.Text, CultureInfo.CurrentCulture);



            if (!isUPDownClicked)
            { return; }

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
            isUPDownClicked = false;
          

        }

    }



}
