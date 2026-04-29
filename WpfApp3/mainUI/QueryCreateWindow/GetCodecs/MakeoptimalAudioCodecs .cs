using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HaruaConvert.mainUI.QueryCreateWindow
{
   public class MakeOptimalName
    {
        public MakeOptimalName()
        {

        }

        public string MakeOptionalVideoContainer(string videocodec)
        {
            string container = string.Empty;
            switch (videocodec)
            {
                case "libx264":
                    container = "mp4";
                    break;
                case "libx265":
                    container = "mp4";
                    break;
                case "libsvtav1":
                    container = "mkv";
                    break;
                case "libvpx-vp9":
                    container = "webm";
                    break;
                case "AV1":
                    container = "mkv";
                    break;
                case "H.264":
                    container = "mp4";
                    break;
                case "H.265":
                    container = "mp4";
                    break;
                case "MPEG-4":
                    container = "mp4";
                    break;
                case "VP9":
                    container = "webm";
                    break;
                default:
                    container = "mp4";
                    break;
            }
            return container;
        }
        public string MakeOptimalVideotoAudioCodecs(string videocodec)
        {
            string audiocodec = string.Empty;
          switch (videocodec)
            {
                case "libx264":                   
                case "libx265":
                case "MPEG-4":
                    audiocodec = "aac";
                    break;

                case "libsvtav1":
                case "libvpx-vp9":
                case "AV1":
                case "VP9":
                    audiocodec = "opus";
                    break;
                
                
              
                default:
                    audiocodec = "aac";
                    break;
            }
            return audiocodec;
        }



    }
}
