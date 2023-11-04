using System.IO;
using WpfApp3.Parameter;

namespace HaruaConvert.Parameter
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1051:参照可能なインスタンス フィールドを宣言しません", Justification = "<保留中>")]
    public class ParamCreateClasss
    {

        public ParamCreateClasss(string _fullPath)
        {


            inputPath_ReadOnly = _fullPath;
            

            //check_output_Readonly = param.check_output_Readonly;
        }
        
        //Perfect Constructor menber
        readonly string inputPath_ReadOnly;


        public string inputPath;
        
        public string check_output;



        public string output;

        public string arguments;
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
                    check_output = Path.GetDirectoryName(inputPath_ReadOnly) +  "\\" + ConvertFileName(Path.GetFileName(inputPath_ReadOnly));



                //inputPath = @"""" + inputPath_ReadOnly + @"""";
                inputPath = "\"" + inputPath_ReadOnly + "\"";

                output = "\"" + check_output + "\"";

               ParamFields.OutputPath = output;


             arguments = inputPath;
                return arguments;
            
            
               
            //変換後のファイル名 
            //出力先にエスケープ文字を追加
            //ソースファイル名にエスケープ文字を追加


        }

        //出力先のファイル名として変換処理
        public string ConvertFileName(string fullPath)
        {
            string convertName = $"{fullPath.Replace(".mp4", "")}"+ ClassShearingMenbers. endFileNameStrings  + ".mp4";

            return convertName;
        }




     




    }
}
