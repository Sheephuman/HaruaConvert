﻿using FFMpegCore;
using HaruaConvert.HaruaServise;
using HaruaConvert.Parameter;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.ComponentModel;
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

    }



    public class Directory_ClickProcedure : IMainTabEvents
    {
        MainWindow main;



        ParamField mainParames;

        public Directory_ClickProcedure(ParamField _mainParames, MainWindow _main)
        {
            main = _main;
            this.mainParames = _mainParames;



            //参照値渡しによりプロパティの状態を受け渡す
        }

        public void Directory_DropButon_Click(object sender, RoutedEventArgs e)
        {

            //  ParamField.identification_Obj = sender;
            ClassShearingMenbers.ButtonName = ((Button)sender).Name;




            if (mainParames.isExecuteProcessed)
            {
                MessageBox.Show("ffmpeg.exeが実行中です");

                return;
            }



            using (CommonOpenDialogClass ofc = new CommonOpenDialogClass(false, ParamField.Maintab_InputDirectory))
            {

                var result = ofc.CommonOpens();

                if (result == CommonFileDialogResult.Ok)  //Selected OK
                {


                    mainParames.setFile = ofc.opFileName;
                    main.harua_View.SourcePathText = main.paramField.setFile;
                    main.FileNameLabel.Text = main.harua_View.SourcePathText;
                    main.FileNameLabel.ToolTip = main.harua_View.SourcePathText;


                    // ParamField.ConvertDirectory = paramField.setFile;

                    main.Drop_Label.Content = "変換";

                    ParamField.Maintab_InputDirectory = Path.GetDirectoryName(main.paramField.setFile);


                    //Update Maintab_InputDirectory
                    ParamField.Maintab_InputDirectory = Path.GetDirectoryName(ofc.opFileName);
                    main.ClearSourceFileData();


                    main.paramField.infoDelll.Invoke(main)
                        .ForEach(token => main.SorceFileDataBox.AppendText(token));


                }
                else //Selected Cancel
                {
                    return;
                }


                //  ParamField.ConvertDirectory = ofc.opFileName;
            }



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
            catch (Win32Exception ex)
            {
                var media = new MediaInfoService();
                media.HandleMediaAnalysisException(ex);


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


                main.mainFileConvertExec(main.paramField.setFile, sender);

                main.paramField.infoDelll.Invoke(main);



                main.LogWindowShow();


            }
            else
            {
                Directory_DropButon_Click(sender, e);
            }
        }
    }







}


