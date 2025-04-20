using QuestionnaireEditorHDCCS.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace QuestionnaireEditorHDCCS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public static readonly RoutedCommand RoutedUICommand_CreateGuid = 
            new RoutedUICommand("CreateGuid",
                nameof(RoutedUICommand_CreateGuid),
                typeof(MainWindow),
                new InputGestureCollection(new InputGesture[]
                    {
                        new KeyGesture(Key.OemTilde, ModifierKeys.Control)
                    })
            );

        public static readonly RoutedCommand RoutedUICommand_AddQuestion =
            new RoutedUICommand("AddQuestion",
                nameof(RoutedUICommand_AddQuestion),
                typeof(MainWindow),
                new InputGestureCollection(new InputGesture[]
                    {
                        new KeyGesture(Key.Q, ModifierKeys.Control)
                    })
            );

        public static readonly RoutedCommand RoutedUICommand_RemoveQuestion =
            new RoutedUICommand("RemoveQuestion",
                nameof(RoutedUICommand_RemoveQuestion),
                typeof(MainWindow),
                new InputGestureCollection(new InputGesture[]
                    {
                            new KeyGesture(Key.W, ModifierKeys.Control)
                    })
            );

        private MainWindowViewModel ViewModel => (MainWindowViewModel)DataContext;


        private void ExpandClick(object sender, RoutedEventArgs e)
        {

        }

        private void ExpandAllClick(object sender, RoutedEventArgs e)
        {

        }

        private void tree1_KeyDown_1(object sender, KeyEventArgs e)
        {

        }

        private void CreateGuid_Click(object sender, ExecutedRoutedEventArgs e)
        {
            ViewModel.CreateGuidCommand.Execute(null);
        }

        private void AddQuestion_Click(object sender, ExecutedRoutedEventArgs e)
        {
            ViewModel.AddQuestionCommand.Execute(null);
        }

        private void RemoveQuestion_Click(object sender, ExecutedRoutedEventArgs e)
        {
            if (ViewModel.RemoveQuestionCommand.CanExecute(null))
            {
                ViewModel.RemoveQuestionCommand.Execute(null);
            }            
        }
    }
}