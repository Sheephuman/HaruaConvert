using Newtonsoft.Json;
using System.Collections.Generic;

namespace HaruaConvert.Json
{
    public sealed class AppSettings
    {
        [JsonProperty("window")]
        public WindowSettings Window { get; set; } = new();

        [JsonProperty("directories")]
        public DirectorySettings Directories { get; set; } = new();

        [JsonProperty("selectorGenerateCount")]
        public string SelectorGenerateCount { get; set; } = "1";

        [JsonProperty("appearance")]
        public AppearanceSettings Appearance { get; set; } = new();

        [JsonProperty("ffmpegQuery")]
        public FfmpegQuerySettings FfmpegQuery { get; set; } = new();

        [JsonProperty("placeHolderIndex")]
        public int PlaceHolderIndex { get; set; }

        [JsonProperty("selectedParamSelectorIndex")]
        public string SelectedParamSelectorIndex { get; set; } = "0";

        [JsonProperty("selectors")]
        public List<ParamSelectorSettings> Selectors { get; set; } = new();

        [JsonProperty("checkState")]
        public Dictionary<string, bool> CheckState { get; set; } = new();
    }

    public sealed class WindowSettings
    {
        [JsonProperty("left")]
        public double Left { get; set; } = 25;

        [JsonProperty("top")]
        public double Top { get; set; } = 50;
    }

    public sealed class DirectorySettings
    {
        [JsonProperty("convert")]
        public string Convert { get; set; } = string.Empty;

        [JsonProperty("output")]
        public string Output { get; set; } = string.Empty;

        [JsonProperty("outputSelector")]
        public string OutputSelector { get; set; } = string.Empty;

        [JsonProperty("inputSelector")]
        public string InputSelector { get; set; } = string.Empty;
    }

    public sealed class AppearanceSettings
    {
        [JsonProperty("backImageOpacity")]
        public double BackImageOpacity { get; set; } = 0.1;

        [JsonProperty("backgroundColor")]
        public RgbColor BackgroundColor { get; set; } = new();

        [JsonProperty("lightness")]
        public double Lightness { get; set; } = 0.95;

        [JsonProperty("saturation")]
        public double Saturation { get; set; } = 1;
    }

    public sealed class RgbColor
    {
        [JsonProperty("red")]
        public double Red { get; set; } = 255;

        [JsonProperty("green")]
        public double Green { get; set; } = 248;

        [JsonProperty("blue")]
        public double Blue { get; set; } = 235;
    }

    public sealed class FfmpegQuerySettings
    {
        [JsonProperty("baseQuery")]
        public string BaseQuery { get; set; } = string.Empty;

        [JsonProperty("endStrings")]
        public string EndStrings { get; set; } = "_Harua";
    }

    public sealed class ParamSelectorSettings
    {
        [JsonProperty("arguments")]
        public string Arguments { get; set; } = string.Empty;

        [JsonProperty("parameterLabel")]
        public string ParameterLabel { get; set; } = "パラメータ名";
    }
}
