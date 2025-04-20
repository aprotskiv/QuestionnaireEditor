using AProtskiv.Questionnaires;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using Newtonsoft.Json;
using PropertyTools.Wpf;
using QuestionnaireEditorHDCCS.Model.QuestionnairesPT;
using QuestionnaireEditorHDCCS.PropertyToolsWPF;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace QuestionnaireEditorHDCCS.ViewModels
{

    public class MainWindowViewModel : QuestionnaireTreeModel
    {
        public MainWindowViewModel()
        {
            // globalization
            this.PropertyGridOperator = new PropertyGridOperator_QE();
            this.PropertyGridControlFactory = new PropertyGridControlFactory_QE();

            NewFileCommand = new RelayCommand(NewFileCommandHandler);
            DemoFileCommand = new RelayCommand(DemoFileCommandHandler);

            OpenFileCommand = new RelayCommand(OpenFileCommandHandler);
            SaveFileCommand = new RelayCommand(SaveFileCommandHandler);

            ExitCommand = new RelayCommand(ExitCommandHandler);
            AboutCommand = new RelayCommand(AboutCommandHandler);

            OptionsCommand = new RelayCommand(OptionsCommandHandler, canExecute: () => false);
            CreateGuidCommand = new RelayCommand(CreateGuidCommandHandler);
        }


        #region Properties



        private Window MainWindow => Application.Current.MainWindow;

        #endregion

        #region Commands

        public ICommand NewFileCommand { get; set; }

        public ICommand DemoFileCommand { get; set; }

        public ICommand OpenFileCommand { get; set; }

        public ICommand SaveFileCommand { get; set; }

        public ICommand ExitCommand { get; set; }

        public ICommand AboutCommand { get; set; }

        public ICommand OptionsCommand { get; set; }

        public ICommand CreateGuidCommand { get; }

        #endregion

        #region Command Handlers

        private void NewFileCommandHandler()
        {
            var qt = new QuestionnairePT
            {
                Name = "New Questionnaire",
                Id = Guid.NewGuid(),
            };

            this.Initialize(qt);

            StatusMessage = $"The draft of Questionnaire has been created.";
        }

        private void DemoFileCommandHandler()
        {
            var qt = GetDemoQuestionnaire();

            this.Initialize(qt);

            StatusMessage = $"The Demo Questionnaire has been created.";
        }

        private QuestionnairePT GetDemoQuestionnaire()
        {
            return new QuestionnairePT
            {
                Name = "Demo Questionnaire",
                Id = Guid.NewGuid(),
                PT_Authors = [
                    new AuthorPT(name:"Name Surname", contact: "Email | Line | Signal | Telegram | WeChat | etc.")
                ],
                PT_Questions = [
                    new QuestionPT
                    {
                        PT_Id = Guid.NewGuid(),
                        PT_Kind = QuestionKind.NumericalQuestion,
                        PT_Text = "How old are you?"
                    },
                    new QuestionPT
                    {
                        PT_Id = Guid.NewGuid(),
                        PT_Kind = QuestionKind.MultipleSelectQuestion,
                        PT_Text = "What pet do you have?",
                        AnswerOptions =
                        [
                            new AnswerOption
                            {
                                Id = "dog" ,
                                Text = "Dog"
                            },
                            new AnswerOption
                            {
                                Id = "cat" ,
                                Text = "Cat"
                            },
                            new AnswerOption
                            {
                                Id = "fish" ,
                                Text = "Fish"
                            },
                        ]
                    },
                    new QuestionPT
                    {
                        PT_Id = Guid.NewGuid(),
                        PT_Kind = QuestionKind.MatchingMatrixQuestion,
                        PT_Text = "Please match countries and their capitals",
                        QuestionOptions =
                        [
                            new QuestionOption{
                                Id = "Thailand",
                                Text = "Thailand",
                            },
                            new QuestionOption{
                                Id = "Indonesia",
                                Text = "Indonesia",
                            },
                            new QuestionOption{
                                Id = "India",
                                Text = "India",
                            },
                        ],
                        AnswerOptions =
                        [
                            new AnswerOption
                            {
                                Id = "Jakarta",
                                Text = "Jakarta"
                            },
                            new AnswerOption
                            {
                                Id = "Delhi",
                                Text = "Delhi"
                            },
                            new AnswerOption
                            {
                                Id = "Bangkok",
                                Text = "Bangkok"
                            },
                        ]
                    },
                ],
            };
        }

        private void OpenFileCommandHandler()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                CheckFileExists = true,
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog(MainWindow) == true)
            {
                var qFilename = openFileDialog.FileName;
                var qContent = File.ReadAllText(qFilename);
                var questionnaire = JsonConvert.DeserializeObject<QuestionnairePT>(qContent);
                if (questionnaire != null)
                {
                    this.Initialize(questionnaire);

                    StatusMessage = $"File has been loaded successfully." + qFilename;
                }
            }
        }

        private void SaveFileCommandHandler()
        {
            var questionnaire = RootModel?.Tag as QuestionnairePT;
            if (questionnaire == null)
                return;


            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*"
            };

            if (saveFileDialog.ShowDialog(MainWindow) == true)
            {
                var qFilename = saveFileDialog.FileName;
                var qContent = JsonConvert.SerializeObject(questionnaire, Formatting.Indented);
                File.WriteAllText(qFilename, contents: qContent);

                StatusMessage = $"File has been saved successfully. " + qFilename;
            }
        }

        private void AboutCommandHandler()
        {
            AboutDialog aboutDialog = new AboutDialog(MainWindow);
            aboutDialog.ShowDialog();
        }

        private void OptionsCommandHandler()
        {

        }

        private void ExitCommandHandler()
        {
            var dialogResult = MessageBox.Show("Do you want to save changes?", "Application closing",
                button: MessageBoxButton.YesNoCancel,
                icon: MessageBoxImage.Question);

            switch (dialogResult)
            {
                case MessageBoxResult.Cancel:
                    return;

                case MessageBoxResult.Yes:
                    SaveFileCommandHandler();
                    Application.Current.Shutdown();
                    break;

                case MessageBoxResult.No:
                    Application.Current.Shutdown();
                    break;
            }
        }

        private void CreateGuidCommandHandler()
        {
            Clipboard.SetText(Guid.NewGuid().ToString());

            StatusMessage = $"New GUID has been generated. You can paste it (CTRL+V) into target Id field.";
        }

        #endregion

        #region PropertyTools / Grid

        public IPropertyGridOperator PropertyGridOperator { get; }

        public IPropertyGridControlFactory PropertyGridControlFactory { get; }

        #endregion
    }
}
