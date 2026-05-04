namespace HaruaConvert.HaruaInterFace
{
    /// <summary>
    /// 変換実行の UI 側開始処理（ログウィンドウ・スレッド起動）。
    /// 旧 IfNoFileExsistsClass / DialogMethod の Yes 分岐を集約する。
    /// </summary>
    public interface IConversionUiLauncher
    {
        /// <summary>
        /// 上書き確認を出さない経路（旧 IfNoFileExsistsClass）。
        /// 新規に変換を開始した場合 true。既に実行中で開始しなかった場合 false。
        /// </summary>
        bool HandleConversionWhenNoOverwritePromptRequired();

        /// <summary>上書きに同意したあと（旧 DialogMethod の Yes 部分）。</summary>
        void BeginConversionAfterOverwriteAccepted();
    }
}
