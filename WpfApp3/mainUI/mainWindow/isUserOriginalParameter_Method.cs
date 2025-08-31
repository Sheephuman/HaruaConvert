using HaruaConvert.Parameter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using static HaruaConvert.Parameter.ParamField;

namespace HaruaConvert.mainUI.mainWindow
{
    internal class isUserOriginalParameter
    {

        MainWindow mw;
        string place_1 { get; set; }
        string place_2;
        public isUserOriginalParameter(MainWindow _main)
        {
            mw = _main;
            place_1 = string.Empty;
            place_2 = string.Empty;
        }

        // EscapePath escapes;

        public bool isUserOriginalParameter_Method(object sender)
        {
            try
            {
                string extention = string.Empty;
                string baseArguments = string.Empty;
                //"FileDropButton2"
                if (ButtonNameField._ExecButton == ((Button)sender).Name)
                {


                    #region foreach Scopes
                    foreach (ParamSelector sp in mw.selectorList)
                    {
                        if (sp.SlectorRadio.IsChecked.Value)
                        {
                            baseArguments = sp.ArgumentEditor.Text;
                        }
                    }

                    foreach (var sp in mw.selectorList)
                    {
                        if (sp.SlectorRadio.IsChecked.Value && !string.IsNullOrEmpty(sp.ArgumentEditor.Text))
                        {

                            string inputFile = mw.InputSelector.FilePathBox.Text; ;
                            string outputFile = mw.OutputSelector.FilePathBox.Text;

                            ParamField.ParamTab_OutputSelectorDirectory = Path.GetDirectoryName(outputFile);


                            var dictionary = (Dictionary<string, List<string>>)
                           mw.placeHolderList.ItemsSource;


                            if (string.IsNullOrEmpty(mw.harua_View.MainParams[0].placement))
                            {
                                MessageBox.Show("添え字が選択されていないわ");
                                return false;
                            }




                            place_1 = dictionary[mw.harua_View.MainParams[0].placement][0];
                            place_2 = dictionary[mw.harua_View.MainParams[0].placement][1];





                            if (inputFile.Contains(place_1) || inputFile.Contains(place_2))
                            {
                                MessageBox.Show("ファイル名に変換対象の添え字が使われているわ\r\n ファイル名を修正してね");
                                return false;
                            }

                            var inputMatches = new Regex("\\" + place_1 + "input" + "\\" + place_2);
                            baseArguments = inputMatches.Replace(baseArguments, "-i " + @"""" + inputFile + @"""").TrimEnd();


                            //事前に変換対象の拡張子を抜き出す
                            // 正規表現にマッチする箇所を探索
                            var extentionMathes = Regex.Match(baseArguments, "\\.\\w+(?=\\s*$)");
                            ///\.：ドット（.）をリテラルとしてマッチ
                            //////\w +：英数字やアンダースコア1文字以上（拡張子部分）
                            ///(?= "\s*$)：直後にダブルクォートと末尾（もしくは空白+末尾）が続く位置だけを対象にする「肯定の先読み」..


                            extention = extentionMathes.Success ? extentionMathes.Value : string.Empty;

                            //   mw.paramField.check_output = mw.OutputSelector.FilePathBox.Text;



                            var OutputMatches = new Regex("\\" + place_1 + "output" + "\\" + place_2);

                            //Attach Output Path as Converted FileName
                            baseArguments = OutputMatches.Replace(baseArguments, @"""" + outputFile);


                            string wEscapePlace = string.Empty;
                            string wEscapePlace2 = string.Empty;
                            if (place_1 == "{")
                            {
                                wEscapePlace = place_1 + place_1;
                                wEscapePlace2 = place_2 + place_2;
                                baseArguments = baseArguments.Replace(wEscapePlace + "input" + wEscapePlace2, @"""" + outputFile + @"""");
                                //"\"{{{input}}}}\""
                                baseArguments = "-y " + baseArguments.Replace(wEscapePlace + "output" + wEscapePlace2, @"""" + outputFile);
                            }
                            else
                            {
                                baseArguments = baseArguments.Replace(place_1 + "input" + place_2, @"""" + outputFile + @"""");
                                //"\"{{{input}}}}\""
                                baseArguments = "-y " + baseArguments.Replace(place_1 + "output" + place_2, @"""" + outputFile);


                            }


                            baseArguments += @"""";
                            //mw.param._convertFile = mw.OutputSelector.FilePathBox.Text;

                            mw.th1.DisableComObjectEagerCleanup();
                            //COMオブジェクトの早期クリーンアップを無効にするメソッド

                            if (baseArguments.Contains("%03d", StringComparison.Ordinal))
                            { baseArguments += @""""; }

                            else if (mw.baseArguments.Contains("%04d", StringComparison.Ordinal))
                            {
                                baseArguments += @"""";
                            }

                            mw._arguments = baseArguments;

                            mw.paramField.check_output = outputFile + extention;

                            ///\. ：ドット（.）をエスケープして文字としてマッチ
                            //\w + ：1文字以上の英数字やアンダースコアにマッチ（拡張子の本体）
                            //$ ：文字列の末尾にマッチ

                            string targetHolder = place_1 + "output" + place_2 + extention;


                            mw._arguments = mw._arguments.TrimEnd();

                            //extention = baseArguments.EndsWith(targetHolder + extention, StringComparison.CurrentCulture) ? Path.GetExtension(Path.GetFileName(baseArguments)) : string.Empty;




                            if (!sp.ArgumentEditor.Text.EndsWith(targetHolder, StringComparison.CurrentCulture))
                            {
                                MessageBox.Show(@"パラメータ末尾に文字列\r\n
                               {targetHolder}{extention}が入っていなければなりません\r\n
                                  パラメータの見直しをお願いします");
                                mw.paramField.isSuccessdbuildQuery = false;
                                return false;
                            }
                            else
                            {
                                mw.paramField.isSuccessdbuildQuery = true;

                            }



                        }



                    }
                    #endregion


                }
                return true;

            }
            catch (KeyNotFoundException ex)
            {
                MessageBox.Show("添え字が選択されていないわ \r\n" + ex.Message);
                return false;
            }
        }

    }
}
