using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HaruaConvert.Parameter
{
    public class MainBindingParam : INotifyPropertyChanged
    {
        //public string iniPath { get; set; }
        public string invisibleText { get; set; }
        public string StartQuery { get; set; }
        public string OutputPath { get; set; }
        public string endString { get; set; }
        public string placement { get; set; }
       // public bool isOpenExplorer { get; set; }
        public ParamField paramField { get; set; }




        double backImeageOpacity;
        public double BackImageOpacity
        {

            get { return backImeageOpacity; }

            set
            {
                if (backImeageOpacity != value)
                {
                    backImeageOpacity = value;
                    OnPropertyChanged();
                }
            }
        }

        private string sourcePath;
        //原因の切り分けのために例外を投げさせる実装
        public string SourcePathText
        {
            get { return sourcePath; }
            set
            {

                //if (string.IsNullOrEmpty(value))
                //{
                //    throw new ArgumentException("Value is Null");
                //}

                //else
                sourcePath = value;
            }

        }
        // INotifyPropertyChangedの実装
        public event PropertyChangedEventHandler PropertyChanged;

        // プロパティ変更通知を行うためのメソッド
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
