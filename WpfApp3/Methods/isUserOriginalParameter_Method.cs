using FFMpegCore.Arguments;
using HaruaConvert.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using static HaruaConvert.Parameter.ParamField;

namespace HaruaConvert.Methods
{
    internal class isUserOriginalParameter
    {

        MainWindow mw;

        public isUserOriginalParameter(MainWindow _main)
        {
            mw = _main;
        
        }

        EscapePath escapes;

       public bool isUserOriginalParameter_Method(object sender)
        {
            //"FileDropButton2"
            if ((ButtonNameField._ExecButton == ((Button)sender).Name))
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
                        
                        
                        var inputMatches = new Regex("\\{" + "input" + "\\}");
                        mw.baseArguments = inputMatches.Replace(mw.baseArguments, @"""" + mw.InputSelector.FilePathBox.Text + @"""");


                        mw.paramField.check_output = mw.OutputSelector.FilePathBox.Text;

                        

                        var OutputMatches = new Regex("\\{" + "output" + "\\}");

                        //Attach Output Path as Converted FileName
                        mw.baseArguments = OutputMatches.Replace(mw.baseArguments, @"""" + mw.OutputSelector.FilePathBox.Text);



                        mw.baseArguments = mw.baseArguments.Replace("{{" + "input" + "}}", @"""" + mw.InputSelector.FilePathBox.Text + @"""");
                        //"\"{{{input}}}}\""
                        mw.baseArguments = mw.baseArguments.Replace("{{" + "output" + "}}", @"""" + mw.OutputSelector.FilePathBox.Text);


                        mw.baseArguments += @"""";
                        //mw.param._convertFile = mw.OutputSelector.FilePathBox.Text;

                        mw.th1.DisableComObjectEagerCleanup();

                        if (mw.baseArguments.Contains("%03d", StringComparison.Ordinal))
                        { mw.baseArguments += @""""; }

                        else if (mw.baseArguments.Contains("%04d", StringComparison.Ordinal))
                        {
                            mw.baseArguments += @"""";
                        }

                        mw._arguments = mw.baseArguments;
                    }
                }
                #endregion
            }
            return true;
        }

    }
}
