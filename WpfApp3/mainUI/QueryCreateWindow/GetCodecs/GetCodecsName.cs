using FFMpegCore.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Documents;
using Windows.Devices.Enumeration;

namespace HaruaConvert.mainUI.QueryCreateWindow.GetCodecs
{
    public class GetCodecsName
    {
        public GetCodecsName() { }


        public GetCodecsName(string name) { }

        public Dictionary<string, string> GetCodecNameExecute(string codecTypeRegex, List<string> codecFind)
        {
            var lineDic = new Dictionary<string, string>();
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

            var regex = new Regex(codecTypeRegex);

            ///     var regex = new Regex(@"^\s*V\s*\.\.\.\.\.\s+([^\s]+)");
            var outregex = new Regex(@"V\.\.\.\.\.\s*=\s*(\S+)");



            bool isFirstLine = true;


            using (var reader = process.StandardOutput)
            {
                string line;



                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains('='))
                        continue;

                    //一行目判定
                    if (isFirstLine)
                    {
                        isFirstLine = false;
                        continue;
                    }


                    var outMatch = outregex.Match(line);

                    var match = regex.Match(line);
                    if (!outMatch.Success)
                        if (match.Success)
                        {


                            //CA1310対応
                            int startIndex = 7;

                            line = line.TrimStart();
                            var analizeSorce = line.Substring(startIndex);

                            int startIndex2 = analizeSorce.IndexOf(" ", StringComparison.OrdinalIgnoreCase);
                            var codecName = analizeSorce.Remove(startIndex2);


                            
                            if (codecName.Contains("hevc_mf"))
                            {

                                codecName = "libx265";                           
                                                              
                            }

                            var codecDoc = codecName + " : " +
                                 analizeSorce.Substring(startIndex2).TrimStart();






                            foreach (var codecindex in codecFind)
                            {

                            

                                if (codecName.Contains(codecindex))
                                {
                                 

                                    // lineDic に codecName が含まれていなければ追加
                                    if (!lineDic.ContainsKey(codecDoc))
                                    {

                                       // Debug.WriteLine(codecName);
                                        lineDic.Add(codecDoc, codecName);
                                    }
                                }
                            }

                        }
                }
            }

            process.WaitForExit();

            return lineDic;
        }
    }
}


    
