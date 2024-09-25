using FFMpegCore;
using FFMpegCore.Arguments;
using HaruaConvert.Methods;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
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


        
        public EscapePath AddParamEscape(EscapePath escape, string extention)
        {
           

            //inputPath = @"""" + inputPath_ReadOnly + @"""";
            escape.inputPath = "\"" + inputPath_ReadOnly + "\"";

             escape.outputPath = "\"" + _convertFile + extention  + "\"";
            
            if(!string.IsNullOrEmpty(extention))
               escape.NonEscape_outputPath = _convertFile + extention;

            return escape;
            
            
               
            //変換後のファイル名p 
            //出力先にエスケープ文字を追加
            //ソースファイル名にエスケープ文字を追加


        }


        public string GetExtentionFileNamepattern(string target)
        {
           
            string pattern = @"\{FileName\}\.(\w+)";

            string extension = string.Empty;


            Match match = Regex.Match(target, pattern);
            if (match.Success)
            {
                extension = "." +match.Groups[1].Value;
                
                
                Debug.WriteLine("拡張子: " + extension);
            
            }

            return extension; 
        }
     




    }
}
