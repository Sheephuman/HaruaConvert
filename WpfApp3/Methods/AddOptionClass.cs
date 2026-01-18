using HaruaConvert;
using HaruaConvert.ViewModel.ffmpegOptions.CheckBox;
using Windows.Media;

namespace HaruaConvert.Methods
{
    public class AddOptionClass
    {
        public AddOptionClass()
        {
            
        }

        /// <summary>
        ///一連のオプションを追加するかどうか
        /// </summary>
        /// <param name="_arguments"></param>
        /// <returns></returns>
        public static string AddOption(string _arguments)
        {
            ffmpegDetailsOptionsStateModel _detailsOptions = new ffmpegDetailsOptionsStateModel();


            if (!_detailsOptions.IsNoAudio)　//音声を削除する
            {
                if(_arguments.StartsWith("-an", System.StringComparison.OrdinalIgnoreCase))
                _arguments += " -an ";
            }
            else //音声を保持する
            {
                if (_arguments.StartsWith("-an", System.StringComparison.OrdinalIgnoreCase))
                {
                    _arguments = _arguments.Replace(" -an ", " ");
                }
            }



            if (MainWindow.ChekOptionStruct.isForceFPS)
            {
                _arguments += " -r 30 ";
            }


            if (MainWindow.ChekOptionStruct.isNoRotate)
            {
                _arguments += " -metadata:s:v:0 rotate=0 ";
            }


            if (MainWindow.ChekOptionStruct.isRightRotate) //右に90度回転
            {
                _arguments += " -vf transpose=1 ";
            }

            if (MainWindow.ChekOptionStruct.isLeftRotate)
            {
                _arguments += " -vf transpose=2 ";
            }

            if (MainWindow.ChekOptionStruct.isHorizontalRotate) //180度回転させる
            {
                _arguments += " -vf hflip,vflip ";
            }




            return _arguments;
        }

    }
}
