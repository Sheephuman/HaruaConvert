using HaruaConvert.userintarface;
using System.Windows.Input;

namespace HaruaConvert.Methods
{
    internal class GenerateSelectParaClass
    {
        public GenerateSelectParaClass ()
            {
            count = 0;

        }
        int count;

        UIManager uiManager;
        /// <summary>
        /// ParamSelectorに各種イベントを登録する 
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="main"></param>
        public void GenerateParaSelector_setPropaties(ParamSelector selector,MainWindow main)
        { 

            uiManager = new UIManager(main);
            var selchild = selector;

            selchild.Name = "ParamSelector" + $"{count}";
            count++;
            //rehgist Event
            selchild.Loaded += main.ParamSelect_Load;
            selchild.SlectorRadio.Checked += main.SlectorRadio_Checked;
            selchild.ArgumentEditor.TextChanged += main.ArgumentEditor_TextChanged;
            
              

            selchild.KeyUp += uiManager.InvisibleText_KeyDown;
            selchild.LostFocus += main.InvisibleText_LostFocus;
            selchild.SelectorLabelCon.MouseDoubleClick += main.Tb_MouseDoubleClick;
            selchild.SelectorLabelCon.MouseMove += main.ParamSelector_MouseEnter;

            TextCompositionManager.AddPreviewTextInputHandler(selchild.invisibleText, main.OnPreviewTextInput);
           // TextCompositionManager.AddPreviewTextInputUpdateHandler(selchild.invisibleText, mainWindow.OnPreviewTextInputUpdate);


         
            main.SelGenerate = count;
        }

    }



}




