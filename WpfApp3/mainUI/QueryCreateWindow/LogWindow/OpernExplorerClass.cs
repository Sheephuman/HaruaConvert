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



        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        public static extern int SHParseDisplayName(
            string name,
            IntPtr bindingContext,
            out IntPtr pidl,
            uint sfgaoIn,
            out uint psfgaoOut);

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



        public void OpenExplorer(ParamField paramField)
        {


            string filePath = paramField.check_output;

            string normalizedOpenPath = filePath.Replace('\\', '/'); // スラッシュを統一

            string folderName = Path.GetDirectoryName(normalizedOpenPath);


            // ファイル PIDL





            if (!paramField.isOpenFolder)
                return;


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




                // 既存ウィンドウを探す
                foreach (var window in windows)
                {
                    try
                    {
                        string appPath = window.FullName as string;
                        string path = Path.GetFullPath(window.Document.Folder.Self.Path);

                        // explorer.exe 以外は除外
                        if (!appPath.EndsWith("explorer.exe", StringComparison.OrdinalIgnoreCase))
                            continue;



                        if (string.Equals(path.TrimEnd('\\'), folderName, StringComparison.OrdinalIgnoreCase))
                        {
                            explorer = window;

                            break;
                        }

                        // ウィンドウをアクティブにする
                        ActivateExplorerWindow(explorer);


                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);

                    }
                }


                string fileName = Path.GetFileName(filePath);

                if (explorer is null)
                {
                    // 指定フォルダを新規に開く
                    shell.Open(folderName);

                }

                else
                {


                    Thread.Sleep(1000); // ウィンドウが開くまで待機



                    // Explorerウィンドウ内のフォルダにあるアイテム一覧を取得
                    dynamic items = explorer.Document.Folder.Items();



                    foreach (var item in items)
                    {
                        // 大文字小文字を区別せずに名前を比較
                        if (string.Equals(item.Name, fileName, StringComparison.OrdinalIgnoreCase))
                        {
                            // 見つかったアイテムを選択状態にする

                            explorer.Document.SelectItem(item, 0x4);   // 既存解除

                            // 0x1 = SVSI_SELECT (選択するだけでフォーカスは移動しない)
                            explorer.Document.SelectItem(item, 0x1);


                            break;
                        }
                    }


                    //指定のファイルを開く
                    if (paramField.isOpenFile)
                        shell.Open(filePath);


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


