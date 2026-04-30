using HaruaConvert.HaruaInterFace;
using HaruaConvert.Parameter;
using System.IO;

namespace HaruaConvert.Methods.Conversion
{
    public sealed class ConversionExecutionPreparer : IConversionExecutionPreparer
    {
        public HaruaInterFace.ConversionPreparationResult PrepareDropConversion(
            string fullPath,
            Harua_ViewModel viewModel,
            ParamField paramField,
            bool isNoAudioChecked,
            string outputPathFromViewModel,
            string selectedFilePath)
        {
            var convertName = new ConvertFileNameClass();

            string checkOutputPath;
            if (!paramField.isOutputButtonChecked)
            {
                checkOutputPath = Path.GetDirectoryName(fullPath) + "\\" +
                                  convertName.ConvertFileName(Path.GetFileName(fullPath), viewModel);
            }
            else
            {
                checkOutputPath = outputPathFromViewModel + "\\" +
                                  convertName.ConvertFileName(Path.GetFileName(selectedFilePath), viewModel);
            }

            var parameterCreator = new ParamCreateClasss(fullPath, checkOutputPath);
            string target = viewModel.MainParams[0].StartQuery;
            string extention = parameterCreator.GetExtentionFileNamepattern(target);

            var escapes = parameterCreator.AddParamEscape(new EscapePath(), extention);
            if (!string.IsNullOrEmpty(extention))
            {
                checkOutputPath = escapes.NonEscape_outputPath;
            }

            var queryBuilder = new FfmpegQueryClass(string.Empty, string.Empty);
            var arguments = queryBuilder.AddsetQuery(escapes.inputPath, viewModel);
            arguments = arguments.Replace("{FileName}" + extention, "");
            arguments = AddOptionClass.AddOption(arguments, isNoAudioChecked) + " " + escapes.outputPath;

            return new HaruaInterFace.ConversionPreparationResult
            {
                Arguments = arguments,
                CheckOutputPath = checkOutputPath,
                Escapes = escapes,
                ParameterCreator = parameterCreator
            };
        }
    }
}
