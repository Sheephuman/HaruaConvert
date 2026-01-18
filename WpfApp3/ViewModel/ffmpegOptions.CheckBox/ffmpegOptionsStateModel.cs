using Prism.Mvvm;

namespace HaruaConvert.ViewModel.ffmpegOptions.CheckBox
{

    public sealed class ffmpegDetailsOptionsStateModel : BindableBase
    {
        public ffmpegDetailsOptionsStateModel()
        {
        }
        MainWindow _main;


        public bool IsNoAudio
        {
            get => field

                ; set
            {
                SetProperty(ref field, value);
            }
        }


        string _arguments = string.Empty;

        public string ffmpegArguments()
        {

            if (!IsNoAudio)
                  {
                      _arguments = _arguments.Replace("-an", ""); /// Remove audio disable flag
            }
            else if (IsNoAudio && _arguments.StartsWith("-an",System.StringComparison.CurrentCulture))
                        _arguments += "-an"; 
  
            return _arguments;
        }

    }
}




