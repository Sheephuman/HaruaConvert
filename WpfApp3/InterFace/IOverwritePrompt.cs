namespace HaruaConvert.HaruaInterFace
{
    /// <summary>
    /// 出力ファイルが既に存在するときの上書き確認（UI は実装側に委譲）。
    /// </summary>
    public interface IOverwritePrompt
    {
        /// <summary>上書きしてよい場合 true。</summary>
        bool AskOverwriteExistingFile();
    }
}
