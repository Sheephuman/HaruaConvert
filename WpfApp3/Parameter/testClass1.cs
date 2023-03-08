using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaruaConvert.Parameter
{
    internal class testClass1
    {
        Members_Interface IMenbers;
        public class Members_1
        {
            public string SheepString { get; set; }
            public string SheepString2 { get; set; }
            public string SheepString3 { get; set; }
        }


        public interface Members_Interface
        {
            public string SheepString { get; set; }
            public string SheepString2 { get; set; }
            public string SheepString3 { get; set; }
        }


        string MyCall()
        {
            
            IMenbers.SheepString = "She is sheep";
            return  IMenbers.SheepString;
        }


    }
}
