﻿using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Input;
using Windows.Storage.Streams;

namespace HaruaConvert
{
    public sealed class IniCreate
    {

        public IniCreate() { }



        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        internal static extern uint GetPrivateProfileString(
            string lpAppName,
            string lpKeyName,
            string lpDefault,
            [Out] char[] lpReturnString,
            uint nSize,
            string iniFilename);

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

            // 出力値の初期化
            outputValue = defaultValue;

            // ファイルパスの有効性をチェック
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                return false;
            }

            // 読み取りバッファの準備
            char[] buffer = new char[1024];
            var iniCreate = new IniCreate();

            // GetPrivateProfileStringを呼び出して設定値を読み取る
            uint readChars = IniCreate.GetPrivateProfileString(sectionName, keyName, null, buffer, (uint)buffer.Length, filePath);

            // 読み取りが成功したかどうかをチェック
            if (readChars == 0)
            {
                return false; // 読み取りに失敗
            }

            // null終端文字までの内容を文字列に変換
            string resultString = new string(buffer, 0, (int)readChars).TrimEnd('\0');

            // 空文字列のチェック
            if (string.IsNullOrEmpty(resultString))
            {
                return false;
            }

            // 型Tへの変換を試みる
            try
            {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
                if (converter != null && converter.CanConvertFrom(typeof(string)))
                {
                    outputValue = (T)converter.ConvertFromString(resultString);
                    return true; // 変換に成功
                }
            }
            catch
            {
                // 変換に失敗した場合は、ここで処理される
            }

            return false; // 変換に失敗または変換器が見つからない
        }
    

        private static uint GetPrivateProfileString(object section, object key, string v, object buffer, uint length, object filepath)
        {
            throw new NotImplementedException();
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

    }
}
