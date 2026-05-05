using System.Windows.Controls;

namespace HaruaConvert.HaruaInterFace
{
    public interface IMainWindowUiDataLoaderService
    {
        bool ApplySelectorInitialValues(MainWindow main, bool firstSet);

        void LoadCommandHistoryItems(ComboBox paramText);

        int LoadPlaceholderIndex(string iniPath);
    }
}
