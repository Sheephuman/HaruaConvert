using HaruaConvert.Parameter;

namespace HaruaConvert
{
    class FfmpegQueryClass
    {

        MainWindow _main;
        public FfmpegQueryClass(MainWindow main)
        {
            _main = main;

        }
        public FfmpegQueryClass(string _ffmepg)
        {
            setQuery = _ffmepg;

        }
        public FfmpegQueryClass(string _ffmepg, string _UserParam)
        {
            setQuery = _ffmepg;
            UserParam = _UserParam;
            
        }
        static string UserParam;
        readonly string setQuery;
        //public static string ffmpegQuery;
        
        

        string SetQuery;


        /// <summary>
        /// 基本パラメータのセット
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string AddsetQuery(string input, string StartQuery)
        {
           var dh = new DataContextClass_HaruaConvert();
            if (string.IsNullOrEmpty(UserParam))
            {

                SetQuery = "-i " + $"{input} " + StartQuery;
            }
            else
                SetQuery = "-i " + $"{input}" + " " + UserParam;
            return SetQuery;
        }



    }

}


