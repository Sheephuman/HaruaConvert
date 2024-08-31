using HaruaConvert.HaruaInterFace;
using HaruaConvert.Parameter;
using Microsoft.WindowsAPICodePack.Dialogs;
using WpfApp3;
using WpfApp3.Parameter;


namespace HaruaConvert
{

    /// <summary>
    /// /共通 ダイアログ表示用クラス
    /// </summary>
    public class CommonOpenDialogClass: IDisposableBase
    {
        public CommonOpenDialogClass(bool _isFolder, string _initializeDirectory) 
        {
            this.Dispose(false);
            disposed= false;
          
           initializeDirectory = _initializeDirectory;

            isFolder = _isFolder;}


        //Completee Construct Menber
        string initializeDirectory { get; }
        bool isFolder { get;  }




        public CommonOpenFileDialog dialog { get; set; }
        public string opFileName { get; set; }


        public CommonFileDialogResult CommonOpens()
        {
            CommonFileDialogResult result;


            //Link ButtonName to InitakDirectory
            switch (ClassShearingMenbers.ButtonName)
            {
                case StaticParamField.ButtonNameField.Convert_DropButton:
                case StaticParamField.ButtonNameField.Directory_DropButon:
                    StaticParamField.InitialDirectory = StaticParamField.Maintab_InputDirectory;
                    break;


                case StaticParamField.ButtonNameField.OutputButton:
                    StaticParamField.InitialDirectory = StaticParamField.MainTab_OutputDirectory;
                    break;
                case StaticParamField.ControlField.InputSelector:
                    StaticParamField.InitialDirectory = StaticParamField.ParamTab_InputSelectorDirectory;
                    break;
                case StaticParamField.ControlField.OutputSelector:
                    StaticParamField.InitialDirectory = StaticParamField.ParamTab_OutputSelectorDirectory;
                    break;
            }





            using (dialog = new CommonOpenFileDialog()
            {
                Title = "フォルダを選択してください",

                InitialDirectory = initializeDirectory,

                AllowPropertyEditing = true,
                NavigateToShortcut = true,


                // フォルダ選択モードにする
                IsFolderPicker = isFolder,
            })

                // フォルダ選択モードではない
                if (!isFolder)
                    dialog.Filters.Add(new CommonFileDialogFilter("Movie file", "Movie File, *.mp4,*.flv, *.mov, *.3gp")) ;




            result = dialog.ShowDialog();



            if (result != CommonFileDialogResult.Ok)
            {
                return result;
            }
            opFileName = dialog.FileName;

            return result;
        }

    }
}
