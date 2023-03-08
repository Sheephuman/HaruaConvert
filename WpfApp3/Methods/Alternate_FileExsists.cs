using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HaruaConvert
{
    public class Alternate_FileExsists
    {
        bool isEqual;
        //      public bool FileExsists(string convertFileName)
        //       {
        //           Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // memo: Shift-JISを扱うためのおまじない

        //           Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");
        //           byte[] conbertBytes = sjisEnc.GetBytes(convertFileName);
        //           var tempBytes = new List<byte[]>();
        //           tempBytes.Add(conbertBytes);

        //           var destinationFileBytes = new List<byte[]>();
        //           destinationFileBytes.Add(getFileNames_convertBytes(Path.GetDirectoryName(convertFileName)));
        //;


        //           //  destinationFileBytes = getFileNames_convertBytes(Path.GetDirectoryName(convertFileName));


        //           isEqual = destinationFileBytes.SequenceEqual(tempBytes); 

        //           return isEqual;
        //       }


        //   /// <summary>
        //   /// 保存先フォルダの全ファイル名を取得してバイト配列に変換
        //   /// </summary>
        //   /// <param name="targetForder"></param>
        //   /// <returns></returns>
        //       byte[] getFileNames_convertBytes(string targetForder)
        //       {


        //           string[] destinationFiles = Directory.GetFiles(targetForder);
        //           var bytes = new byte[destinationFiles.GetLength(0)];

        //           Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");
        //           foreach ( string file in destinationFiles)
        //           {

        //               bytes = sjisEnc.GetBytes(file);

        //           }

        //           return bytes;
        //       }


        public bool FileExsists(string convertFileName)
        {


            var getFilesLArray = getFileNames(Path.GetDirectoryName(convertFileName));



            var getFileList = getFilesLArray.ToList();

            isEqual = getFileList.Any(target =>
                 target.SequenceEqual(convertFileName)
             );


            //      isEqual = getFileList.SequenceEqual(getFilesLArray);
            //foreach (string targetList in getFilesLArray)
            //{
            //    isEqual = targetList.SequenceEqual(convertFileName);

            //    if (isEqual)
            //        return isEqual;
            //}
            return isEqual;
        }

        string[] getFileNames(string targetForder)
        {
            string[] destinationFiles = null;


            string target = targetForder;

            destinationFiles = Directory.GetFiles(target);

            return destinationFiles;
        }


    }
}
