using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace HaruaConvert
{
    internal sealed class SettingIniCreate
    {

        public SettingIniCreate() { }

        [DllImport("kernel32", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi, ExactSpelling = false)]
        internal static extern uint GetPrivateProfileString(
           [MarshalAs(UnmanagedType.LPWStr), In] string lpAppName,
           [MarshalAs(UnmanagedType.LPWStr), In] string lpKeyName,
           [MarshalAs(UnmanagedType.LPWStr), In] string lpDefault,
           [MarshalAs(UnmanagedType.LPWStr), Out] StringBuilder lpReturnString,
           uint nSize,
           [MarshalAs(UnmanagedType.LPWStr), In] string iniFilename);

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
        public static bool TryGetValueOrDefault<T>(string filePath, string sectionName, string keyName, T defaultValue, out T outputValue)
        {

            outputValue = defaultValue;

            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                return false;

            var sb = new char[1024];  //CA1838 の解決
                                      //var ret = SettingIniCreate.GetPrivateProfileString(sectionName, keyName, string.Empty, sb, Convert.ToUInt32(sb), filePath);
            var ret = SettingIniCreate.GetPrivateProfileString(sectionName, keyName, string.Empty, sb, Convert.ToUInt32(sb.Length, System.Globalization.CultureInfo.InvariantCulture), filePath); if (ret == 0 || string.IsNullOrEmpty(sb.ToString())) 　//CA1305 の解決
                return false;

            var conv = TypeDescriptor.GetConverter(typeof(T));
            if (conv == null)
                return false;

            try
            {
                outputValue = (T)conv.ConvertFromString(sb.ToString());
            }
            catch (NotSupportedException)
            {
                return false;
            }
            catch (FormatException)
            {
                return false;
            }

            return true;

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
        public static void SetValue(string filePath, string sectionName, string keyName, string outputValue) =>
            SettingIniCreate.WritePrivateProfileString(sectionName, keyName, outputValue, filePath);


    }
}
