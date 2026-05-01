namespace HaruaConvert.HaruaInterFace
{
    /// <summary>
    /// 出力パスについて、上書き確認ダイアログを出すべきかを判定する。
    /// </summary>
    public interface IConversionOutputConflictEvaluator
    {
        /// <summary>
        /// ファイルが存在し、かつ「確認なし」がオフのとき true（従来の FileExsosts_and_NoDialogCheck と同値）。
        /// </summary>
        bool ShouldPromptOverwrite(string checkOutputPath, bool noDialogCheckBoxChecked);
    }
}
