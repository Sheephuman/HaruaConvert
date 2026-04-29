using FFMpegCore.Arguments;
using HaruaConvert.Methods;
using HaruaConvert.Parameter;
using Prism.Mvvm;

namespace HaruaConvert.ViewModel.ffmpegOptions.CheckBox
{

    public sealed class ffmpegDetailsOptionsStateModel : BindableBase
    {

        public bool IsNoAudio
        {
            get => field

                ; set
            {
                SetProperty(ref field, value);
                //CheckBoxの状態はOneWaytoSorceで反映される
            }
        }


    }
}




