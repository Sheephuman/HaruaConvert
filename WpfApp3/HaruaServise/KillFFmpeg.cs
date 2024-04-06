using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaruaConvert.HaruaService
{
    public class KillFFprobe
    {



        public void KillExistingFFprobeProcesses()
        {
            var ffprobeProcesses = Process.GetProcessesByName("ffprobe.exe");
            if (ffprobeProcesses.Length > 0)
            {
                ffprobeProcesses[0].Kill();
            }
        }
    }
}
