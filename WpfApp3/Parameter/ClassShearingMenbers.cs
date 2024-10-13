using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3.Parameter
{
    public static class ClassShearingMenbers
    {

        public static string ButtonName { get; set; }

        public static string endString { get; set; }

        public static string defaultQuery { get; set; } = "-codec:v libx265 -vf yadif=0:-1:1 -pix_fmt yuv420p -acodec aac -threads 2 ";

    }
}