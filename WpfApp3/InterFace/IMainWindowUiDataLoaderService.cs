using HaruaConvert.Json;
using System.Collections.Generic;
using System.Windows.Controls;

namespace HaruaConvert.HaruaInterFace
{
    public interface IMainWindowUiDataLoaderService
    {
        bool ApplySelectorInitialValues(MainWindow main, bool firstSet);

        void LoadCommandHistoryItems(ComboBox paramText);

        void LoadCherkRueles(Dictionary<string, QueryCheckRules> rulesQuery);


        int LoadPlaceholderIndex(AppSettingsStore store);
    }
}
