using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaruaConvert.Parameter
{
    public class IniSettingsConst
    {

        public const string OutputDirectory = "MainTab_OutputDirectory";
        public const string OutputSelectorDirectory = "ParamTab_OutputSelectorDirectory";
        public const string InputSelectorDirectory = "ParamTab_InputSelectorDirectory";
        public const string ConvertDirectory = "ConvertDirectory";
        public int Selectors { get; set; }
        public const string MainUIBackGrounColor_Red = nameof(MainUIBackGrounColor_Red);
        public const string MainUIBackGrounColor_Green = nameof(MainUIBackGrounColor_Green);
        public const string MainUIBackGrounColor_Blue  = nameof(MainUIBackGrounColor_Blue);

        public const string Apperance = nameof(Apperance);

        public const string Selector_Generate = "Selector_Generate";

        public const string Arguments_ = "Arguments_";
        public const string ParameterLabel = "ParameterLabel";
        public const string BackImageOpacity = nameof(BackImageOpacity);
        public const string MainUIBackGrounColor = nameof(MainUIBackGrounColor);
    }
}
