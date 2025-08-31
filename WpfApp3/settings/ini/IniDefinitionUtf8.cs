using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace HaruaConvert.settings.ini
{
    public static class IniDefinitionUtf8
    {
        public static uint TryGetValueOrDefault<T>(
            string filePath,
            string sectionName,
            string keyName,
            T defaultValue,
            out T outputValue)
        {






            outputValue = defaultValue;
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                return 0;


            try
            {
                if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                    return 0;

                string[] lines;

                using (var sr = new StreamReader(filePath, System.Text.Encoding.UTF8, detectEncodingFromByteOrderMarks: true))
                {
                    lines = sr.ReadToEnd().Replace("\uFEFF", "").Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                }


                string currentSection = null;

                foreach (var line in lines)
                {
                    string trimmed = line.Trim();

                    // セクション判定
                    if (trimmed.StartsWith("[", StringComparison.CurrentCultureIgnoreCase) && trimmed.EndsWith("]", StringComparison.CurrentCultureIgnoreCase))
                    {
                        currentSection = trimmed.Substring(1, trimmed.Length - 2);
                        continue;
                    }

                    // セクション不一致ならスキップ
                    if (currentSection != sectionName)
                        continue;

                    // キー判定
                    if (trimmed.StartsWith(keyName + "=", StringComparison.OrdinalIgnoreCase))
                    {
                        string valueStr = trimmed.Substring(keyName.Length + 1).Trim();

                        if (string.IsNullOrEmpty(valueStr))
                            return 0;

                        TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
                        if (converter != null && converter.CanConvertFrom(typeof(string)))
                        {
                            if (typeof(T) == typeof(int))
                            {
                                if (double.TryParse(valueStr, NumberStyles.Any, CultureInfo.CurrentCulture, out double d))
                                {
                                    outputValue = (T)(object)Convert.ToInt32(d);
                                    return (uint)valueStr.Length;
                                }
                            }
                            else if (typeof(T) == typeof(bool))
                            {
                                if (bool.TryParse(valueStr, out bool b))
                                {
                                    outputValue = (T)(object)b;
                                    return (uint)valueStr.Length;
                                }
                            }
                            else
                            {
                                outputValue = (T)converter.ConvertFromString(null, CultureInfo.InvariantCulture, valueStr);
                                return (uint)valueStr.Length;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return 0;
        }


    }


}
