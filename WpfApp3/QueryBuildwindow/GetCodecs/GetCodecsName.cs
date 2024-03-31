using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Documents;

namespace HaruaConvert.QueryBuildwindow.GetCodecs
{
    public class GetCodecsName
    {
        public GetCodecsName() { }


        public GetCodecsName(string name) { }

        public List<string> GetCodecNameExecute()
        {
            var lineList = new List<string>();
            // ffmpegのパスを設定
            var ffmpegPath = @"dll\\ffmpeg.exe";
            var startInfo = new ProcessStartInfo(ffmpegPath, "-encoders")
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            var process = new Process { StartInfo = startInfo };
            process.Start();

           var regex = new Regex(@"^\s*V\s*\.\.\.\.\.\s+(\S+)");

///     var regex = new Regex(@"^\s*V\s*\.\.\.\.\.\s+([^\s]+)");
              var outregex = new Regex(@"V\.\.\.\.\.\s*=\s*(\S+)");
            bool isFirstLine =true ;


            using (var reader = process.StandardOutput)
            {
                string line;

                

                while ((line = reader.ReadLine()) != null)
                {

                    //一行目判定
                    if (isFirstLine)
                    { 
                    isFirstLine = false;
                        continue;
                    }


                    var outMatch = outregex.Match(line);


                    var match = regex.Match(line);
                    if(!outMatch.Success)
                    if (match.Success)
                    {
                       
                            //CA1310対応
                            int startIndex = 7;
                            string codecName = line.Substring(startIndex);
                            // コーデック名を出力
                            lineList.Add(codecName);
                    }
                }
            }

            process.WaitForExit();

            return lineList;
        }


    }


}
