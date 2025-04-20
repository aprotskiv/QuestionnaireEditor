using QuestionnaireEditorHDCCS.ViewModels;
using System.Windows;

namespace QuestionnaireEditorHDCCS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var vm = new MainWindowViewModel();
            vm.NewFileCommand.Execute(null);

            var window = new MainWindow()
            {
                DataContext = vm
            };            
            window.Show();
        }
    }

}
