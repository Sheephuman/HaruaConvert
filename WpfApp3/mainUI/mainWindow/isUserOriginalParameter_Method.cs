using FFMpegCore.Arguments;
using HaruaConvert.Parameter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
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

                //"FileDropButton2"
                if (ButtonNameField._ExecButton == ((Button)sender).Name)
                {


                    #region foreach Scopes
                    foreach (ParamSelector sp in mw.selectorList)
                    {
                        if (sp.SlectorRadio.IsChecked.Value)
                        {
                            mw.baseArguments = sp.ArgumentEditor.Text;
                        }
                    }

                    foreach (var sp in mw.selectorList)
                    {




                        if (sp.SlectorRadio.IsChecked.Value && !string.IsNullOrEmpty(sp.ArgumentEditor.Text))
                        {

                            string inputFile = mw.InputSelector.FilePathBox.Text; ;
                            string outputFile = mw.OutputSelector.FilePathBox.Text;

                            ParamField.ParamTab_OutputSelectorDirectory = Path.GetDirectoryName(outputFile);


                            var dictionary = (Dictionary<string, List<string>>)mw.placeHolderList.ItemsSource;
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
                            mw.baseArguments = inputMatches.Replace(mw.baseArguments, "-i " + @"""" + inputFile + @"""").TrimEnd();


                            //   mw.paramField.check_output = mw.OutputSelector.FilePathBox.Text;



                            var OutputMatches = new Regex("\\" + place_1 + "output" + "\\" + place_2);

                            //Attach Output Path as Converted FileName
                            mw.baseArguments = OutputMatches.Replace(mw.baseArguments,  @"""" + outputFile);


                            string wEscapePlace = string.Empty;
                            string wEscapePlace2 = string.Empty;
                            if (place_1 == "{")
                            {
                                wEscapePlace = place_1 + place_1;
                                wEscapePlace2 = place_2 + place_2;
                                mw.baseArguments = mw.baseArguments.Replace(wEscapePlace + "input" + wEscapePlace2, @"""" + outputFile + @"""");
                                //"\"{{{input}}}}\""
                                mw.baseArguments = "-y " + mw.baseArguments.Replace(wEscapePlace + "output" + wEscapePlace2, @"""" + outputFile);
                            }
                            else
                            {
                                mw.baseArguments = mw.baseArguments.Replace(place_1 + "input" + place_2, @"""" + outputFile + @"""");
                                //"\"{{{input}}}}\""
                                mw.baseArguments = "-y " + mw.baseArguments.Replace(place_1 + "output" + place_2, @"""" + outputFile);


                            }


                            mw.baseArguments += @"""";
                            //mw.param._convertFile = mw.OutputSelector.FilePathBox.Text;

                            mw.th1.DisableComObjectEagerCleanup();
                            //COMオブジェクトの早期クリーンアップを無効にするメソッド

                            if (mw.baseArguments.Contains("%03d", StringComparison.Ordinal))
                            { mw.baseArguments += @""""; }

                            else if (mw.baseArguments.Contains("%04d", StringComparison.Ordinal))
                            {
                                mw.baseArguments += @"""";
                            }

                            mw._arguments = mw.baseArguments;

                            mw.paramField.check_output = outputFile;
                            extention = Path.GetExtension(sp.ArgumentEditor.Text).Replace("\"", "");
                            if (!outputFile.Contains(extention))
                                mw.paramField.check_output += extention;

                            mw._arguments = mw._arguments.TrimEnd();

                            

                            if (!sp.ArgumentEditor.Text.EndsWith(place_1 + "output" + place_2 + extention, StringComparison.CurrentCultureIgnoreCase))
                            {
                                MessageBox.Show($"パラメータ末尾に文字列{place_1}output{place_2}{extention}が入っていなければなりません \n\r　" +
                                    "パラメータの見直しをお願いします");
                                mw.paramField.isSuccessdbuildQuery = false;
                                return false;
                            }
                            else
                                mw.paramField.isSuccessdbuildQuery = true;




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
