using HaruaConvert.Methods;
using System.IO;
using WpfApp3.Parameter;

namespace HaruaConvert.Parameter
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1051:参照可能なインスタンス フィールドを宣言しません", Justification = "<保留中>")]
    public class ParamCreateClasss
    {

        public ParamCreateClasss(string _fullPath , string ConvertFile)
        {


            inputPath_ReadOnly = _fullPath;
            _convertFile = ConvertFile;

            //check_output_Readonly = param.check_output_Readonly;
        }
        
        //Perfect Constructor menber
        readonly string inputPath_ReadOnly;

        public string _convertFile;

        public string arguments;
        /// <summary>
        /// parameterのEscapeを行う
        /// make Parameter
        /// </summary>
        /// <returns></returns>      /// 


        
        public EscapePath AddParamEscape(EscapePath escape)
        {




            //inputPath = @"""" + inputPath_ReadOnly + @"""";
            escape.inputPath = "\"" + inputPath_ReadOnly + "\"";

                  escape.outputPath = "\"" + _convertFile + "\"";

               //ParamField.OutputPath = outputPath;


           //  arguments = inputPath;
                return escape;
            
            
               
            //変換後のファイル名p 
            //出力先にエスケープ文字を追加
            //ソースファイル名にエスケープ文字を追加


        }



     




    }
}
