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
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        public static void ActivateExplorerWindow(dynamic explorer)
        {
            try
            {
                // explorer.HWND でウィンドウハンドルを取得
                IntPtr hwnd = (IntPtr)explorer.HWND;
                SetForegroundWindow(hwnd); // 前面に表示
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Explorer をアクティブにできません: {ex.Message}");
            }
        }


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

                // 既存ウィンドウを探す
                foreach (var window in windows)
                {
                    try
                    {
                        string path = Path.GetFullPath(window.Document.Folder.Self.Path);
                        if (string.Equals(path.TrimEnd('\\'), folderName, StringComparison.OrdinalIgnoreCase))
                        {
                            explorer = window;

                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);

                    }
                }


                string fileName = Path.GetFileName(filePath);

                if (explorer is null)
                {
                    // ウィンドウが無いので新しく開く
                    Process.Start("explorer.exe", "/select," + filePath);
                }

                else
                {

                // 指定パスを開く
                shell.Open(folderName);

                //指定のファイルを開く
                if (paramField.isOpenFile)
                    shell.Open(filePath);

                Thread.Sleep(1000); // ウィンドウが開くまで待機



                    dynamic items = explorer.Document.Folder.Items();

                    foreach (var item in items)
                    {
                        if (string.Equals(item.Name, fileName, StringComparison.OrdinalIgnoreCase))
                        {
                            explorer.Document.SelectItem(item, 0x1); // 1 = SVSI_SELECT

                            break;
                        }




                    }


                    ActivateExplorerWindow(explorer);




                }

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


