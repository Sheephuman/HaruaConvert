using FFMpegCore;
using HaruaConvert.HaruaInterFace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaruaConvert.MediaInfo
{
    public class MediaInfoSetvice:IMediaInfoManager
    {
        public MediaInfoSetvice() { }

        public void MediaInfoDisplay(IMediaAnalysis mediaInfo)
        {
            throw new NotImplementedException();
        }

        public void HandleMediaAnalysisException(Exception ex)
        {
            throw new NotImplementedException();
        }

        public List<string> DisplayMediaInfo(IMediaAnalysis mediaInfo)
        {
            throw new NotImplementedException();
        }
    }
}
