using System;

namespace HaruaConvert.Methods
{
    public class ExitProcedureClass
    {

        MainWindow main { get; }
        public ExitProcedureClass(MainWindow _main)
        {
            main = _main;

        }


        public void ExitProcedure(object sender, EventArgs e)
        {
            var param = main.paramField;
            main.ParamSave_Procedure(param.isParamEdited, param.isParamEdited);

            //main.Lw.window_Closed(sender, e);
        }

        //public void mainTerminate_deligateExec(int _target_id)
        //{

        //    KillProcessDeligate kp = Terminate_Process;

        //    kp(_target_id);

        //}


    }
}
