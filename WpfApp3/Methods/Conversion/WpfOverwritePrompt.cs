using HaruaConvert.HaruaInterFace;
using System.Windows;

namespace HaruaConvert.Methods.Conversion
{
    public sealed class WpfOverwritePrompt : IOverwritePrompt
    {
        public bool AskOverwriteExistingFile()
        {
            MessageBoxResult msbr = MessageBox.Show(
                "ファイルが存在しますわ。上書きしますか？",
                "メッセージボックス",
                MessageBoxButton.YesNo,
                MessageBoxImage.Asterisk);

            return msbr == MessageBoxResult.Yes;
        }
    }
}
