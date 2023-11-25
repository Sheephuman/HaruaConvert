using System.IO;
using System.Threading;

namespace HaruaConvert.Parameter
{

    /// <summary>
    /// Ini関係はIniSettingConsthへ    ///
    /// </summary>
    /// 

    ////Case:use static When share used parameter on Class 
    public class ParamField
    {

        public  string iniPath { get; set; }
        public  string setFile { get; set; }

       

        public bool isExitProcessed { get; set; }
        public string output { get; set; }


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


        public static class QueryNames
        {
            public const string ffmpegQuery = nameof(ffmpegQuery);
            public const string BaseQuery = nameof(BaseQuery);
            public const string endStrings = nameof(endStrings);
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
