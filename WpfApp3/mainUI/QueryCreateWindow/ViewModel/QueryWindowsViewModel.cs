using HaruaConvert.QueryBuilder;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace HaruaConvert.mainUI.QueryCreateWindow.ViewModel
{
    public class QueryWindowViewModel : BindableBase
    {
        private readonly MakeOptimalName _codecResolver = new MakeOptimalName();

        QueryField _queryField { get; }

        public QueryWindowViewModel(QueryField queryField)
        {
            _queryField = queryField;

            VideoContainerName = string.Empty;
             AudioName = string.Empty;
            // 初期値
            SelectedFileExtension = ".mp4";
        }

        /* ---------- ItemsSource ---------- */
        // QueryWindowViewModel.cs に追加
        private ObservableCollection<string> _fileExtensions = new()
{
    ".mp4", ".mkv", ".webm", ".mov", ".avi", ".ts", ".flv", ".wmv"
    // 必要に応じて増やす
};

        public ObservableCollection<string> FileExtensions
        {
            get => _fileExtensions;

            set => SetProperty(ref _fileExtensions, value);
            // set 不要（固定リストなら）
        }

        // 既存のプロパティ（こっちをメインで使う）

        private string _selectedFileExtension;
        public string SelectedFileExtension
        {
            get => _selectedFileExtension;
            set
            {
                if (SetProperty(ref _selectedFileExtension, value))
                {
                    _queryField.UpdateAllInput(value);
                }
            }
        }



        public string AudioName
        {
            get => field;
            set => SetProperty(ref field, value);
        }
        

        public string VideoContainerName
        {
            get => field;
            set => SetProperty(ref field, value);
        }


        /* ---------- 内部ロジック ---------- */

        public void ApplyCodecRelation(string videoCodec)
        {
           _queryField.AudioCodecStrings =
                _codecResolver.MakeOptimalVideotoAudioCodecs(videoCodec);

            string getCodecNamed = 
                _codecResolver.MakeOptionalVideoContainer(videoCodec);
            SelectedFileExtension = getCodecNamed;

            _queryField.UpdateAllInput(getCodecNamed);

            if (!string.IsNullOrEmpty(AudioName) && !string.IsNullOrEmpty(VideoContainerName))
            {
                Debug.WriteLine(
                    $"[ApplyCodecRelation] videoCodec={videoCodec}, " +
                    $"audioName={AudioName}, " +
                    $"container={VideoContainerName}");
            }
        }

    }
}