using System.Windows;

namespace HaruaConvert.Methods
{
    internal class Alternate_GetFileNameClass
    {


        public string Alternate_GetFileName(string _fileName)
        {
            var getFilesNameSt = getSimgleFileNames(_fileName);


            return getFilesNameSt;


        }

        string getSimgleFileNames(string targetFile)
        {

            MessageBox.Show(targetFile.IndexOf("\\").ToString());
            targetFile = targetFile.Remove(0, targetFile.IndexOf(@"\"));


            return targetFile;
        }
    }
}
