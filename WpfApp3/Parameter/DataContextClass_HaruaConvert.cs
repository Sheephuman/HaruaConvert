using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HaruaConvert.Parameter
{
#pragma warning disable CA1707 // 識別子はアンダースコアを含むことはできません
    public class DataContextClass_HaruaConvert
#pragma warning restore CA1707 // 識別子はアンダースコアを含むことはできません
    {

        public string StartQuery { get; set; }
        public string OutputPath { get; set; }
        public string endString { get; set; }

        //public string SourcePathText = "Source File";


        private string sourcePath;
        //原因の切り分けのために例外を投げさせる実装
        public string SourcePathText {             
            get { return sourcePath; } 
            set{
                
                if(string.IsNullOrEmpty(value))
                    {                     
                    throw new ArgumentException("Value is Null"); }                

                else
                    sourcePath = value;
            }           
            
            }

        int propValue;
        public int PropValue
        {
            get { return propValue; }
            set { propValue = value; }
        }





    }
}
