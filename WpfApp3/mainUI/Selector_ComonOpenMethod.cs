using HaruaConvert.HaruaServise;
using HaruaConvert.Parameter;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;
using WpfApp3.Parameter;

namespace HaruaConvert
{
    internal class Selector_OpenMethodClass
    {
        public Selector_OpenMethodClass(MainWindow _main,ParamField _param)
        {
           param = _param;
        main = _main;
         
        }


        MainWindow main { get; set; }
        ParamField param { get; set; }
      

    public CommonFileDialogResult Selector_ComonOpenMethod(bool isFolder, FileSelector selector)
        {

            //  ClassShearingMenbers.ButtonName = selector.Name;
            var mfc = new mainUIparameter();
            mfc.ofc = new CommonOpenDialogClass(isFolder, ParamField.ParamTab_InputSelectorDirectory);




            if (selector.Name == ParamField.ControlField.InputSelector)
            {
                

                ParamField.InitialDirectory = string.Empty;

                ParamField.InitialDirectory = ParamField.ParamTab_InputSelectorDirectory;
            }
            else if (selector.Name == ParamField.ControlField.OutputSelector)
            {
                mfc.ofc = new CommonOpenDialogClass(isFolder, ParamField.ParamTab_OutputSelectorDirectory);

                ParamField.InitialDirectory = string.Empty;

                ParamField.InitialDirectory = ParamField.ParamTab_OutputSelectorDirectory;
            }

            var commons = mfc.ofc.CommonOpens();

            mfc.outputFileName = mfc.ofc.opFileName;
            mfc.outputFileName = Path.ChangeExtension(mfc.outputFileName, null);


            if (commons == CommonFileDialogResult.Cancel)
                return commons;




            if (isFolder) //Clicked OutputSelector 
            {


                //Update ParamTab_OutputSelectorDirectory
                //  ParamField.ParamTab_OutputSelectorDirectory = ofc.opFileName;


                ParamField.ParamTab_OutputSelectorDirectory = mfc.outputFileName;

                //string dateNows = DateTime.Now.ToString("MM'-'dd'-'yyyy", CultureInfo.CurrentCulture);



                selector.FilePathBox.Text = ParamField.ParamTab_OutputSelectorDirectory + "\\" + param.outputFileName_withoutEx + ClassShearingMenbers.endString + ".mp4";


            }
            else if (!isFolder) //Clicked InputSelector 
            {
                //Update inputSelectorDirectory
                ParamField.ParamTab_InputSelectorDirectory
                    = Path.GetDirectoryName(mfc.ofc.opFileName);


                string file = ParamField.ParamTab_InputSelectorDirectory;
                param.outputFileName_withoutEx = Path.GetFileNameWithoutExtension(mfc.ofc.opFileName);
                selector.FilePathBox.Text = mfc.ofc.opFileName;

                SetUIEvent ui = new SetUIEvent(main);
                var fileName = ui.OutputFileRename(file,param.outputFileName_withoutEx, main.harua_View.MainParams[0].endString);
               main.OutputSelector.FilePathBox.Text = fileName;
                
                param.check_output = string.Empty; //初期化
                param.check_output = fileName;

                return commons;
            }





            return commons;
        }



    }
}
