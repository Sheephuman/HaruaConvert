using HaruaConvert.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaruaConvert.Methods
{
    public class ConvertFileNameClass
    {

        //出力先のファイル名として変換処理
        public string ConvertFileName(string fullPath, Harua_ViewModel harua_view)
        {
            string convertName = $"{fullPath.Replace(".mp4", "")}" + harua_view._Main_Param[0].endString + ".mp4";

            
            return convertName;
        }


    }
}
