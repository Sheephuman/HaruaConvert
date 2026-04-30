using HaruaConvert.Parameter;

namespace HaruaConvert.HaruaInterFace
{
    public sealed class ConversionPreparationResult
    {
        public string Arguments { get; init; } = string.Empty;

        public string CheckOutputPath { get; init; } = string.Empty;

        public EscapePath Escapes { get; init; } = new EscapePath();

        public ParamCreateClasss ParameterCreator { get; init; } = null!;
    }
}
