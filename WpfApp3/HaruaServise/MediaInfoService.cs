using FFMpegCore;
using HaruaConvert.Parameter;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using WpfApp3.Parameter;

namespace HaruaConvert.HaruaServise
{
    public class MediaInfoService
    {

        MainWindow main;
       public MediaInfoService(MainWindow _main)
            {
            
            main= _main;
            }
            
        

        public void displayMediaInfo(string setFile)
        {

            try
            {
                if (string.IsNullOrEmpty(setFile))
                { return; }
                KillFFprobe killprobe = new KillFFprobe();
                killprobe.KillExistingFFprobeProcesses();


         //      ClearSourceFileData();

                FFOptions probe = new FFOptions();
                probe.BinaryFolder = "dll";


                 var mediaInfo = FFProbe.Analyse(setFile, probe);
                AppendMediaInfoToSourceFileData(mediaInfo);





                //明示的GC呼び出し
                //Call explicit GC
                //   GC.Collect();
            }
            catch (Exception ex) when (ex is FFMpegCore.Exceptions.FFMpegException ||
                               ex is NullReferenceException ||
                               ex is FFMpegCore.Exceptions.FormatNullException ||
                               ex is Instances.Exceptions.InstanceProcessAlreadyExitedException ||
                               ex is Win32Exception)
            {
                main.HandleMediaAnalysisException(ex);

            }
        }
        public void AppendMediaInfoToSourceFileData(IMediaAnalysis mediaInfo)
        {

            if (mediaInfo.PrimaryAudioStream == null)
            {
                MessageBox.Show("primary streams がhullだわ");
                return;
            }


            var resultBitRate = Math.Truncate(mediaInfo.PrimaryVideoStream.BitRate * 0.001);
            var resultAudioBitRate = Math.Truncate(mediaInfo.PrimaryAudioStream.BitRate * 0.001);
            var resultCodec = mediaInfo.PrimaryVideoStream.CodecLongName;
            var resultAudioCodec = mediaInfo.PrimaryAudioStream.CodecLongName;
            var resultCannels = mediaInfo.PrimaryAudioStream.Channels;

            main.SorceFileDataBox.AppendText("BitRate:" + $"{resultBitRate}" + "Kbps");
            main.SorceFileDataBox.AppendText(Environment.NewLine);
            main.SorceFileDataBox.AppendText("AudioBitRate:" + $"{resultAudioBitRate}" + "Kbps");
            main.SorceFileDataBox.AppendText(Environment.NewLine);
            main.SorceFileDataBox.AppendText("Codec:" + $"{resultCodec}");
            main.SorceFileDataBox.AppendText(Environment.NewLine);
            main.SorceFileDataBox.AppendText("AudioCodec:" + $"{resultAudioCodec}");
            main.SorceFileDataBox.AppendText(Environment.NewLine);
            main.SorceFileDataBox.AppendText("Cannels:" + $"{resultCannels}");
           



        }

    }
}
