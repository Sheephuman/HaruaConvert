using HaruaConvert.Parameter;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace HaruaConvert.mainUI.QueryCreateWindow.LogWindow
{


    public interface IOpenExplorer
    {
        public void OpenExplorer(ParamField paramField);
    }


    public class OpernExplorerClass : IOpenExplorer
    {
        dynamic WindowCounter(dynamic windows, dynamic explorer, string normalizedOpenPath)
        {
            for (int i = 0; i < windows.Count; i++)
            {
                dynamic window = windows.Item(i);

                Uri uri = new Uri(window.LocationURL);
                string locationPath = uri.LocalPath; // OS形式のフォルダパスに変換

                Debug.WriteLine(locationPath + ":" + normalizedOpenPath);
                string fileDir = Path.GetDirectoryName(normalizedOpenPath);



                if (string.Equals(fileDir, locationPath, StringComparison.OrdinalIgnoreCase))
                {
                    explorer = window;
                    break;
                }
            }

            return explorer;

        }

        public void OpenExplorer(ParamField paramField)
        {


            if (!paramField.isOpenFolder)
                return;

            string filePath = paramField.check_output;
            bool exsist = Path.Exists(filePath);
            if (!exsist)
            {
                paramField.check_output = string.Empty;
                return;
            }

            // COMオブジェクトの操作
            dynamic shell = null;

            try
            {
                // Shell.Applicationのインスタンスを作成
                Type shellType = Type.GetTypeFromProgID("Shell.Application");
                shell = Activator.CreateInstance(shellType);



                // エクスプローラーウィンドウを検索
                dynamic windows = shell.Windows();
                dynamic explorer = null;




                string normalizedOpenPath = filePath.Replace('\\', '/'); // スラッシュを統一


                string folderName = Path.GetDirectoryName(normalizedOpenPath);

                explorer = WindowCounter(windows, explorer, normalizedOpenPath);


                if (explorer is null)
                    return;


                // フォルダを選択
                bool found = false;

                // 指定パスを開く
                shell.Open(folderName);

                //指定のファイルを開く
                if (paramField.isOpenFile)
                    shell.Open(filePath);

                Thread.Sleep(1000); // ウィンドウが開くまで待機



                dynamic items = explorer.Document.Folder.Items();

                string fileName = Path.GetFileName(filePath);

                explorer.Document.SelectItem(fileName, 1); // 1 = 選択

                Debug.WriteLine($"フォルダ '{folderName}' を選択しました: {filePath}");






                if (!found)
                {
                    Debug.WriteLine($"フォルダ '{folderName}' が見つかりませんでした。");
                    return;
                }





                string homeDir = "C:\\"; // デフォルトのホームディレクトリを設定

                string setArgument = string.IsNullOrEmpty(filePath) ? homeDir : $"/select, \"{filePath}\""; // 

                //Exception


                //using (Process explorerProcess = new Process())
                //{
                //    explorerProcess.StartInfo = new ProcessStartInfo
                //    {
                //        FileName = "explorer", // フルパスで指定せず「explorer」とだけ書く
                //        Arguments = setArgument,
                //        UseShellExecute = true
                //    };

                //    explorerProcess.Start();
                //    //memorySize = explorerProcess.WorkingSet64;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            finally
            {
                // COMオブジェクトの解放
                if (shell != null)
                {
                    Marshal.ReleaseComObject(shell);
                }
            }



        }

        // Debug.WriteLine($"/select, \"{paramField.check_output}\"");

    }
}


