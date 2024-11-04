using HaruaConvert.Parameter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit;

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

            bool exsist = Path.Exists(paramField.check_output);
            if (!exsist)
            {
                paramField.check_output = string.Empty;
            }

            if (!string.IsNullOrEmpty(paramField.check_output))
                using (Process explorerProcess = new Process())
                {
                    explorerProcess.StartInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "explorer", // フルパスで指定せず「explorer」とだけ書く
                        Arguments = $"/select, \"{paramField.check_output}\"", // 引数に「/select,」を付ける
                        UseShellExecute = true
                    };

                    explorerProcess.Start();
                    //memorySize = explorerProcess.WorkingSet64;


                }



            else
            {
                MessageBox.Show("ファイルが出力されませんでしたわ");
            }



            Debug.WriteLine($"/select, \"{paramField.check_output}\"");

        }
    }
}
