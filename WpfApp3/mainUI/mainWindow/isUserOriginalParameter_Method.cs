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
    internal class isUserOriginalParameter
    {
        private static readonly IOriginalParameterQueryBuilder QueryBuilder = new OriginalParameterQueryBuilder();

        MainWindow mw;

        public isUserOriginalParameter(MainWindow _main)
        {
            mw = _main;
        }

        public bool isUserOriginalParameter_Method(object sender)
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

                OriginalQueryBuildResult result = QueryBuilder.Build(
                    selectors,
                    dictionary,
                    mw.harua_View.MainParams[0].placement,
                    mw.InputSelector.FilePathBox.Text,
                    mw.OutputSelector.FilePathBox.Text,
                    ((Button)sender).Name,
                    ButtonNameField._ExecButton,
                    mw.baseArguments);

                if (!result.IsSuccess)
                {
                    if (!string.IsNullOrEmpty(result.ErrorMessage))
                    {
                        MessageBox.Show(result.ErrorMessage);
                    }

                    if (result.SetQueryBuildFailedFlag)
                    {
                        mw.paramField.isSuccessdbuildQuery = false;
                    }

                    if (!string.IsNullOrEmpty(result.OutputPath))
                    {
                        mw.paramField.check_output = result.OutputPath;
                    }

                    return false;
                }

                if (string.IsNullOrEmpty(result.Arguments))
                {
                    return true;
                }

                mw.th1.DisableComObjectEagerCleanup();
                mw._arguments = result.Arguments;
                mw.paramField.check_output = result.OutputPath;
                mw.paramField.isSuccessdbuildQuery = true;
                if (!string.IsNullOrEmpty(result.ParamTabOutputSelectorDirectory))
                {
                    ParamField.ParamTab_OutputSelectorDirectory = result.ParamTabOutputSelectorDirectory;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
    }
}
