using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HaruaConvert.UserControls
{
    public  class NumericUpDownAbstract :UserControl
    {
        protected NumericUpDownAbstract() { }

        public static readonly int minvalue = 1;
        public static readonly int maxvalue = 100;
        public const int startvalue = 10;
        public static int selGenerate { get; set; }
    }
}
