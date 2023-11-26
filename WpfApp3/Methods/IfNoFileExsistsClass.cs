using HaruaConvert.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HaruaConvert.Methods
{
    public class IfNoFileExsistsClass
    {
        public IfNoFileExsistsClass(MainWindow _main) { 
        main = _main;
        }
        MainWindow main;

        public bool IfNoFileExsists()
        {
            //ffmpegが終了している状態のとき
            if (main. paramField.isExitProcessed)
            {

                main.th1.Start();

                main.Lw.Show();
                main.Lw.Activate();
            }
            else
            {
                MessageBox.Show("ffmpeg.exeが実行中なのです");
            }
            return false;

        }


    }
}
