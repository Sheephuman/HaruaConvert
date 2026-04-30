using HaruaConvert.Parameter;

namespace HaruaConvert.HaruaInterFace
{
    public interface IConversionExecutionPreparer
    {
        HaruaConvert.HaruaInterFace.ConversionPreparationResult PrepareDropConversion(
            string fullPath,
            Harua_ViewModel viewModel,
            ParamField paramField,
            bool isNoAudioChecked,
            string outputPathFromViewModel,
            string selectedFilePath);
    }
}
