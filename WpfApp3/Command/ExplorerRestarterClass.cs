using HaruaConvert.Methods;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;


namespace HaruaConvert.Command
{
    internal class ExplorerRestarterClass
    {
        internal async Task ExPlorerRestarter(Terminate_ProcessClass tpc)
        {
            var getExplorer = Process.GetProcessesByName("Explorer");

            tpc = new Terminate_ProcessClass();
            //new が必要

            MainWindow.killProcessDell = tpc.Terminate_Process;
            try
            {
                foreach (var process in getExplorer)
                {
                    await MainWindow.killProcessDell(process.Id);
                }
            }

            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);


            }
            await Task.Delay(1000);

            var sessions = new SessionStartParames("cmd.exe", false, true, "/c start explorer.exe");
            var prosessStart = new ProcessStartClass(
                );                                                            //start explorer.exe

            prosessStart.ProcessStartMethod(sessions);


        }

    }
}