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
        internal readonly string typeVideo = @"^\s*(V\.....D?)(\s+)([^\s]+)(\s+)(.*)";


        internal readonly string typeAudio  = @"^\s*(A\.....D?)(\s+)([^\s]+)(\s+)(.*)";





    }
}
