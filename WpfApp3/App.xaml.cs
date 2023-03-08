using System.Windows;


namespace HaruaConvert
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        
        protected override void OnStartup(StartupEventArgs e)
        {
            var view = new MainWindow()
            {
                DataContext = new MainWindow()
            };

            
        }
    }
}
