using System.IO;

namespace HaruaConvert.Parameter
{
    public class ParamCreateClasss
    {

        public ParamCreateClasss(string _fullPath)
        {


            inputPath_ReadOnly = _fullPath;
            check_output_Readonly = inputPath_ReadOnly;


            //check_output_Readonly = param.check_output_Readonly;
        }
        
        //Perfect Constructor menber
        readonly string inputPath_ReadOnly;
        readonly string check_output_Readonly;

        public string inputPath;
        public string check_output;



        public string output;

        public static string arguments;
        /// <summary>
        /// Call ffmpegQuery Class
        /// make Parameter
        /// </summary>
        /// <returns></returns>
        /// 

        public string AddParamEscape(string locatePath)
        {


                //保存先パスの有無判定
                if(string.IsNullOrEmpty(locatePath))
                    check_output = locatePath + "\\" + ConvertFileName(Path.GetFileName(inputPath_ReadOnly));
                else
                    check_output = "\\" + ConvertFileName(Path.GetFileName(inputPath_ReadOnly));



                inputPath = @"""" + inputPath_ReadOnly + @"""";
                

                output = @"""" + check_output + @"""";          

                arguments = inputPath;
                return arguments;
            
            
               
            //変換後のファイル名 
            //出力先にエスケープ文字を追加
            //ソースファイル名にエスケープ文字を追加


        }

        //出力先のファイル名として変換処理
        public string ConvertFileName(string fullPath)
        {
            string convertName = $"{fullPath.Replace(".mp4", "")}"+ ParamInterfase.endFileNameStrings  + ".mp4";

            return convertName;
        }




        ///// <summary>
        ///// Call ffmpegQuery Class
        ///// </summary>
        ///// <param name="locatePath"></param>
        ///// <returns></returns>
        //public override string AddParam(string locatePath)
        //{

        //    var paramQuery = new FfmpegQuery(inputPath_ReadOnly);

        //    var fileName = Path.GetFileName(inputPath_ReadOnly);

        //    output = paramQuery.ConvertFileName(inputPath_ReadOnly);  

        //    check_output_Readonly = locatePath + "\\" + Path.GetFileName(output);
        //    //保存先パスにファイル名を追加

        //    inputPath_ReadOnly = @"""" + inputPath_ReadOnly + @"""";

        //    inputPath_ReadOnly = paramQuery.AddsetQuery(inputPath_ReadOnly);
        //    //基本パラメータのセット


        //    //入力ファイル名を変換後のファイル名に



        //   output = @"""" + check_output_Readonly + @"""";
        //    //出力パスとして扱う


        //    arguments = AddOption(inputPath_ReadOnly);
        //    //オプションパラメータを追加

        //    arguments += $"{output}";
        //    //最後に出力先を追加

        //    return arguments;
        //}


    }
}
