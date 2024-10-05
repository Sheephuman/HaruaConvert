using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaruaConvert.QueryBuilder
{
    public class QuearyBuildHelper
    {
        public QuearyBuildHelper(QueryField _qf) {
        
        
        }

       

        

        public string QueryBuilderExecute(string bit)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format(CultureInfo.CurrentCulture, $"-b:v {bit}k "));
            
            

            return sb.ToString(); 
        }

    }
}
