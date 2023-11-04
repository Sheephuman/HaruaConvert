using Microsoft.WindowsAPICodePack.Dialogs;
using System.Windows;
using System.Windows.Controls;

namespace HaruaConvert.HaruaInterFace
{
    public interface IHaruaInterFace
    {
        //インターフェース
        interface IMainWindwEvents
        {
            void NUDUP_Button_Click(object sender, RoutedEventArgs e);
            void ArgumentEditor_TextChanged(object sender, TextChangedEventArgs e);
        }
        interface IMouseEvents
        {
           void FileSelector_MouseDown(object sender, RoutedEventArgs e);

            void Directory_DropButon_Click(object sender, RoutedEventArgs e);
        }

        interface IMethods 
        {
            CommonFileDialogResult CommonOpens();

        }
        



    }

    
}
