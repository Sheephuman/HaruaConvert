using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaruaConvert.Parameter
{
    public class MainBindingParam
    {
        //public string iniPath { get; set; }
        public string invisibleText { get; set; }
        public string StartQuery { get; set; }
        public string OutputPath { get; set; }
        public string endString { get; set; }
        public string placement { get; set; }

        public ParamField paramField { get; set; }



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

    }
}
