using HaruaConvert.HaruaInterFace;
using System.IO;

namespace HaruaConvert.Methods.Conversion
{
    public sealed class ConversionOutputConflictEvaluator : IConversionOutputConflictEvaluator
    {
        public bool ShouldPromptOverwrite(string checkOutputPath, bool noDialogCheckBoxChecked)
        {
            bool exists = File.Exists(checkOutputPath);
            if (exists && !noDialogCheckBoxChecked)
            {
                return true;
            }

            return false;
        }
    }
}
