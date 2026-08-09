using System.Collections.Generic;

namespace HaruaConvert.HaruaInterFace
{
 

    public readonly record struct SelectorState(bool IsChecked, string ArgumentText);



    /// <summary>
    /// /C# の record 型（レコード型）は、C# 9.0 で導入されたデータ中心の型です。    主に不変（immutable）なデータモデルを簡潔に書くために作られました。
    /// </summary>
    /// <param name="IsChecked"></param>
    /// <param name="ArgumentText"></param>

    public readonly record struct OriginalQueryBuildResult(
        bool IsSuccess,
        string Arguments,
        string OutputPath,
        string ErrorMessage,
        string ParamTabOutputSelectorDirectory,
        bool SetQueryBuildFailedFlag)
    {
        public bool SetQueryBuildFailedFlag { get; }
    }
}
