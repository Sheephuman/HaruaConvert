using System.IO;
using System.Threading;

namespace HaruaConvert.Parameter
{

    /// <summary>
    /// Ini関係はIniSettingConsthへ
    ///
    /// </summary>
    internal interface ParamInterfase
    {
        public static string ButtonName { get; set; }

        public static bool isExitProcessed { get; set; }

        public static string arguments { get; set; }

        public static string InputFileName { get; set; }

        // public static string output { get; set; }
        public static string check_output { get; set; }

        public static int ffmpeg_pid { get; set; }
        public static CancellationTokenSource ctoken { get; set; }
        public abstract string AddParam(string locatePath);

        public static string InitialDirectory = Directory.GetCurrentDirectory();
        public static string InputDirectory { get; set; }
        public static string OutputDirectory { get; set; }
        public static string InputSelectorDirectory { get; set; }
        public static string OutputSelectorDirectory { get; set; }

        public static string endFileNameStrings { get; set; }

        public static string isUsedOriginalArgument { get; set; }


        public static bool isAutoScroll { get; set; }
        public static bool isPaused { get; set; }

        

        public static class ButtonNameField
        {
            public const string Convert_DropButton = "Convert_DropButton";
            public const string Directory_DropButon = "Directory_DropButon";
            public const string OutputButton = "OutputButton";

            public const string _openButton =   "_openButton";

        }

        public static class ControlField
        {

            public const string ParamSelector = "ParamSelector";
            public const string OutputSelector = "OutputSelector";
            public const string InputSelector = "InputSelector";


        }
        struct ControlFieldStruct
        {
            public const string ParamSelector = "ParamSelector";
            public const string OutputSelector = "OutputSelector";
            public const string InputSelector = "InputSelector";

        }

    }
}
