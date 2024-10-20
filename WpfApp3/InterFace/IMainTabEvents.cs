using FFMpegCore;
using HaruaConvert.HaruaServise;
using HaruaConvert.Parameter;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using WpfApp3.Parameter;

namespace HaruaConvert.HaruaInterFace
{

    public interface IMainTabEvents
    {
        public void Directory_DropButon_Click(object sender, RoutedEventArgs e);
        public void Convert_DropButton_Click(object sender, RoutedEventArgs e);
        public void DisplayMedia();
    }



    public class Directory_ClickProcedure : IMainTabEvents
    {
        MainWindow main;
        public Directory_ClickProcedure(MainWindow _main)
        {
            main = _main;
        }

        public void Directory_DropButon_Click(object sender, RoutedEventArgs e)
        {

            //  ParamField.identification_Obj = sender;
            ClassShearingMenbers.ButtonName = ((Button)sender).Name;




            if (main.paramField.isExecuteProcessed)
            {
                MessageBox.Show("ffmpeg.exeが実行中です");

                return;
            }



            using (CommonOpenDialogClass ofc = new CommonOpenDialogClass(false, ParamField.Maintab_InputDirectory))
            {

                var result = ofc.CommonOpens();

                if (result == CommonFileDialogResult.Ok)  //Selected OK
                {


                    main.paramField.setFile = ofc.opFileName;
                    main.harua_View.SourcePathText = main.paramField.setFile;
                    main.FileNameLabel.Text = main.harua_View.SourcePathText;
                    main.FileNameLabel.ToolTip = main.harua_View.SourcePathText;


                    // ParamField.ConvertDirectory = paramField.setFile;

                    main.Drop_Label.Content = "変換";

                    ParamField.Maintab_InputDirectory = Path.GetDirectoryName(main.paramField.setFile);


                    //Update Maintab_InputDirectory
                    ParamField.Maintab_InputDirectory = Path.GetDirectoryName(ofc.opFileName);
                    main.ClearSourceFileData();

                    DisplayMedia();

                }
                else //Selected Cancel
                {
                    return;
                }


                //  ParamField.ConvertDirectory = ofc.opFileName;
            }



        }


        public void DisplayMedia()
        {

            IMediaInfoManager Imedia = new MediaInfoService(main);
            var media = CallFfprobe(main.paramField.setFile);
            List<string> mediaLists = Imedia.DisplayMediaInfo(media);

            main.Dispatcher.Invoke(() =>
            {



                foreach (var mediaData in mediaLists)
                {
                    main.SorceFileDataBox.AppendText(mediaData);
                }

            });


        }

        public IMediaAnalysis CallFfprobe(string setFile)
        {

            FFOptions probe = new FFOptions();
            probe.BinaryFolder = "dll";

            IMediaAnalysis mediaInfo = null;
            try
            {
                mediaInfo = FFProbe.Analyse(setFile, probe);
                return mediaInfo;

            }
            catch (FFMpegCore.Exceptions.FFMpegException ex)
            {
                MessageBox.Show(ex.Message);

            }

            return mediaInfo;

        }

        public void Convert_DropButton_Click(object sender, RoutedEventArgs e)
        {

            if (main.paramField.isExecuteProcessed)
            {
                MessageBox.Show("ffmpeg.exeが実行中ですよ");
                return;
            }



            ClassShearingMenbers.ButtonName = ((Button)sender).Name;
            //var runinng = Process.GetProcessesByName("ffmpeg.exe");
            if (!string.IsNullOrEmpty(main.paramField.setFile))
            {
                //Convert Process Improvement Part


                main.paramField.isExecuteProcessed = main.FileConvertExec(main.paramField.setFile, sender);

                if (MainWindow.Lw == null)
                {
                    MainWindow.Lw = new LogWindow(main.paramField);

                    MainWindow.Lw.Show();


                }

                MainWindow.Lw.Activate();
            }
            else
            {
                Directory_DropButon_Click(sender, e);
            }
        }
    }







}


