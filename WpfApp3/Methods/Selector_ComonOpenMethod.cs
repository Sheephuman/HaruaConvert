using HaruaConvert.HaruaServise;
using HaruaConvert.Parameter;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;
using WpfApp3.Parameter;

namespace HaruaConvert.Methods
{
    internal class Selector_OpenMethodClass
    {
        public Selector_OpenMethodClass(MainWindow _main)
        {
            main = _main;

        }


        MainWindow main { get; set; }

#pragma warning disable CA1822 // メンバーを static に設定します
        public CommonFileDialogResult Selector_ComonOpenMethod(bool isFolder, FileSelector selector)
#pragma warning restore CA1822 // メンバーを static に設定します
        {

            //  ClassShearingMenbers.ButtonName = selector.Name;




            if (selector.Name == StaticParamField.ControlField.InputSelector)
            {

                main.ofc = new CommonOpenDialogClass(isFolder, StaticParamField.ParamTab_InputSelectorDirectory);


                StaticParamField.InitialDirectory = string.Empty;

                StaticParamField.InitialDirectory = StaticParamField.ParamTab_InputSelectorDirectory;
            }
            else if (selector.Name == StaticParamField.ControlField.OutputSelector)
            {
                main.ofc = new CommonOpenDialogClass(isFolder, StaticParamField.ParamTab_OutputSelectorDirectory);

                StaticParamField.InitialDirectory = string.Empty;

                StaticParamField.InitialDirectory = StaticParamField.ParamTab_OutputSelectorDirectory;
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


                StaticParamField.ParamTab_OutputSelectorDirectory = main.outputFileName;

                //string dateNows = DateTime.Now.ToString("MM'-'dd'-'yyyy", CultureInfo.CurrentCulture);



                selector.FilePathBox.Text = StaticParamField.ParamTab_OutputSelectorDirectory + "\\" + main.paramField.outputFileName_withoutEx + ClassShearingMenbers.endString + ".mp4";


            }
            else if (!isFolder) //Clicked InputSelector 
            {
                //Update inputSelectorDirectory
                StaticParamField.ParamTab_InputSelectorDirectory
                    = Path.GetDirectoryName(main.ofc.opFileName);


                string file = StaticParamField.ParamTab_InputSelectorDirectory;
                main.paramField.outputFileName_withoutEx = Path.GetFileNameWithoutExtension(main.ofc.opFileName);
                selector.FilePathBox.Text = main.ofc.opFileName;

                SetUIEvent ui = new SetUIEvent(main);
                main.OutputSelector.FilePathBox.Text = ui.OutputFileRename(file,
                            main.paramField.outputFileName_withoutEx, main.harua_View.MainParams[0].endString);



                return commons;
            }





            return commons;
        }



    }
}
