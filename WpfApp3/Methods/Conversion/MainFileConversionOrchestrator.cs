using HaruaConvert.HaruaInterFace;
using HaruaConvert.mainUI.mainWindow;
using HaruaConvert.Parameter;
using System;
using System.Windows;
using System.Windows.Controls;
using WpfApp3.Parameter;

namespace HaruaConvert.Methods.Conversion
{
    public sealed class MainFileConversionOrchestrator : IMainFileConversionOrchestrator
    {
        private readonly MainWindow _host;
        private readonly IConversionExecutionPreparer _preparer;
        private readonly IConversionOutputConflictEvaluator _outputConflictEvaluator;
        private readonly Func<IConversionUiLauncher> _conversionUiLauncherFactory;
        public UserOriginalParameter OrigenelParam { get; set; }
        public MainFileConversionOrchestrator(
            MainWindow host,
            IConversionExecutionPreparer preparer,
            IConversionOutputConflictEvaluator outputConflictEvaluator,
            Func<IConversionUiLauncher> conversionUiLauncherFactory)
        {
            _host = host;
            _preparer = preparer;
            _outputConflictEvaluator = outputConflictEvaluator;
            _conversionUiLauncherFactory = conversionUiLauncherFactory;
        }

        public bool Execute(string fullPath, string ButtonName, object sender)
        {
            _host.escapes = new EscapePath();

            _host.baseArguments = string.Empty;

            if (_host._arguments == null)
            {
                _host._arguments = string.Empty;
            }

            

            if (ParamField.ButtonNameField.Convert_DropButton == ButtonName)
            {
                var prepared = _preparer.PrepareDropConversion(
                    fullPath,
                    _host.harua_View,
                    _host.paramField,
                    _host.NoAudio.IsChecked == true,
                    _host.harua_View.OutputPath,
                    _host.paramField.setFile);

                _host.paramField.check_output = prepared.CheckOutputPath;
                _host._arguments = prepared.Arguments;
                _host.escapes = prepared.Escapes;
                _host.param = prepared.ParameterCreator;
            }
            else 
            {
                 OrigenelParam = new UserOriginalParameter(_host);
                _host.paramField.isQueryBuildFailed = OrigenelParam.UserOriginalParameter_Method(sender);

                if (_host.paramField.isQueryBuildFailed)
                {
                    return false;
                }
            }

            bool checker = false;
            bool needsOverwrite = _outputConflictEvaluator.ShouldPromptOverwrite(
                _host.paramField.check_output,
                _host.NoDialogCheck.IsChecked == true);

            try
            {
                IConversionUiLauncher launcher = _conversionUiLauncherFactory();
                checker = needsOverwrite
                    ? _host.DialogMethod()
                    : launcher.TryStartConversion(_host._arguments);

                if (needsOverwrite)
                {
                    _host.paramField.isExecuteProcessed = checker;
                }
                else
                {
                    if (!checker)
                    {
                        return false;
                    }
                }

                if (!_host.paramField.isExecuteProcessed)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException?.Message ?? ex.Message);
                
                return false;
            }
        }
    }
}
