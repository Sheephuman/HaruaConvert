using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaruaConvert.QueryBuildwindow
{
    public class CodecTypeManager
    {
        public CodecTypeManager()
        {


        }
        public const string typeVideo = @"^\s*(V\.....D?)(\s+)([^\s]+)(\s+)(.*)";
        
        
        public const string typeAudio = @"^\s*(A\.....D?)(\s+)([^\s]+)(\s+)(.*)";
        

        public string TypeVideo
        {
            get { return typeVideo; }
        }
        public string TypeAudio
        {
            get { return typeAudio; }
        }


    }
}
