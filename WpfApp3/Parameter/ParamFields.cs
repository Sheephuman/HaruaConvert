using System.IO;
using System.Threading;

namespace HaruaConvert.Parameter
{

    /// <summary>
    /// Ini関係はIniSettingConsthへ    ///
    /// </summary>
    /// 

    ////Case:use static When share used parameter on Class 
    public class ParamFields
    {   

        public bool isExitProcessed { get; set; }
        public string output { get; set; }


        public string InputFileName { get; set; }

        public string tooltipText { get; set; }
       

        public int ffmpeg_pid { get; set; }
        public  CancellationTokenSource ctoken { get; set; }

   

        public static string OutputPath { get; set; }

        public static string InitialDirectory { get; set; }
        public static string InputDirectory { get; set; }
        public static string OutputDirectory { get; set; }
        public static string InputSelectorDirectory { get; set; }
        public static string OutputSelectorDirectory { get; set; }
        public bool isParam_Edited { get; set; } 

        public string isUsedOriginalArgument { get; set; }


        public bool isAutoScroll { get; set; }
        public bool isPaused { get; set; }

        

        public static class ButtonNameField
        {
            public const string Convert_DropButton = "Convert_DropButton";
            public const string Directory_DropButon = "Directory_DropButon";
            public const string OutputButton = "OutputButton";

            public const string _openButton =   "_openButton";

        }
      
        public static class ControlField
        {           
            public const string ParamSelector = nameof(ParamSelector);
            public const string OutputSelector = nameof(OutputSelector);
            public const string InputSelector = nameof(InputSelector);
        }   


    }
}
