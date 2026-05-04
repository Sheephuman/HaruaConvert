namespace HaruaConvert.HaruaInterFace
{
    /// <summary>
    /// mainFileConvertExec の分岐・準備・上書き経路のオーケストレーション。
    /// </summary>
    public interface IMainFileConversionOrchestrator
    {
        bool Execute(string fullPath, object sender);
    }
}
