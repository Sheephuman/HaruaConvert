using System.Collections.Generic;
using System.Windows.Controls;

namespace HaruaConvert.HaruaInterFace
{
    public interface IMainWindowSettingsPersistenceService
    {
        void Save(MainWindow main, bool isEdit, bool isChecked, IEnumerable<CheckBox> childCheckBoxList);
    }
}
