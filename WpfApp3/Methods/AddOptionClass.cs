using HaruaConvert;
using HaruaConvert.Parameter;
using HaruaConvert.ViewModel.ffmpegOptions.CheckBox;
using System.Diagnostics;
using System.Windows;
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
        public static string AddOption(string _arguments, bool isCheked)
        {
           
            if (isCheked)　//音声を削除する
            {
                if(!_arguments.Contains("-an"))
                   _arguments += " -an ";
                     
            }
            else //音声を保持する
            {
              
                    _arguments = _arguments.Replace(" -an ", " ");
            
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
