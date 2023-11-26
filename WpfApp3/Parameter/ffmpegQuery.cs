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
        public string AddsetQuery(string input, Harua_ViewModel harua_view)
        {
     //      var dh = new Harua_ViewModel();
            if (string.IsNullOrEmpty(UserParam))
            {
                //var hview = new Harua_ViewModel(_main);
                
                SetQuery = "-i " + $"{input} " + harua_view._Main_Param[0].StartQuery;
            }
            else
                SetQuery = "-i " + $"{input}" + " " + UserParam;


            return SetQuery;
            
        }



    }

}


