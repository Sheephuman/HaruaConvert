﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HaruaConvert.UserControls
{
    public class NumericUpDownManager
    {
        public NumericUpDownManager(TextBox _NUDTextBox)
        {
            NUDTextBox = _NUDTextBox;



        }
        readonly int minValue = 100;

        TextBox NUDTextBox;

        int currentValue;


        public void NumericSetupEventHandlers()
        {



        }

        public void ValidateAndCorrectInput(TextBox NUDTextBox, int minValue, int maxValue)
        {
            if (int.TryParse(NUDTextBox.Text, out var number))
            {
                number = Math.Clamp(number, minValue, maxValue);
                NUDTextBox.Text = number.ToString(CultureInfo.CurrentCulture);
            }
            else
            {
                // 無効な入力を扱うロジック。
                NUDTextBox.Text = minValue.ToString(CultureInfo.CurrentCulture);
            }
            // フォーカスが外れたり、特定のキーが押された時にのみ実行される
            // これにより、ユーザーがテキストを入力する度に発生するレスポンスの低下を防ぐ
        }





        //public static void NumericUpDownTextChangedProc(TextBox NUDTextBox, int startvalue, int maxvalue, int minvalue)
        //{
        //    //int number = 0;
        //    //if (NUDTextBox.Text != "")
        //    //    if (!int.TryParse(NUDTextBox.Text, out number))
        //    //        NUDTextBox.Text = startvalue.ToString(CultureInfo.CurrentCulture);


        //    ////if (number > maxvalue) NUDTextBox.Text = maxvalue.ToString(CultureInfo.CurrentCulture);
        //    ////if (number < minvalue) NUDTextBox.Text = minvalue.ToString(CultureInfo.CurrentCulture);
        //    ////NUDTextBox.SelectionStart = NUDTextBox.Text.Length;

        //    //number = Math.Clamp(number, minvalue, maxvalue);
        //    //NUDTextBox.Text = number.ToString(CultureInfo.CurrentCulture);
        //    //NUDTextBox.SelectionStart = NUDTextBox.Text.Length;
        //    //// 入力された値の範囲をチェックし、必要に応じて修正
        //}


        // NUDButtonDown と NUDButtonUP_ClickProc は、
        // 直接数値操作を行い、不要な変換を避けるように最適化
        public void NUDButtonDown(TextBox NUDTextBox, int minvalue, int interval)
        {

            int selnumber = 0;
            string intext = string.Empty;
            if (int.TryParse(NUDTextBox.Text, out var number))
            {
                selnumber = number + interval;
                intext = selnumber.ToString(CultureInfo.CurrentCulture);
            }



            if (string.IsNullOrEmpty(intext))
                NUDTextBox.Text = minValue.ToString(CultureInfo.CurrentCulture); //初期値を設定





            //int number;
            //if (NUDTextBox.Text != "") number = Convert.ToInt32(NUDTextBox.Text, CultureInfo.CurrentCulture);
            //else number = 0;

            if (selnumber >= minvalue)
                NUDTextBox.Text = selnumber.ToString(CultureInfo.CurrentCulture);

        }


        public void NUDTextBox_PreviewKeyDownProc(TextBox NUDTextBox, int minValue, int maxValue, int interval, KeyEventArgs e)
        {


            //QueryCreateWindow.qc.Dispatcher.Invoke(() =>
            // {
            int currentVal;
            if (e.Key == Key.Up)
            {
                currentVal = int.Parse(NUDTextBox.Text, CultureInfo.CurrentCulture);
                IncrementValue(NUDTextBox, maxValue, currentVal + interval);

            }
            else if (e.Key == Key.Down)
            {
                currentVal = int.Parse(NUDTextBox.Text, CultureInfo.CurrentCulture);
                DecrementValue(NUDTextBox, minValue, currentVal);


            }
            //});

        }



        private void DecrementValue(TextBox nUDTextBox, int minValue, int currentVal)
        {
            currentVal = int.Parse(nUDTextBox.Text, CultureInfo.CurrentCulture);

            currentVal -= 10;


            if (currentVal < minValue)
                currentVal = minValue; // 最大値を超えないようにする


            nUDTextBox.Text = currentVal.ToString(CultureInfo.CurrentCulture);
        }


        private void IncrementValue(TextBox nUDTextBox, int maxValue, int currentVal)
        {
            currentVal = int.Parse(nUDTextBox.Text, CultureInfo.CurrentCulture);

            currentVal += 10;


            if (currentVal > maxValue)
                currentVal = maxValue; // 最大値を超えないようにする


            nUDTextBox.Text = currentVal.ToString(CultureInfo.CurrentCulture);



        }


        public void NUDTextBox_PreviewKeyUpProc(TextBox NUDTextBox, int maxValue, int minValue, KeyEventArgs e)
        {

            string textin = string.Empty;
            int currentVal = 0;
            if (e.Key == Key.Up)
            {
                IncrementValue(NUDTextBox, maxValue, currentVal);
            }
            else if (e.Key == Key.Down)
            {
                DecrementValue(NUDTextBox, minValue, currentValue);
            }




            NUDTextBox.Text = textin;




        }



        public void NUDButtonUP(TextBox NUDTextBox, int maxvalue, int interval)
        {
            int currentVal = int.Parse(NUDTextBox.Text, CultureInfo.CurrentCulture);
            currentVal += interval;




            if (currentVal > maxvalue)
                currentVal = maxvalue; // 最大値を超えないようにする


            NUDTextBox.Text = currentVal.ToString(CultureInfo.CurrentCulture);


        }

        internal void NUDTextBox_PreviewMouseWheelProc(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            int minValue = 100; // 最小値の設定が必要
            int maxValue = 10000; // 最大値の設定が必要
            try
            {
                if (string.IsNullOrEmpty(NUDTextBox.Text))
                {
                    NUDTextBox.Text = minValue.ToString(CultureInfo.CurrentCulture);
                    return;
                }


                var delta = e.Delta;

                //currentValue = int.Parse(NUDTextBox.Text, CultureInfo.CurrentCulture);


                if (delta > 0)
                {


                    // マウスホイールが上に回転した場合、数値を増やす
                    IncrementValue(NUDTextBox, maxValue, currentValue + 10);
                }
                else if (delta < 0)
                {




                    // マウスホイールが下に回転した場合、数値を減らす
                    DecrementValue(NUDTextBox, minValue, currentValue - 10);
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show(ex.Message);
                NUDTextBox.Text = minValue.ToString(CultureInfo.CurrentCulture);
            }
            // 他のハンドラーでイベントを処理しないようにする
            e.Handled = true;
        }
    }
}