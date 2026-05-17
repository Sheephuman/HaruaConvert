using HaruaConvert.mainUI.QueryCreateWindow;
using HaruaConvert.mainUI.QueryCreateWindow.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace HaruaConvert.userintarface
{
    public partial class QueryCreateWindow : Window
    {
       

        
        private void VideoCodecBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (VideoCodecBox.SelectedValue == null)
                return;

            if (string.IsNullOrEmpty(VideoCodecBox.SelectedValue.ToString()))
                return;


                qf._queryWindowViewModel.ApplyCodecRelation(VideoCodecBox.SelectedValue.ToString());
            qf.UpdateAllInput(FileNameExtentionBox.Text);   
        }



        string indexMake(ComboBox CodecBox)
        {
            string removeText = string.Empty;


            removeText = CodecBox.SelectedItem.ToString();



            var result = removeText.Replace("[", "").Replace("]", "");


            //if (!string.IsNullOrEmpty(result))
            //   qf.VideoCodecStrings = result;


            return result;

        }
    }
}
