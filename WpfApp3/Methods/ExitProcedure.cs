using HaruaConvert.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static HaruaConvert.Terminate_ProcessClass;

namespace HaruaConvert.Methods
{
    public class ExitProcedureClass
    {

        MainWindow main { get; }
        public ExitProcedureClass(MainWindow _main)
         {
          main = _main;
        
        }
            

        public void ExitProcedure(object sender , EventArgs e)
        {
            ParamSaveClass paramSaver = new(main);
            paramSaver.ParamSave_Procedure(main.paramField);

            //main.Lw.window_Closed(sender, e);
        }

        //public void mainTerminate_deligateExec(int _target_id)
        //{

        //    KillProcessDeligate kp = Terminate_Process;

        //    kp(_target_id);

        //}


    }
}
