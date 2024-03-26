using FFMpegCore;
using HaruaConvert.HaruaServise;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Documents;

namespace HaruaConvert.HaruaInterFace
{
    public interface IMediaInfoManager
    {
      public  List<string> DisplayMediaInfo(IMediaAnalysis mediaInfo);
       public void HandleMediaAnalysisException(Exception ex);
    }

  
    


}
