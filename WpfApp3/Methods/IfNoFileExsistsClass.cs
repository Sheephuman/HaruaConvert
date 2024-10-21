using HaruaConvert.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

        public bool IfNoFileExsists(LogWindow Lw)
        {
            //ffmpegが終了している状態のとき
            if (!main.paramField.isExecuteProcessed)
            {
                main.LogWindowShow();                
                

                main.th1 = new Thread(() => main.ffmpegProsseing());
                main.th1.IsBackground = true; // メインスレッドが終了すると自動的に終了
                main.th1.Start();
               
            }
        
            else
            {
                
                   MessageBox.Show("ffmpeg.exeが実行中なのです");
            }
            return false;

        }


    }
}
