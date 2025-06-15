using System.Diagnostics;

namespace HaruaConvert.Methods
{
    internal class ProcessStartClass
    {
        public ProcessStartClass()
        {

        }
        public void ProcessStartMethod(SessionStartParames prosessStart)
        {
            using (Process targetProcess = new Process())
            {
                targetProcess.StartInfo.FileName = prosessStart.processName;
                targetProcess.StartInfo.UseShellExecute = prosessStart.useShellExcute;

                targetProcess.StartInfo.CreateNoWindow = prosessStart.CreateNoWindow;
                targetProcess.StartInfo.Arguments = prosessStart.Arguments;

                targetProcess.Start();
                // プロセスが終了するのを非同期的に待つ
                targetProcess.WaitForExitAsync(); // これにより、await がサポートされる

            }
        }
    }

    internal class SessionStartParames
    {
        public SessionStartParames(string _processName, bool _useSheelExcute, bool _CreateNoWindow, string _arguments)
        {
            processName = _processName;
            useShellExcute = _useSheelExcute;
            CreateNoWindow = _CreateNoWindow;
            Arguments = _arguments;

        }

        public readonly string processName;

        public readonly bool useShellExcute;

        public readonly bool CreateNoWindow;
        public readonly string Arguments;



    }
}

