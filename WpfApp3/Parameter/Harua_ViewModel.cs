using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WpfApp3.Parameter;

namespace HaruaConvert.Parameter
{
#pragma warning disable CA1707 // 識別子はアンダースコアを含むことはできません
    public class Harua_ViewModel : INotifyPropertyChanged
#pragma warning restore CA1707 // 識別子はアンダースコアを含むことはできません
    {
        MainWindow main { get; set; } = null!;
        public Harua_ViewModel(MainWindow _main) 
        {
            
            main = _main;
            _Main_Param = new ObservableCollection<Main_Param>
            {
               new Main_Param { StartQuery = IniDefinition.GetValueOrDefault
                                       (main.paramField.iniPath, "ffmpegQuery", "BaseQuery", "  -b:v 1200k -pix_fmt yuv420p -acodec aac -y -threads 2"),
                OutputPath = ParamField.MainTab_OutputDirectory,
                endString = ClassShearingMenbers.endFileNameStrings,
                SourcePathText = "フォルダ:" + IniDefinition.GetValueOrDefault
                                       (main.paramField.iniPath, "Directory", IniSettingsConst.ConvertDirectory, "Source File"),
                invisibleText = ""
               }
            };


        }

        private ObservableCollection<Main_Param> _main_Param = new ObservableCollection<Main_Param>();


        public ObservableCollection<Main_Param> _Main_Param
        {

            get => _main_Param;
            set
            {
                _main_Param = value;
                RaisePropertyChanged("_main_Param");
            }
        }



        // INotifyPropertyChangedの実装
        public event PropertyChangedEventHandler PropertyChanged;


        public string invisibleText { get; set; }
        public string StartQuery { get; set; }
        public string OutputPath { get; set; }
        public string endString { get; set; }

        //public string SourcePathText = "Source File";


        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string sourcePath;
        //原因の切り分けのために例外を投げさせる実装
        public string SourcePathText {             
            get { return sourcePath; } 
            set{
                
                //if(string.IsNullOrEmpty(value))
                //    {                     
                //    throw new ArgumentException("Value is Null"); }                

                //else
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
