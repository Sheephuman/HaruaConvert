using HaruaConvert;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HaruaConvert.InterFace
{
    public interface IHaruaInterFace
    {
        //インターフェース
        interface IMainWindwEvents
        {
            void NUDUP_Button_Click(object sender, RoutedEventArgs e);
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
