using HaruaConvert.Methods;
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
        /// parameterのEscapeを行う
        /// make Parameter
        /// </summary>
        /// <returns></returns>      /// 


        
        public string AddParamEscape()
        {




                //inputPath = @"""" + inputPath_ReadOnly + @"""";
                inputPath = "\"" + inputPath_ReadOnly + "\"";

                output = "\"" + check_output + "\"";

               ParamField.OutputPath = output;


             arguments = inputPath;
                return arguments;
            
            
               
            //変換後のファイル名 
            //出力先にエスケープ文字を追加
            //ソースファイル名にエスケープ文字を追加


        }



     




    }
}
