using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HaruaConvert.Methods
{
    internal class ExplorerRestarterClass
    {
        public ExplorerRestarterClass()
        { }



        private delegate Task ProcessKill_deligate(int targetProcess);

       async public Task ExPlorerRestarter(Terminate_ProcessClass tpc)
        {

            
            tpc = new Terminate_ProcessClass();
            ProcessKill_deligate killProcessDell;
            killProcessDell = tpc.Terminate_Process;

            List<Process> AllExplorerProcesses = new List<Process>();

            Process[] exploreres = Process.GetProcessesByName("explorer");
            foreach (Process ex in exploreres)
            {
                AllExplorerProcesses.Add(ex);
            }




            if (AllExplorerProcesses == null)
            {
                return;
            }
            // 最小メモリサイズのプロセスを取得


                using (tpc = new Terminate_ProcessClass())
            {


                foreach (Process explorer in AllExplorerProcesses)
                    //if (smallestMemoryProcess.WorkingSet64 >= explorer.WorkingSet64)
                    await killProcessDell(explorer.Id);  // 非同期にプロセスを終了

            }
           
            await Task.Delay(1000);
          

                var processStaert = new ProcessStartClass();
                var processParam = new SessionStartParames("cmd.exe", false, true, "/c start explorer.exe");
                processStaert.ProcessStartMethod(processParam);

            
            Task.CompletedTask.Wait();


        }


    }
}
