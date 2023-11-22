using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WpfApp3;

namespace HaruaConvert
{
    internal class Alternate_FileExsists: IDisposableBase
    {
        bool isEqual;
      



        public bool FileExsists(string convertFileName)
        {
            
            var getFilesLArray = getFileNames(Path.GetDirectoryName(convertFileName));


            isEqual = getFilesLArray.SequenceEqual(getFilesLArray);
            foreach (string targetList in getFilesLArray)
            {
                isEqual = targetList.SequenceEqual(convertFileName);

                if (isEqual)
                    return isEqual;
            }



            return isEqual;
        }

        string[] getFileNames(string targetForder)
        {
            string[] destinationFiles;


            string target = targetForder;

            destinationFiles = Directory.GetFiles(target);

            return destinationFiles;
        }


    }
}
