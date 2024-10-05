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
                case ParamField.ButtonNameField.Convert_DropButton:
                case ParamField.ButtonNameField.Directory_DropButon:
                    ParamField.InitialDirectory = ParamField.Maintab_InputDirectory;
                    break;


                case ParamField.ButtonNameField.OutputButton:
                    ParamField.InitialDirectory = ParamField.MainTab_OutputDirectory;
                    break;
                case ParamField.ControlField.InputSelector:
                    ParamField.InitialDirectory = ParamField.ParamTab_InputSelectorDirectory;
                    break;
                case ParamField.ControlField.OutputSelector:
                    ParamField.InitialDirectory = ParamField.ParamTab_OutputSelectorDirectory;
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
                {
                    dialog.Filters.Add(new CommonFileDialogFilter("Movie file", "Movie File, *.mp4,*.flv, *.mov, *.3gp"));
                    dialog.Filters.Add(new CommonFileDialogFilter("全てのファイル", "*.*"));
                }



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
