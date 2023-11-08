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


        public void GenerateParaSelector_setPropaties(ParamSelector selector,MainWindow main)
        { 
            var selchild = selector;

            selchild.Name = "ParamSelector" + $"{count}";
            count++;
            //rehgist Event
            selchild.Loaded += main.ParamSelect_Load;
            selchild.SlectorRadio.Checked += main.SlectorRadio_Checked;
            selchild.ArgumentEditor.TextChanged += main.ArgumentEditor_TextChanged;
            
              

            selchild.KeyUp += main.InvisibleText_KeyDown;
            selchild.LostFocus += main.InvisibleText_LostFocus;
            selchild.SelectorLabelCon.MouseDoubleClick += main.Tb_MouseDoubleClick;
            selchild.SelectorLabelCon.MouseMove += main.ParamSelector_MouseEnter;

            TextCompositionManager.AddPreviewTextInputHandler(selchild.invisibleText, main.OnPreviewTextInput);
           // TextCompositionManager.AddPreviewTextInputUpdateHandler(selchild.invisibleText, main.OnPreviewTextInputUpdate);


         
            main.SelGenerate = count;
        }

    }



}




