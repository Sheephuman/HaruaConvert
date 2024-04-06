using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaruaConvert.QueryBuildwindow
{
    internal class CodecTypeManager
    {
        internal readonly string TypeVideo = @"^\s*(V\.....D?)(\s+)([^\s]+)(\s+)(.*)";
        internal readonly string TypeAudio = @"^\s*(A\.....D?)(\s+)([^\s]+)(\s+)(.*)";
    }
}
