using System.Windows;
using System.Runtime.Loader;
using System.IO;
using System.Reflection;
using System;

namespace HaruaConvert
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {

            //    string exeDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            //    string dllDir = Path.Combine(exeDir, "dll");


            //    try
            //    {
            //        string[] fileList = Directory.GetFiles(dllDir);

            //        foreach (string file in fileList)
            //        {
            //            if (Path.GetExtension(file).Equals(".dll", StringComparison.OrdinalIgnoreCase))
            //            {
            //                Assembly.LoadFrom(file);

            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        // 例外処理: ファイルの読み込みやアセンブリのロードに関するエラーが発生した場合の処理
            //        Console.WriteLine($"Error loading assemblies: {ex.Message}");
            //    }
            //}

        }

    }
}
