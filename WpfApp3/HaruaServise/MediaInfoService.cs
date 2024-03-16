using FFMpegCore;
using HaruaConvert.Parameter;
using System;
using System.ComponentModel;
using System.IO;
using WpfApp3.Parameter;

namespace HaruaConvert.HaruaServise
{
    public class MediaInfoService
    {

       
        public void displayMediaInfo( MainWindow main)
        {

            try
            {
                if (string.IsNullOrEmpty(main.paramField.setFile))
                { return; }

                main.KillExistingFFprobeProcesses();


               main.ClearSourceFileData();

                FFOptions probe = new FFOptions();
                probe.BinaryFolder = "dll";


                 var mediaInfo = FFProbe.Analyse(main.paramField.setFile, probe);
                main.AppendMediaInfoToSourceFileData(mediaInfo);





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

    }
}
