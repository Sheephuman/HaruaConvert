using HaruaConvert.InterFace;
using HaruaConvert.Parameter;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.IO;
using System.Windows;

namespace HaruaConvert
{
    internal class CommonOpenDialogClass:IHaruaInterFace.IMethods,IDisposable
    {
        public CommonOpenDialogClass(bool _isFolder, string _initializeDirectory) 
        {
            this.Dispose(false);
            disposed= false;
           //if(string.IsNullOrEmpty(_initializeDirectory))
           //     throw new ArgumentException("Value is Null");
            //      throw new ArgumentException("Value is Null");                        
            
           initializeDirectory = _initializeDirectory;

            isFolder = _isFolder;
        }


        //Perfect Construct Menber
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
                    dialog.Filters.Add(new CommonFileDialogFilter("Movie file", "Movie File, *.mp4,*.flv, *.mov"));


            //Link ButtonName to InitakDirectory
            switch (ParamInterfase.ButtonName )
            {
                case ParamInterfase.ButtonNameField.Convert_DropButton:
                   ParamInterfase.InitialDirectory = ParamInterfase.InputDirectory;
                    break;
                case ParamInterfase.ButtonNameField.Directory_DropButon:
                    ParamInterfase.InitialDirectory = ParamInterfase.InputDirectory;
                    break;

                case ParamInterfase.ButtonNameField.OutputButton:
                    ParamInterfase.InitialDirectory = ParamInterfase.OutputDirectory;
                    break;
                case ParamInterfase.ControlField.InputSelector:
                    ParamInterfase.InitialDirectory = ParamInterfase.InputSelectorDirectory;
                    break;
                case ParamInterfase.ControlField.OutputSelector:
                    ParamInterfase.InitialDirectory = ParamInterfase.OutputSelectorDirectory;
                    break;
            }



            result = dialog.ShowDialog();



            if (result != CommonFileDialogResult.Ok)
            {
                return result;
            }
            opFileName = dialog.FileName;




            //switch(ParamInterfase.ButtonName)
            //{
            //    case "FileDrop_ConvertButton":


            //        ParamInterfase.ConvertDirectory = dialog.FileName;
            //       // dialog.InitialDirectory = ParamInterfase.ConvertDirectory;
            //        break;
            //    case "Convert_DropButon":
            //        ParamInterfase.ConvertDirectory = Path.GetDirectoryName(dialog.FileName);
            //       // dialog.InitialDirectory = ParamInterfase.ConvertDirectory;
            //        break;

            //    case "SavelocateButton":
            //        ParamInterfase.OutputDirectory = dialog.FileName;
            //       // dialog.InitialDirectory = ParamInterfase.OutputDirectory;
            //        break;

            //    case "InputSelector":
            //        //ParamInterfase.InputSelectorDirectory = Path.GetDirectoryName(dialog.FileName);
            //        dialog.InitialDirectory = ParamInterfase.InputSelectorDirectory;
            //        break;
            //    case "OutputSelector":
            //        //ParamInterfase.OutputSelectorDirectory = dialog.FileName;
            //        dialog.InitialDirectory = ParamInterfase.OutputSelectorDirectory;
            //        break;
            //}






            return result;
        }

        private readonly IntPtr unmanagedResource;

        private StreamWriter managedResource;

        private bool disposed;

          public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

        protected virtual void Dispose(bool isDisposing)
        {
            if (!this.disposed)
            {
                this.Free(this.unmanagedResource);

                if (isDisposing)
                {
                    if (this.managedResource != null)
                    {
                        this.managedResource.Dispose();
                    }
                }

                this.disposed = true;
            }
        }



        private void Free(IntPtr unmanagedResource)
        {
            MessageBox.Show("CAｌｌ");       
        }
    }
}
