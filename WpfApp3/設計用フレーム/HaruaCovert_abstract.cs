namespace HaruaConvert.設計用フレーム
{


    /// <summary>
    /// 最小限の機能を持つ基底クラス
    /// MainWindowを想定
    /// 1.ffmpegの入出力
    /// 2.ファイルの読み込み
    /// </summary>
    abstract class HaruaCovert_abstract
    {
        string ffmpegQuery { get; set; }
        bool isFfmpegExcecuted { get; set; } = true;

        string endStrings { get; set; }


        public abstract void LoadFile();

        public abstract void CommonOpenDialog();
    }
}
