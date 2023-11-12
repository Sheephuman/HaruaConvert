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
    internal class CommonOpenDialogClass: IDisposableBase,IHaruaInterFace.IMethods
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




        public CommonOpenFileDialog dialog;
        public string opFileName;


        public CommonFileDialogResult CommonOpens()
        {
            CommonFileDialogResult result;
            

            using (dialog = new CommonOpenFileDialog()
            {
                Title = "フォルダを選択してください",

                InitialDirectory = initializeDirectory,

                AllowPropertyEditing = true,
                NavigateToShortcut = true,


                // フォルダ選択モードにする
                IsFolderPicker = isFolder,
            })


            if (!isFolder)
                    dialog.Filters.Add(new CommonFileDialogFilter("Movie file", "Movie File, *.mp4,*.flv, *.mov")) ;




            //Link ButtonName to InitakDirectory
            switch (ClassShearingMenbers.ButtonName )
            {
                case ParamField.ButtonNameField.Convert_DropButton:                    
                case ParamField.ButtonNameField.Directory_DropButon:
                    ParamField.InitialDirectory = ParamField.InputDirectory;
                    break;


                case ParamField.ButtonNameField.OutputButton:
                    ParamField.InitialDirectory = ParamField.OutputDirectory;
                    break;
                case ParamField.ControlField.InputSelector:
                    ParamField.InitialDirectory = ParamField.InputSelectorDirectory;
                    break;
                case ParamField.ControlField.OutputSelector:
                    ParamField.InitialDirectory = ParamField.OutputSelectorDirectory;
                    break;
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
