using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;

namespace HaruaConvert
{

    /// <summary>
    /// GetPrivateProfileString　UTF-16 LE(Bom付き)しか読み込めない
    /// </summary>
    internal sealed class IniCreate
    {

        public IniCreate()
        {

        }
        [DllImport("kernel32", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
        internal static extern int WritePrivateProfileString(
          [MarshalAs(UnmanagedType.LPWStr), In] string lpAppName,
          [MarshalAs(UnmanagedType.LPWStr), In] string lpKeyName,
          [MarshalAs(UnmanagedType.LPWStr), In] string lpString,
          [MarshalAs(UnmanagedType.LPWStr), In] string lpFileName);
    }



    public static class IniDefinition
    {

        /// <summary>
        /// MarshalAs属性の削除: MarshalAs(UnmanagedType.LPWStr)は
        /// StringBuilderに適用されることが一般的ですが、char[]を
        /// 使用する場合は、この属性が原因で問題が発生することがあり
        /// ます。char[]の場合、CLRは自動的に適切なマーシャリングを行います。
        /// </summary>
        /// <param name="lpAppName"></param>
        /// <param name="lpKeyName"></param>
        /// <param name="lpDefault"></param>
        /// <param name="lpReturnString"></param>
        /// <param name="nSize"></param>
        /// <param name="iniFilename"></param>
        /// <returns></returns>



        /// <summary>
        /// INIファイルからキーの値を取得します
        /// <para>戻り値は, 取得が成功したかどうかを示します</para>
        /// </summary>
        /// <typeparam name="T">データ取得する型</typeparam>
        /// <param name="filePath">ファイルパス</param>
        /// <param name="sectionName">セクション名</param>
        /// <param name="keyName">キー名</param>
        /// <param name="defaultValue">初期値</param>
        /// <param name="outputValue">出力値</param>
        /// <returns>取得の成功有無</returns>
        public static bool TryGetValueOrDefault<T>(
              string filePath,
              string sectionName,
              string keyName,
              T defaultValue,
              out T outputValue)
        {
            // 出力値の初期化
            outputValue = defaultValue;
            try
            {
                // ファイルパスの有効性をチェック
                if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                {
                    return false;
                }

                // 読み取りバッファの準備
                char[] buffer = new char[1024];  // //CA1838 の解決

                // GetPrivateProfileStringを呼び出して設定値を読み取る
                uint readChars = (uint)(object)GetValueOrDefault(filePath, sectionName, keyName, defaultValue);
                //GetPrivateProfileString(sectionName, keyName, null, buffer, (uint)buffer.Length, filePath);


                // 読み取りが成功したかどうかをチェック
                if (readChars == 0)
                {
                    return false;

                }

                // null終端文字までの内容を文字列に変換
                string resultString = new string(buffer, 0, (int)readChars).TrimEnd('\0');


                //   Debug.WriteLine($"INI読み込み: file={filePath}, section={sectionName}, key={keyName}, value='{resultString}'");
                // 空文字列のチェック
                if (string.IsNullOrEmpty(resultString))
                {
                    return false;
                }


                TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
                if (converter != null && converter.CanConvertFrom(typeof(string)))
                {
                    if (typeof(T) == typeof(int))
                    {
                        double d = double.Parse(resultString, CultureInfo.CurrentCulture);
                        outputValue = (T)(object)Convert.ToInt32(d);  // 明示的キャスト
                        return true;

                    }

                    else if (typeof(T) == typeof(bool))
                    {
                        if (bool.TryParse(resultString, out bool b))
                        {
                            outputValue = (T)(object)b;
                            return true;
                        }
                    }
                    else
                    {
                        // それ以外は TypeConverter に任せる
                        outputValue = (T)converter.ConvertFromString(null, CultureInfo.InvariantCulture, resultString);
                        return true;

                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

            return false;
        }



        /// <summary>
        /// INIファイルからキーの値を取得します
        /// </summary>
        /// <typeparam name="T">データ取得する型</typeparam>
        /// <param name="filePath">ファイルパス</param>
        /// <param name="sectionName">セクション名</param>
        /// <param name="keyName">キー名</param>
        /// <param name="defaultValue">初期値</param>
        /// <returns>キー値</returns>
        public static T GetValueOrDefault<T>(string filePath, string sectionName, string keyName, T defaultValue)
        {
            T ret = defaultValue;
            TryGetValueOrDefault(filePath, sectionName, keyName, defaultValue, out ret);

            return ret;
        }

        /// <summary>
        /// INIファイルにデータを書き込みます
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <param name="sectionName">セクション名</param>
        /// <param name="keyName">キー名</param>
        /// <param name="outputValue">出力値</param>
        public static void SetValue(string filePath, string sectionName, string keyName, string outputValue)
        {

            int result = IniCreate.WritePrivateProfileString(sectionName, keyName, outputValue, filePath);
            ;// CA1806の解決
            // WritePrivateProfileString が 0 を返した場合、操作は失敗しています。
            if (result == 0)
            {
                throw new IOException($"Failed to write to INI file. FilePath: {filePath}, SectionName: {sectionName}, KeyName: {keyName}");
            }
        }

        internal static void SetValue(string iniPath, object checkState, string name, string v)
        {
            throw new NotImplementedException();
        }
    }
}
