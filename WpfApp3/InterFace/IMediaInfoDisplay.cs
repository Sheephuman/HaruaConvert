using FFMpegCore;
using HaruaConvert.HaruaServise;
using System;
using System.IO;

namespace HaruaConvert.HaruaInterFace
{
    public interface IMediaInfoManager
    {
        void DisplayFileData(IMediaAnalysis mediaInfo);
        void HandleMediaAnalysisException(Exception ex);
    }

    public class MediaInfoManager
    {
        private readonly IMediaInfoManager mediaInfoDisplay;

        public MediaInfoManager(IMediaInfoManager mediaInfoDisplay)
        {
            this.mediaInfoDisplay = mediaInfoDisplay ?? throw new ArgumentNullException(nameof(mediaInfoDisplay));
        }

        public void DisplayMediaInfo(string setFile)
        {
            try
            {
                if (string.IsNullOrEmpty(setFile))
                {
                    return;
                }

                KillFFprobe killprobe = new KillFFprobe();
                killprobe.KillExistingFFprobeProcesses();

                // メディア情報の取得と表示の処理
                FFOptions probe = new FFOptions { BinaryFolder = "dll" };
                var mediaInfo = FFProbe.Analyse(setFile, probe);
                mediaInfoDisplay.DisplayFileData(mediaInfo);
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                mediaInfoDisplay.HandleMediaAnalysisException(ex);
            }
        }
    }


}
