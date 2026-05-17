using Prism.Mvvm;
using System;
using System.Windows.Media;

namespace HaruaConvert.mainUI.QueryCreateWindow.ViewModel
{
    public class BackColorViewModels : BindableBase
    {
        public Brush ResultColor {
            get { return field; }
            set => SetProperty(ref field, value); } = new SolidColorBrush(Color.FromRgb(253, 238, 240));

        public double BackgroundRed
        {
            get { return field; }
            set
            { SetProperty(ref field, value);
                UpdateComputedBackground();
            }
        }
        public double BackgroundGreen
        {
            get { return field; }
            set
            {
                SetProperty(ref field, value);
                UpdateComputedBackground();
            }
        }
        public double BackgroundBlue
        {
            get { return field; }
            set
            {
                SetProperty(ref field, value);
                UpdateComputedBackground();
            }
        }

        public double Lightness
        {
            get { return field; }
            set
            {
                SetProperty(ref field, value);
                UpdateComputedBackground();
            }
        } = 0.5;

        //  ResultColor = new SolidColorBrush(Color.FromRgb(253, 238, 240)); 

        public double Saturation { get;

            set
            {
                SetProperty(ref field, value);
                UpdateComputedBackground();
            }
        } = 1.0;

        public Brush ComputedBackground
        {
            get => field;
            private set => SetProperty(ref field, value);
        }

        private void UpdateComputedBackground()
        {
            // ① RGB からベース Color
            byte r = ToByte(BackgroundRed);
            byte g = ToByte(BackgroundGreen);
            byte b = ToByte(BackgroundBlue);


            // ② HSL に変換 → Hue += HueOffset, S = Saturation, L = Lightness
            var (h, s, l) = RgbToHsl(r, g, b);

            // ②' スライダー値を反映
            h = NormalizeHue(h + BackgroundHueOffset);  // 色相を足す
            s = Clamp01(Saturation);                    // 彩度（0～1）
            l = Clamp01(Lightness);

            // ③ RGB に戻す
            Color computed = HslToRgb(h, s, l);

            ComputedBackground = new SolidColorBrush(computed);

            ResultColor = ComputedBackground;
            }

        private byte ToByte(double value)
    => (byte)Math.Clamp(Math.Round(value), 0, 255);

        private  double Clamp01(double value)
    => Math.Clamp(value, 0, 1);
        private  double NormalizeHue(double hue)
            => (hue % 360 + 360) % 360;

        /// <summary>
        /// 忘れっぽい....
        /// </summary>

        public double BackgroundHueOffset { get; set; }

        ///休憩
        ///
        private (double H, double S, double L) RgbToHsl(byte r, byte g, byte b)
        {
            double rf = r / 255.0;
            double gf = g / 255.0;
            double bf = b / 255.0;

            double max = Math.Max(rf, Math.Max(gf, bf));
            double min = Math.Min(rf, Math.Min(gf, bf));
            double delta = max - min;

            double h = 0;
            double s = 0;
            double l = (max + min) / 2.0;

            if (delta > 1e-6)
            {
                s = l > 0.5
                    ? delta / (2.0 - max - min)
                    : delta / (max + min);

                if (max == rf)
                    h = (gf - bf) / delta + (gf < bf ? 6 : 0);
                else if (max == gf)
                    h = (bf - rf) / delta + 2;
                else
                    h = (rf - gf) / delta + 4;

                h *= 60.0; // 0～360
            }

            return (h, s, l);
        }

        private Color HslToRgb(double h, double s, double l)
        {
            
            h = NormalizeHue(h);
            s = Clamp01(s);
            l = Clamp01(l);

            double c = (1 - Math.Abs(2 * l - 1)) * s;
            double x = c * (1 - Math.Abs((h / 60.0) % 2 - 1));
            double m = l - c / 2;

            double rf = 0, gf = 0, bf = 0;

            if (h < 60) { rf = c; gf = x; bf = 0; }
            else if (h < 120) { rf = x; gf = c; bf = 0; }
            else if (h < 180) { rf = 0; gf = c; bf = x; }
            else if (h < 240) { rf = 0; gf = x; bf = c; }
            else if (h < 300) { rf = x; gf = 0; bf = c; }
            else { rf = c; gf = 0; bf = x; }

            byte r = ToByte((rf + m) * 255);
            byte g = ToByte((gf + m) * 255);
            byte b = ToByte((bf + m) * 255);

            return Color.FromRgb(r, g, b);
        }


        public BackColorViewModels()
        {
            BackgroundRed = 253;
            BackgroundGreen = 238;
            BackgroundBlue = 240;
            BackgroundHueOffset = 0;
            Saturation = 1.0;
            
            UpdateComputedBackground();
        }
    }
}
