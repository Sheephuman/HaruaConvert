using HaruaConvert.HaruaInterFace;

namespace HaruaConvert.Methods.Conversion
{
    public sealed class ConversionRequestValidator : IConversionRequestValidator
    {
        public string ValidateOriginalExecutionRequest(
            bool isExecuteProcessed,
            string usedOriginalArgument,
            string inputPath,
            string outputPath)
        {
            if (isExecuteProcessed)
            {
                return "ffmpwg.exeが実行中ですわ";
            }

            if (string.IsNullOrEmpty(usedOriginalArgument))
            {
                return "ユーザーパラメータが空欄です";
            }

            if (string.IsNullOrEmpty(inputPath))
            {
                return "入力パスが空欄です";
            }

            if (string.IsNullOrEmpty(outputPath))
            {
                return "出力パスが空欄です";
            }

            return string.Empty;
        }
    }
}
