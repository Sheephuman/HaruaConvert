namespace WpfApp3.Parameter
{
    public static class ClassShearingMenbers
    {
        internal static string defaultQuery = "-codec:v libx265 -vf yadif=0:-1:1 -pix_fmt yuv420p -acodec aac -threads 2";

        public static string ButtonName { get; set; }

        public static string endString { get; set; }
        public static string extention { get; internal set; }
    }
}