using HaruaConvert.HaruaServise;
using HaruaConvert.Parameter;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;
using WpfApp3.Parameter;

namespace HaruaConvert
{
    internal class Selector_OpenMethodClass
    {
        public Selector_OpenMethodClass(MainWindow _main)
        {
            main = _main;

        }


        MainWindow main { get; set; }


        public CommonFileDialogResult Selector_ComonOpenMethod(bool isFolder, FileSelector selector)
        {

            //  ClassShearingMenbers.ButtonName = selector.Name;




            if (selector.Name == ParamField.ControlField.InputSelector)
            {

                main.ofc = new CommonOpenDialogClass(isFolder, ParamField.ParamTab_InputSelectorDirectory);


                ParamField.InitialDirectory = string.Empty;

                ParamField.InitialDirectory = ParamField.ParamTab_InputSelectorDirectory;
            }
            else if (selector.Name == ParamField.ControlField.OutputSelector)
            {
                main.ofc = new CommonOpenDialogClass(isFolder, ParamField.ParamTab_OutputSelectorDirectory);

                ParamField.InitialDirectory = string.Empty;

                ParamField.InitialDirectory = ParamField.ParamTab_OutputSelectorDirectory;
            }

            var commons = main.ofc.CommonOpens();

            main.outputFileName = main.ofc.opFileName;
            main.outputFileName = Path.ChangeExtension(main.outputFileName, null);


            if (commons == CommonFileDialogResult.Cancel)
                return commons;




            if (isFolder) //Clicked OutputSelector 
            {


                //Update ParamTab_OutputSelectorDirectory
                //  ParamField.ParamTab_OutputSelectorDirectory = ofc.opFileName;


                ParamField.ParamTab_OutputSelectorDirectory = main.outputFileName;

                //string dateNows = DateTime.Now.ToString("MM'-'dd'-'yyyy", CultureInfo.CurrentCulture);



                selector.FilePathBox.Text = ParamField.ParamTab_OutputSelectorDirectory + "\\" + main.paramField.outputFileName_withoutEx + ClassShearingMenbers.endString + ".mp4";


            }
            else if (!isFolder) //Clicked InputSelector 
            {
                //Update inputSelectorDirectory
                ParamField.ParamTab_InputSelectorDirectory
                    = Path.GetDirectoryName(main.ofc.opFileName);


                string file = ParamField.ParamTab_InputSelectorDirectory;
                main.paramField.outputFileName_withoutEx = Path.GetFileNameWithoutExtension(main.ofc.opFileName);
                selector.FilePathBox.Text = main.ofc.opFileName;

                SetUIEvent ui = new SetUIEvent(main);
                var fileName = ui.OutputFileRename(file,
                            main.paramField.outputFileName_withoutEx, main.harua_View.MainParams[0].endString);
                main.OutputSelector.FilePathBox.Text = fileName;

                main.paramField.check_output = string.Empty; //初期化
                main.paramField.check_output = fileName;

                return commons;
            }





            return commons;
        }



    }
}
