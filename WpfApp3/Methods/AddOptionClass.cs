using HaruaConvert;

namespace HaruaConvert.Methods
{
    public class AddOptionClass
    {

        MainWindow main;
        public AddOptionClass(MainWindow _main)
        {

            main = _main;
        }



        /// <summary>
        ///一連のオプションを追加するかどうか
        /// </summary>
        /// <param name="_arguments"></param>
        /// <returns></returns>
        public static string AddOption(string _arguments)
        {

            if (MainWindow.ChekOptionStruct.isAudio)　//チェックすると音声無し
            {
                _arguments += " -an ";
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
                _arguments += " -vf transpose=3 ";
            }




            return _arguments;
        }

    }
}
