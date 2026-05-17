

using HaruaConvert.mainUI.QueryCreateWindow.ViewModel;
using HaruaConvert.ViewModel.ffmpegOptions.CheckBox;

namespace HaruaConvert.Parameter
{
    public class MainBindingParam
    {
        //public string iniPath { get; set; }
        public string invisibleText { get; set; } = null!;
        public string StartQuery { get; set; } = null!;
        public string OutputPath { get; set; } = null!;
        public string endString { get; set; } = null!;
        public string placement { get; set; }

        

        internal ffmpegDetailsOptionsStateModel ffmpegOptionsStateModel { get; set; }


        private string sourcePath;
        //原因の切り分けのために例外を投げさせる実装
        public string SourcePathText
        {
            get { return sourcePath; }
            set
            {

                //if (string.IsNullOrEmpty(value))
                //{
                //    throw new
                //    ("Value is Null");
                //}

                //else
                sourcePath = value;
            }

        }

        public double BackImageOpacity { get; set; }


      
    }
}
