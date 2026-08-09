

using HaruaConvert.mainUI.QueryCreateWindow.ViewModel;
using HaruaConvert.ViewModel.ffmpegOptions.CheckBox;
using Prism.Mvvm;
using System.Configuration;

namespace HaruaConvert.Parameter
{
    public class MainBindingParam : BindableBase
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

        public double BackImageOpacity { get;
            set => SetProperty(ref field,value);    }

        /// <summary>
        /// 以下、UI通知用　ini読み込み用　BackcGroudColorのRGB値　0~255で指定
        /// </summary>
        public double BackgroundRed { get; set; }

        public double BackgroundGreen { get; set; }

        public double BackgroundBlue { get; set; }

    }
}
