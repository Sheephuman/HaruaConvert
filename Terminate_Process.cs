using System;

namespace HaruaConvert
{

    public class Terminate_Process
    {
        public Terminate_Process()
        {
        }


            public void Terminate_Process()
            {
                try
                {

                    Process hProcess;
                    hProcess = Process.GetProcessById(ParamInterfase.ffmpeg_pid);
                    using (killp = new Process())
                    {
                        killp.StartInfo.FileName = "cmd.exe";

                        killp.StartInfo.CreateNoWindow = true;
                        killp.StartInfo.Arguments = "/c Taskkill /F /pid " + ParamInterfase.ffmpeg_pid;

                        killp.Start();
                        killp.WaitForExit();

                    }


                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message + "Sheep is lady!!");
                }

            }
        }
    }
}