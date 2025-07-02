using HaruaConvert.Parameter;
using System.Diagnostics;
using System.IO;

namespace HaruaConvert.Methods
{


    public interface IOpenExplorer
    {
        public void OpenExplorer(ParamField paramField);
    }


    public class OpernExplorerClass : IOpenExplorer
    {


        public void OpenExplorer(ParamField paramField)
        {


            if (!paramField.isOpenFolder)
                return;


            bool exsist = Path.Exists(paramField.check_output);
            if (!exsist)
            {
                paramField.check_output = string.Empty;
            }

            string homeDir = "C:\\"; // デフォルトのホームディレクトリを設定

            string setArgument = string.IsNullOrEmpty(paramField.check_output) ? homeDir : $"/select, \"{paramField.check_output}\""; // 



            using (Process explorerProcess = new Process())
            {
                explorerProcess.StartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "explorer", // フルパスで指定せず「explorer」とだけ書く
                    Arguments = setArgument,
                    UseShellExecute = true
                };

                explorerProcess.Start();
                //memorySize = explorerProcess.WorkingSet64;


            }



            Debug.WriteLine($"/select, \"{paramField.check_output}\"");

        }
    }
}
