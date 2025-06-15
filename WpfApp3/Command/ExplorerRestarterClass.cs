using HaruaConvert.Methods;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HaruaConvert.Command
{
    internal class ExplorerRestarterClass
    {
        internal async Task ExPlorerRestarter(Terminate_ProcessClass tpc)
        {
            var getExplorer = Process.GetProcessesByName("Explorer");

            MainWindow.killProcessDell = tpc.Terminate_Process;

            foreach (var process in getExplorer)
            {
                await MainWindow.killProcessDell(process.Id);
            }

            await Task.Delay(1000);

            var sessions = new SessionStartParames("cmd.exe", false, true, "/c start explorer.exe");
            var prosessStart = new ProcessStartClass(
                );                                                            //start explorer.exe

            prosessStart.ProcessStartMethod(sessions);
        }
    }
}