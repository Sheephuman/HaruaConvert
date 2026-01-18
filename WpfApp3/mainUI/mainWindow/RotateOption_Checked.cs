using System.Windows;
using System.Windows.Controls;

namespace HaruaConvert
{
    public partial class MainWindow
    {

        /// <summary>
        /// Radio buttonが押されたとき、それ以外のパラメータを消去する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void RotateOption_Checked(object sender, RoutedEventArgs e)
        {
            var radio = sender as RadioButton;



            //RadioButtonをクリックしたときにfalseを再設定する
            switch (radio.Name)
            {
                case "NoRotate":
                    ChekOptionStruct.isNoRotate = true;
                    ChekOptionStruct.isRightRotate = false;
                    ChekOptionStruct.isLeftRotate = false;
                    ChekOptionStruct.isHorizontalRotate = false;
                    break;

                case "Right_Rotate":
                    ChekOptionStruct.isNoRotate = false;
                    ChekOptionStruct.isRightRotate = true;
                    ChekOptionStruct.isLeftRotate = false;
                    ChekOptionStruct.isHorizontalRotate = false;
                    break;

                case "Left_Rotate":
                    ChekOptionStruct.isNoRotate = false;
                    ChekOptionStruct.isRightRotate = false;
                    ChekOptionStruct.isLeftRotate = true;
                    ChekOptionStruct.isHorizontalRotate = false;
                    break;

                case "Horizon_Rotate":
                    ChekOptionStruct.isNoRotate = false;
                    ChekOptionStruct.isRightRotate = false;
                    ChekOptionStruct.isLeftRotate = false;
                    ChekOptionStruct.isHorizontalRotate = true;
                    break;
            }


            ////Remove Arguments
            if (!ChekOptionStruct.isNoRotate)
            { _arguments = _arguments.Replace(" -metadata:s:v:0 rotate=0 ", ""); }

            else if (!ChekOptionStruct.isRightRotate)
            {
                _arguments = _arguments.Replace(" -vf transpose=1 ", "");
            }

            else if (!ChekOptionStruct.isLeftRotate)
            {
                _arguments = _arguments.Replace(" -vf transpose=2 ", "");
            }
            if (!ChekOptionStruct.isHorizontalRotate)
            {
                _arguments = _arguments.Replace(" -vf -vf hflip,vflip ", "");
                
            }

        }


    }
}
