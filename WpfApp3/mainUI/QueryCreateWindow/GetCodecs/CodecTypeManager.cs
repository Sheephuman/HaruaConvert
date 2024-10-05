using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaruaConvert
{
    public class CodecTypeManager
    {
        public CodecTypeManager()
        {


        }
        //internal readonly string typeVideo = @"^\s*(V\.....D?)(\s+)([^\s]+)(\s+)(.*)";
        //internal readonly string typeVideo = @"^\s*(E:|V:)?\s*(D?)(\s+)([^\s]+)(\s+)(.*)";
        //internal readonly string typeVideo = @"^\s*(E:|V:)?\s*(D?)(\s+)([a-zA-Z0-9_]+)(\s+)(.*)";
　        internal readonly string typeVideo = @"^\s*(D?)(E?)(V?)(I?)(L?)(S?)\s+([^\s]+)(\s+)(.*)";



        //internal readonly string typeVideo = @"^\s*D?E?V\.L\.\s*hevc";




        internal readonly string typeAudio  = @"^\s*(A\.....D?)(\s+)([^\s]+)(\s+)(.*)";





    }
}
