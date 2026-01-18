using FFMpegCore;
using HaruaConvert.HaruaInterFace;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace HaruaConvert.HaruaServise
{
    public class MediaInfoService : IMediaInfoManager
    {

        public MediaInfoService()
        {

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
            try
            {

                if (mediaInfo == null)
                    return MediaResultList;


                var resultBitRate = Math.Truncate(mediaInfo.PrimaryVideoStream.BitRate * 0.001);

                MediaResultList.Add("BitRate:" + $"{resultBitRate}" + "Kbps");


                MediaResultList.Add(Environment.NewLine);

                var resultCodec = mediaInfo.PrimaryVideoStream.CodecLongName;

                MediaResultList.Add("Codec:" + $"{resultCodec}");


                MediaResultList.Add(Environment.NewLine);


                var resultFramerate = Math.Truncate(mediaInfo.PrimaryVideoStream.AvgFrameRate);

                MediaResultList.Add("Framerate:" + $"{resultFramerate}");
                MediaResultList.Add(Environment.NewLine);

                var resultHeight = mediaInfo.PrimaryVideoStream.Height;

                MediaResultList.Add("Height:" + $"{resultHeight}");


                MediaResultList.Add(Environment.NewLine);
                var resultWidth = mediaInfo.PrimaryVideoStream.Width;
                MediaResultList.Add("Width:" + $"{resultWidth}");

                MediaResultList.Add(Environment.NewLine);



                if (mediaInfo.PrimaryAudioStream != null)
                {
                    var resultAudioBitRate = Math.Truncate(mediaInfo.PrimaryAudioStream.BitRate * 0.001);
                    MediaResultList.Add("AudioBitRate:" + $"{resultAudioBitRate}" + "Kbps");

                    MediaResultList.Add(Environment.NewLine);

                    var resultAudioCodec = mediaInfo.PrimaryAudioStream.CodecLongName;

                    MediaResultList.Add("AudioCodec:" + $"{resultAudioCodec}");

                    MediaResultList.Add(Environment.NewLine);

                    var resultCannels = mediaInfo.PrimaryAudioStream.Channels;
                    MediaResultList.Add("Cannels:" + $"{resultCannels}");




                }
                else
                {
                    MessageBox.Show("Audio Streamがないわ");

                }

                return MediaResultList;

            }
            catch (Exception ex)
            {
                HandleMediaAnalysisException(ex);
                return MediaResultList;

            }
        }
    }
}
