using HaruaConvert.HaruaInterFace;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace HaruaConvert.Methods.Conversion
{
    public sealed class OriginalParameterQueryBuilder : IOriginalParameterQueryBuilder
    {
        public OriginalQueryBuildResult Build(
            IEnumerable<SelectorState> selectors,
            IDictionary<string, List<string>> placeholders,
            string selectedPlaceholderKey,
            string inputFilePath,
            string outputFilePath,
            string triggerButtonName,
            string executeButtonName,
            string legacyBaseArgumentsForPercentCheck)
        {
            if (executeButtonName != triggerButtonName)
            {
                return new OriginalQueryBuildResult(true, string.Empty, string.Empty, string.Empty, string.Empty, false);
            }

            var list = selectors.ToList();
            string baseArguments = string.Empty;
            foreach (var sp in list)
            {
                if (sp.IsChecked)
                {
                    baseArguments = sp.ArgumentText;
                }
            }

            foreach (var sp in list)
            {
                if (!sp.IsChecked || string.IsNullOrEmpty(sp.ArgumentText))
                {
                    continue;
                }

                try
                {
                    if (string.IsNullOrEmpty(selectedPlaceholderKey))
                    {
                        return new OriginalQueryBuildResult(false, string.Empty, string.Empty, "添え字が選択されていないわ", string.Empty, false);
                    }

                    string place1 = placeholders[selectedPlaceholderKey][0];
                    string place2 = placeholders[selectedPlaceholderKey][1];

                    if (inputFilePath.Contains(place1, StringComparison.Ordinal) || inputFilePath.Contains(place2, StringComparison.Ordinal))
                    {
                        return new OriginalQueryBuildResult(false, string.Empty, string.Empty, "ファイル名に変換対象の添え字が使われているわ\r\n ファイル名を修正してね", string.Empty, false);
                    }

                    var inputMatches = new Regex("\\" + place1 + "input" + "\\" + place2);
                    baseArguments = inputMatches.Replace(baseArguments, "-i " + "\"" + inputFilePath + "\"").TrimEnd();

                    var extentionMathes = Regex.Match(baseArguments, "\\.\\w+(?=\\s*$)");
                    string extention = extentionMathes.Success ? extentionMathes.Value : string.Empty;

                    var outputMatches = new Regex("\\" + place1 + "output" + "\\" + place2);
                    baseArguments = outputMatches.Replace(baseArguments, "\"" + outputFilePath);

                    if (place1 == "{")
                    {
                        string wEscapePlace = place1 + place1;
                        string wEscapePlace2 = place2 + place2;
                        baseArguments = baseArguments.Replace(wEscapePlace + "input" + wEscapePlace2, "\"" + outputFilePath + "\"");
                        baseArguments = "-y " + baseArguments.Replace(wEscapePlace + "output" + wEscapePlace2, "\"" + outputFilePath);
                    }
                    else
                    {
                        baseArguments = baseArguments.Replace(place1 + "input" + place2, "\"" + outputFilePath + "\"");
                        baseArguments = "-y " + baseArguments.Replace(place1 + "output" + place2, "\"" + outputFilePath);
                    }

                    baseArguments += "\"";

                    if (baseArguments.Contains("%03d", StringComparison.Ordinal))
                    {
                        baseArguments += "\"";
                    }
                    else if (!string.IsNullOrEmpty(legacyBaseArgumentsForPercentCheck) &&
                             legacyBaseArgumentsForPercentCheck.Contains("%04d", StringComparison.Ordinal))
                    {
                        baseArguments += "\"";
                    }

                    string checkOutput = outputFilePath + extention;
                    baseArguments = baseArguments.TrimEnd();

                    string targetHolder = place1 + "output" + place2 + extention;
                    if (!sp.ArgumentText.EndsWith(targetHolder, StringComparison.CurrentCulture))
                    {
                        string msg =
                            $"パラメータ末尾に文字列\r\n{targetHolder}{extention}が入っていなければなりません\r\nパラメータの見直しをお願いします";
                        return new OriginalQueryBuildResult(false, string.Empty, checkOutput, msg, Path.GetDirectoryName(outputFilePath) ?? string.Empty, true);
                    }

                    string paramTabOut = Path.GetDirectoryName(outputFilePath) ?? string.Empty;
                    return new OriginalQueryBuildResult(true, baseArguments, checkOutput, string.Empty, paramTabOut, false);
                }
                catch (KeyNotFoundException ex)
                {
                    return new OriginalQueryBuildResult(false, string.Empty, string.Empty, "添え字が選択されていないわ \r\n" + ex.Message, string.Empty, false);
                }
            }

            return new OriginalQueryBuildResult(true, string.Empty, string.Empty, string.Empty, string.Empty, false);
        }
    }
}
