namespace HaruaConvert.HaruaInterFace
{
    public interface IConversionRequestValidator
    {
        /// <summary>
        /// 実行可否を判定し、エラーがある場合はメッセージを返す。問題なければ null/empty。
        /// </summary>
        /// 
        string ValidateOriginalExecutionRequest(
            bool isExecuteProcessed,
            string usedOriginalArgument,
            string inputPath,
            string outputPath);
    }
}
