using FFMpegCore;
using HaruaConvert.HaruaInterFace;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace HaruaConvert.HaruaServise
{
    public class MediaInfoService:IMediaInfoManager
    {

        MainWindow main;
       public MediaInfoService(MainWindow _main)
            {
            
            main= _main;
            }
            
       readonly IMediaInfoManager mediaInfoManager;
       public MediaInfoService(IMediaInfoManager imedia)
        {
            this.mediaInfoManager = imedia ?? throw new ArgumentNullException(nameof(imedia));

        }

        public List<string> displayMediaInfo(string setFile)
        {
            var result = new List<string>();
            try
            {
                if (string.IsNullOrEmpty(setFile))
                { return result; }


                //      ClearSourceFileData();

                FFOptions probe = new FFOptions();
                probe.BinaryFolder = "dll";


                //        return result;


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
                HandleMediaAnalysisException(ex);
                return result;
            }

         
            return result;

            //}
        }


        public void HandleMediaAnalysisException(Exception ex)
        {

            string message = ex.Message;

            // 特定の例外タイプに基づいてカスタマイズされたメッセージを追加
            if (ex is NullReferenceException)
            {
                message += "\nfforobeの呼び出しに失敗したみたい...";
            }
            else if (ex is Win32Exception)
            {
                message += "\nffprobe.exeがないわよ";
            }
            // その他の特定の例外に対する処理...

            MessageBox.Show(message);



        }
        //public List<string> AppendMediaInfoToSourceFileData(IMediaAnalysis mediaInfo)
            //{
        //    var MediaResultList = new List<string>();

        //    //if (mediaInfo.PrimaryAudioStream == null)
        //    //{
        //    //    MessageBox.Show("primary streams がhullだわ");
        //    //    return MediaResultList;
        //    //}


        //    //var resultBitRate = Math.Truncate(mediaInfo.PrimaryVideoStream.BitRate * 0.001);
        //    //var resultAudioBitRate = Math.Truncate(mediaInfo.PrimaryAudioStream.BitRate * 0.001);
        //    //var resultCodec = mediaInfo.PrimaryVideoStream.CodecLongName;
        //    //var resultAudioCodec = mediaInfo.PrimaryAudioStream.CodecLongName;
        //    //var resultCannels = mediaInfo.PrimaryAudioStream.Channels;

        //    //MediaResultList.Add("BitRate:" + $"{resultBitRate}" + "Kbps");
        //    //MediaResultList.Add(Environment.NewLine);
        //    //MediaResultList.Add("AudioBitRate:" + $"{resultAudioBitRate}" + "Kbps");
        //    //MediaResultList.Add(Environment.NewLine);
        //    //MediaResultList.Add("Codec:" + $"{resultCodec}");
        //    //MediaResultList.Add(Environment.NewLine);
        //    //MediaResultList.Add("AudioCodec:" + $"{resultAudioCodec}");
        //    //MediaResultList.Add(Environment.NewLine);
        //    //MediaResultList.Add("Cannels:" + $"{resultCannels}");
            
        //    return MediaResultList;

        //}


       public List<string> DisplayMediaInfo(IMediaAnalysis mediaInfo)
        {
            var MediaResultList = new List<string>();

            if (mediaInfo.PrimaryAudioStream == null)
            {
                MessageBox.Show("primary streams がhullだわ");
                return MediaResultList;
            }


            var resultBitRate = Math.Truncate(mediaInfo.PrimaryVideoStream.BitRate * 0.001);
            var resultAudioBitRate = Math.Truncate(mediaInfo.PrimaryAudioStream.BitRate * 0.001);
            var resultCodec = mediaInfo.PrimaryVideoStream.CodecLongName;
            var resultAudioCodec = mediaInfo.PrimaryAudioStream.CodecLongName;
            var resultCannels = mediaInfo.PrimaryAudioStream.Channels;

            MediaResultList.Add("BitRate:" + $"{resultBitRate}" + "Kbps");
            MediaResultList.Add(Environment.NewLine);
            MediaResultList.Add("AudioBitRate:" + $"{resultAudioBitRate}" + "Kbps");
            MediaResultList.Add(Environment.NewLine);
            MediaResultList.Add("Codec:" + $"{resultCodec}");
            MediaResultList.Add(Environment.NewLine);
            MediaResultList.Add("AudioCodec:" + $"{resultAudioCodec}");
            MediaResultList.Add(Environment.NewLine);
            MediaResultList.Add("Cannels:" + $"{resultCannels}");

            return MediaResultList;
        }
    }
}
