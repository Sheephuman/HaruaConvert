using System.ComponentModel;
using System.Diagnostics.Metrics;

namespace HaruaConvert.ViewModel
{
    internal class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModel() =>
         // データの初期化
         ToDos = new ObservableCollection<ToDo>
         {
                 new ToDo { Task = "タスク1", IsCompleted = false },
                 new ToDo { Task = "タスク2", IsCompleted = true }
         };



    }
}
