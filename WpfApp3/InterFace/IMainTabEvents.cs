using HaruaConvert.HaruaServise;
using HaruaConvert.Parameter;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using WpfApp3.Parameter;

namespace HaruaConvert.HaruaInterFace
{
      
        public interface IMainTabEvents
        {
           public void Directory_DropButon_Click(object sender, RoutedEventArgs e);
           public void Convert_DropButton_Click(object sender, RoutedEventArgs e);
        }

     

     public class Directory_ClickProcedure : IMainTabEvents
    {
        MainWindow main ;
        public Directory_ClickProcedure(MainWindow _main)         
        { 
        main = _main;        
        }

        public void Directory_DropButon_Click(object sender, RoutedEventArgs e)
        {

            //  ParamField.identification_Obj = sender;
            ClassShearingMenbers.ButtonName = ((Button)sender).Name;




            if (!main.paramField.isExitProcessed )
            {
                MessageBox.Show("ffmpeg.exeが実行中です");

                return;
            }



            using (CommonOpenDialogClass ofc = new CommonOpenDialogClass(false, ParamField.Maintab_InputDirectory))
            {

                var result = ofc.CommonOpens();

                if (result == CommonFileDialogResult.Ok)  //Selected OK
                {


                    main.paramField.setFile = ofc.opFileName;
                    main.harua_View.SourcePathText = main.paramField.setFile;
                    main.SourcePathLabel.Text = main.harua_View.SourcePathText;
                    main.SourcePathLabel.ToolTip = main.harua_View.SourcePathText;


                    // ParamField.ConvertDirectory = paramField.setFile;

                    main.Drop_Label.Content = "変換";

                    ParamField.Maintab_InputDirectory = Path.GetDirectoryName(main.paramField.setFile);


                    //Update Maintab_InputDirectory
                    ParamField.Maintab_InputDirectory = Path.GetDirectoryName(ofc.opFileName);

                    
                }
                else //Selected Cancel
                {
                    return;
                }
                    
                MediaInfoService media = new MediaInfoService(main);

                media.displayMediaInfo(main.paramField.setFile);
                //  ParamField.ConvertDirectory = ofc.opFileName;
            }



        }



        public void Convert_DropButton_Click(object sender, RoutedEventArgs e)
        {

            if (!main.paramField.isExitProcessed)
            {
                MessageBox.Show("ffmpwg.exeが実行中ですよ");
                return;
            }



            ClassShearingMenbers.ButtonName = ((Button)sender).Name;
            //var runinng = Process.GetProcessesByName("ffmpeg.exe");
            if (!string.IsNullOrEmpty(main.paramField.setFile))
            {
                //Convert Process Improvement Part
               

               main.paramField.isExitProcessed = main.FileConvertExec(main.paramField.setFile, sender);


                
               main.Lw.Activate();

            }
            else
            {

               Directory_DropButon_Click(sender, e);
               main.harua_View.SourcePathText =  main.paramField.setFile;

            }




        }

    }





    public interface IMethods 
        {
            CommonFileDialogResult CommonOpens();

        }
        



    }

   
