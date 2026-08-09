using HaruaConvert.Json;
using HaruaConvert.mainUI.QueryCreateWindow.ViewModel;
using System.Globalization;

namespace HaruaConvert.Methods
{
    public sealed class AppearanceSettingsApplier
    {
        public void ApplyToMainWindow(MainWindow main, AppearanceSettings appearance)
        {
            if (appearance == null || main?.harua_View == null)
            {
                return;
            }

            ApplyToBackColorView(main.harua_View.BackColorView, appearance);
            ApplyBackImageOpacity(main, appearance.BackImageOpacity);
        }

        private void ApplyBackImageOpacity(MainWindow main, double opacity)
        {
            if (main.harua_View.MainParams.Count > 0)
            {
                main.harua_View.MainParams[0].BackImageOpacity = opacity;
            }

            // バインドだけでは ImageBrush / Slider に届かないことがあるため、以前と同様に UI へ直接反映する。
            if (main.opacitySlider != null)
            {
                main.opacitySlider.Value = opacity;
            }

            if (main.myBrush != null)
            {
                main.myBrush.Opacity = opacity;
            }
        }

        public void ApplyToBackColorView(BackColorViewModels backColor, AppearanceSettings appearance)
        {
            backColor.BackgroundRed = appearance.BackgroundColor.Red;
            backColor.BackgroundGreen = appearance.BackgroundColor.Green;
            backColor.BackgroundBlue = appearance.BackgroundColor.Blue;
            backColor.Lightness = appearance.Lightness;
            backColor.Saturation = appearance.Saturation;
        }

        public void CaptureAppearanceFromUi(MainWindow main)
        {
            if (main?.harua_View == null)
            {
                return;
            }

            var backColor = main.harua_View.BackColorView;

            if (main.LightnessSlider != null)
            {
                backColor.Lightness = main.LightnessSlider.Value;
            }

            if (main.SaturationSlider != null)
            {
                backColor.Saturation = main.SaturationSlider.Value;
            }

            if (main.opacitySlider != null && main.harua_View.MainParams.Count > 0)
            {
                main.harua_View.MainParams[0].BackImageOpacity = main.opacitySlider.Value;
            }
        }

        public double ResolveBackImageOpacity(MainWindow main, double fallback)
        {
            if (main?.opacitySlider != null)
            {
                return main.opacitySlider.Value;
            }

            if (main?.harua_View?.MainParams.Count > 0)
            {
                return main.harua_View.MainParams[0].BackImageOpacity;
            }

            if (main?.myBrush != null)
            {
                return main.myBrush.Opacity;
            }

            return fallback;
        }

        public void CopyFromBackColorView(AppearanceSettings appearance, BackColorViewModels backColor, double backImageOpacity)
        {
            appearance.BackgroundColor.Red = backColor.BackgroundRed;
            appearance.BackgroundColor.Green = backColor.BackgroundGreen;
            appearance.BackgroundColor.Blue = backColor.BackgroundBlue;
            appearance.Lightness = backColor.Lightness;
            appearance.Saturation = backColor.Saturation;
            appearance.BackImageOpacity = backImageOpacity;
        }
    }
}
