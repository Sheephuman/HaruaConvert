using System.Windows;
using System.Runtime.Loader;
using System.IO;
using System.Reflection;

namespace HaruaConvert
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        
        protected override void OnStartup(StartupEventArgs e)
        {
            var view = new MainWindow()
            {
                DataContext = new MainWindow()
            };
            string exeDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            //string dllPath = Path.Combine(exeDir, ".\\bin\\FFMpegCore.dll");

            string[] fileList = Directory.GetFiles(exeDir + "\\dll");


            //    try
            //    {
            //        string[] fileList = Directory.GetFiles(dllDir);

            foreach (string file in fileList)
            {
                if (file.Contains(".dll"))
                    AssemblyLoadContext.Default.LoadFromAssemblyPath(file);
            }



        }
    }
}
