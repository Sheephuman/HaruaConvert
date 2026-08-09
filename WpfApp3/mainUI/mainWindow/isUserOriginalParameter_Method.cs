using HaruaConvert.HaruaInterFace;
using HaruaConvert.Methods.Conversion;
using HaruaConvert.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static HaruaConvert.Parameter.ParamField;

namespace HaruaConvert.mainUI.mainWindow
{
    public class UserOriginalParameter
    {
        MainWindow mw;
        public OriginalQueryBuildResult paramResult { get; set; }


        public UserOriginalParameter(MainWindow _main)
        {
            mw = _main;
        }

        public bool UserOriginalParameter_Method(object sender)
        {
            try
            {
                if (ButtonNameField._ExecButton != ((Button)sender).Name)
                {
                    return true;
                }

                var dictionary = (Dictionary<string, List<string>>)mw.placeHolderList.ItemsSource;
                var selectors = mw.selectorList.Select(sp =>
                    new SelectorState(sp.SlectorRadio.IsChecked == true, sp.ArgumentEditor.Text));

                paramResult = new OriginalParameterQueryBuilder().Build(
                   selectors,
                   dictionary,
                   mw.harua_View.MainParams[0].placement,
                   mw.InputSelector.FilePathBox.Text,
                   mw.OutputSelector.FilePathBox.Text,
                   ((Button)sender).Name,
                   ButtonNameField._ExecButton,
                   mw.baseArguments);

                if (!paramResult.IsSuccess)
                {
                    if (!string.IsNullOrEmpty(paramResult.ErrorMessage))
                    {
                        MessageBox.Show(paramResult.ErrorMessage);
                    }

                    if (paramResult.SetQueryBuildFailedFlag)
                    {
                        return true;
                    }

                    if (!string.IsNullOrEmpty(paramResult.OutputPath))
                    {
                        mw.paramField.check_output = paramResult.OutputPath;
                    }

                    return false;
                }



                if (string.IsNullOrEmpty(paramResult.Arguments))
                {
                    return true;
                }


                mw._arguments = paramResult.Arguments;
                mw.paramField.check_output = paramResult.OutputPath;
                mw.paramField.isQueryBuildFailed = true;
                if (!string.IsNullOrEmpty(paramResult.ParamTabOutputSelectorDirectory))
                {
                    ParamField.ParamTab_OutputSelectorDirectory = paramResult.ParamTabOutputSelectorDirectory;
                }

                return false;
            }
            catch (Exception)
            {
                MessageBox.Show(paramResult.OutputPath + "\n" + paramResult.IsSuccess + "\n" + paramResult.ErrorMessage + "\n" + paramResult.Arguments);
                return false;
            }

            finally
            {
                if (mw.th1 != null)
                     mw.th1.DisableComObjectEagerCleanup();
            }
        }
    }
}
