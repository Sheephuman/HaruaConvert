using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Text;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;


namespace HaruaConvert.Parameter
{

    /// <summary>
    /// MainWindowでインスタンス化して使い回しするデータクラス  ///
    /// </summary>
    /// 

    ////Case:use static When share used parameter on Class 
    public class ParamField 
    {
        public string inputPath_ReadOnly { get;}

        public string  check_output { get; set; }

        public  string iniPath { get; set; }
        public string profileQueryIni { get; set; } = "";

        public  string setFile { get; set; }

        


        public bool isExecuteProcessed { get; set; }
        public string output { get; set; }

        public bool isOpenFolder { get; set; }
        public bool isBackImage { get; set; }

        public bool isOutputButtonChecked { get; set; }


        public string InputFileDirectory { get; set; }
#pragma warning disable CA1711 // 識別子は、不適切なサフィックスを含むことはできません
        public string outputFileName_withoutEx { get; set; }
#pragma warning restore CA1711 // 識別子は、不適切なサフィックスを含むことはできません

        public string tooltipText { get; set; }
       

        public int ffmpeg_pid { get; set; }
        public  CancellationTokenSource ctoken { get; set; }

   

        public static string OutputPath { get; set; }

        public static string InitialDirectory { get; set; }
        public static string Maintab_InputDirectory { get; set; }
        public static string MainTab_OutputDirectory { get; set; }
        public static string ParamTab_InputSelectorDirectory { get; set; }
        public static string ParamTab_OutputSelectorDirectory { get; set; }
        public bool isParam_Edited { get; set; } 

        public string usedOriginalArgument { get; set; }


       public bool isAutoScroll { get; set; }
        

  


        public bool isPaused { get; set; }
        public bool isClosedQueryBuildWindow { get; set; }
        public List<int> explorerPrpcesslist { get; internal set; } = new List<int>();
        public bool isSuccessdbuildQuery { get; internal set; }

        public static class QueryNames
        {
            public const string ffmpegQuery = nameof(ffmpegQuery);
            public const string BaseQuery = nameof(BaseQuery);
            public const string endStrings = nameof(endStrings);

            public const string profileName = nameof(profileName);
            public const string profileQuery = nameof(profileQuery);
            public const string placeHolder = nameof(placeHolder);
            public const string placeHolderCount = nameof(placeHolder) + "_count";

            public const string opacitySlider = nameof(opacitySlider);
            public const string userControl = nameof(userControl);
        }

        public static class ButtonNameField
        {
            public const string Convert_DropButton = "Convert_DropButton";
            public const string Directory_DropButon = "Directory_DropButon";
            public const string OutputButton = "OutputButton";

            public const string _openButton =   "_openButton";
            public const string _ExecButton = "ExecButton";
                                              
        }
      
        public static class ControlField
        {           
            public const string ParamSelector = nameof(ParamSelector);
            public const string OutputSelector = nameof(OutputSelector);
            public const string InputSelector = nameof(InputSelector);
        }

    }
}
